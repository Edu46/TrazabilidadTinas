using LogisticaERP.Clases.RecepcionarASN.OracleCloud.DTO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Configuration;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace LogisticaERP.Clases.RecepcionarASN.OracleCloud
{
    public class INVOrganization
    {
        private readonly string _endpointBase = WebConfigurationManager.AppSettings["OracleCloudEndPoint"];
        private readonly string _endpointUser = WebConfigurationManager.AppSettings["OracleCloudUser"];
        private readonly string _endpointPassword = WebConfigurationManager.AppSettings["OracleCloudPassword"];
        private readonly string _endpointSOAPReport = WebConfigurationManager.AppSettings["OracleCloudEndPointSOAPReport"];
        private const int _timeOutValue = 1200000; //20 min.

        public List<Organization> ObtenerOrganizaciones(decimal businessUnitID)
        {
            try
            {
                var organizations = new List<Organization>();
                ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(_endpointBase + _endpointSOAPReport);
                request.ContentType = "application/soap+xml;charset=utf-8"; //"text/xml" or application/soap+xml for SOAP 1.2
                request.Method = "POST";
                request.KeepAlive = false;
                request.Timeout = _timeOutValue;
                request.ReadWriteTimeout = _timeOutValue;
                string encoded = System.Convert.ToBase64String(System.Text.Encoding.GetEncoding("ISO-8859-1").GetBytes(_endpointUser + ":" + _endpointPassword));
                request.Headers.Add("Authorization", "Basic " + encoded);

                string payload = 
                    @"<soap:Envelope xmlns:soap=""http://www.w3.org/2003/05/soap-envelope"" xmlns:pub=""http://xmlns.oracle.com/oxp/service/PublicReportService"">
                        <soap:Header/>
                        <soap:Body>
                            <pub:runReport>
                                <pub:reportRequest>
                                    <pub:parameterNameValues>
                                        <pub:item>
                                            <pub:name>P_BUSINESS_UNIT_ID</pub:name>
                                            <pub:values>
                                                <pub:item>{0}</pub:item>
                                            </pub:values>
                                        </pub:item>
                                    </pub:parameterNameValues>
                                    <pub:reportAbsolutePath>/Custom/GRUPO_PINSA/Integraciones/XXGPIN_INV_ORGANIZATIONS.xdo</pub:reportAbsolutePath>
                                    <pub:sizeOfDataChunkDownload>-1</pub:sizeOfDataChunkDownload>
                                </pub:reportRequest>
                            </pub:runReport>
                        </soap:Body>
                    </soap:Envelope>";

                payload = string.Format(payload, businessUnitID.ToString());
                byte[] byteArray = Encoding.UTF8.GetBytes(payload);
                request.ContentLength = byteArray.Length;

                Stream requestStream = request.GetRequestStream();
                requestStream.Write(byteArray, 0, byteArray.Length);
                requestStream.Close();

                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                var responseStream = response.GetResponseStream();
                var reader = new StreamReader(responseStream);
                XNamespace xsiNs = "http://xmlns.oracle.com/oxp/service/PublicReportService";
                var doc = XDocument.Load(reader);
                string reportBytes = doc.Descendants(xsiNs + "reportBytes").FirstOrDefault().Value;
                byte[] data = System.Convert.FromBase64String(reportBytes);
                string base64Decoded = System.Text.Encoding.UTF8.GetString(data);//System.Text.ASCIIEncoding.ASCII.GetString(data);

                var serializer = new XmlSerializer(typeof(DSOrganization));
                using (TextReader tr = new StringReader(base64Decoded))
                {
                    organizations = (serializer.Deserialize(tr) as DSOrganization).Organizations;
                }

                // clean
                reader.Close();
                requestStream.Close();
                responseStream.Close();
                response.Close();
                return organizations;
            }
            catch (WebException webException)
            {
                var resp = new StreamReader(webException.Response.GetResponseStream()).ReadToEnd();
                throw new Exception(string.IsNullOrEmpty(resp) ? webException.Message : resp);
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }
    }
}