using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Web;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace LogisticaERP.Clases.RecepcionarASN.OracleCloud
{
    public class SOAPBase
    {
        private static readonly string _endpointBase = ConfigurationManager.AppSettings["OracleCloudEndPoint"];
        private static readonly string _endpointUser = ConfigurationManager.AppSettings["OracleCloudUser"];
        private static readonly string _endpointPassword = ConfigurationManager.AppSettings["OracleCloudPassword"];
        private static readonly string _endpointSOAPReport = ConfigurationManager.AppSettings["OracleCloudEndPointSOAPReport"];

        public T Send<T>(string xml)
        {
            try
            {
                if (xml == null)
                    throw new Exception("No se ha proporcionado ningún valor a la propiedad \"xml\"");

                HttpWebRequest request = WebRequest(_endpointSOAPReport);
                var xmlDocument = new XmlDocument();
                xmlDocument.LoadXml(xml);

                using (var stream = request.GetRequestStream())
                    xmlDocument.Save(stream);

                using (var response = request.GetResponse())
                using (var rd = new StreamReader(response.GetResponseStream()))
                {
                    string result = rd.ReadToEnd();
                    var doc = XDocument.Parse(result);
                    return GetResult<T>(doc);
                }
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

        private static HttpWebRequest WebRequest(string URL, int maxTimeMilliseconds = 120000)
        {
            ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls11 | System.Net.SecurityProtocolType.Tls12;
            var webRequest = (HttpWebRequest)HttpWebRequest.Create(URL);
            webRequest.Timeout = maxTimeMilliseconds;
            webRequest.ReadWriteTimeout = maxTimeMilliseconds;
            webRequest.Method = "POST";
            webRequest.ContentType = "application/soap+xml;charset=utf-8";
            string encoded = System.Convert.ToBase64String(System.Text.Encoding.GetEncoding("ISO-8859-1").GetBytes(_endpointUser + ":" + _endpointPassword));
            webRequest.Headers.Add(HttpRequestHeader.Authorization, "Basic " + encoded);

            return webRequest;
        }

        private T GetResult<T>(XDocument xmlDoc)
        {
            XNamespace xsiNs = "http://xmlns.oracle.com/oxp/service/PublicReportService";
            string reportBytes = xmlDoc.Descendants(xsiNs + "reportBytes").FirstOrDefault().Value;
            byte[] data = System.Convert.FromBase64String(reportBytes);
            string base64Decoded = System.Text.Encoding.UTF8.GetString(data);
            var serializer = new XmlSerializer(typeof(T));
            using (TextReader tr = new StringReader(base64Decoded))
            {
                return (T)serializer.Deserialize(tr);
            }
        }
    }
}