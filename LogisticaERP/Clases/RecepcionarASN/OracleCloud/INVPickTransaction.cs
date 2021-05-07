using LogisticaERP.Clases.RecepcionarASN.OracleCloud.DTO;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Configuration;

namespace LogisticaERP.Clases.RecepcionarASN.OracleCloud
{
    public class INVPickTransaction
    {
        private readonly string _endpointUser = WebConfigurationManager.AppSettings["OracleCloudUser"];
        private readonly string _endpointPassword = WebConfigurationManager.AppSettings["OracleCloudPassword"];
        private readonly string _endpoint = WebConfigurationManager.AppSettings["OracleCloudEndPoint"];
        private readonly string _endpointRestCreatePickTransaction = WebConfigurationManager.AppSettings["OracleCloudEndPointRESTInventoryCreatePickTransactions"];
        private const int _timeOutValue = 1200000; //20 min.

        public PickTransactionResponse.PickTransaction CreatePickTransactions(PickTransaction pickTransactions)
        {
            try
            {
                ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;
                string requestUriString = string.Format("{0}{1}", _endpoint, _endpointRestCreatePickTransaction);
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(requestUriString);
                request.ContentType = "application/vnd.oracle.adf.resourceitem+json"; //"text/xml" or application/soap+xml for SOAP 1.2 or application/json for JSON

                request.Method = "POST";
                request.KeepAlive = false;
                request.Timeout = _timeOutValue;
                request.ReadWriteTimeout = _timeOutValue;
                string encoded = System.Convert.ToBase64String(System.Text.Encoding.GetEncoding("ISO-8859-1").GetBytes(_endpointUser + ":" + _endpointPassword));
                request.Headers.Add("Authorization", "Basic " + encoded);

                string payload = JsonConvert.SerializeObject(pickTransactions);
                byte[] byteArray = Encoding.UTF8.GetBytes(payload);
                request.ContentLength = byteArray.Length;

                Stream requestStream = request.GetRequestStream();
                requestStream.Write(byteArray, 0, byteArray.Length);
                requestStream.Close();

                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                var responseStream = response.GetResponseStream();
                var reader = new StreamReader(responseStream);
                string result = reader.ReadToEnd();

                var pickTransaction = JsonConvert.DeserializeObject<PickTransactionResponse.PickTransaction>(result);

                // clean
                reader.Close();
                requestStream.Close();
                responseStream.Close();
                response.Close();

                return pickTransaction;
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