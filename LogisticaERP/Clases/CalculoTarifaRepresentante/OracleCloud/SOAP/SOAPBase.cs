using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Configuration;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace LogisticaERP.Clases.CalculoTarifaRepresentante.OracleCloud
{
    public class SOAPBase
    {
        private static readonly string _endpointBase = WebConfigurationManager.AppSettings["OracleCloudEndPoint"];
        private static readonly string _endpointUser = WebConfigurationManager.AppSettings["OracleCloudUser"];
        private static readonly string _endpointPassword = WebConfigurationManager.AppSettings["OracleCloudPassword"];
        private static readonly string _endpointSOAPPurchaseOrder = WebConfigurationManager.AppSettings["OracleCloudEndPointSOAPProcurementPurchaseOrderV2"];
        private static readonly string _endpointSOAPAccountCombination = WebConfigurationManager.AppSettings["OracleCloudEndPointSOAPFinancialsAccountCombinations"];

        public T Send<T>(string xml, Service service, string soapAction = null)
        {
            try
            {
                if (xml == null)
                    throw new Exception("No se ha proporcionado ningún valor a la propiedad \"xml\"");

                var url = GetServiceEndpoint(service);
                HttpWebRequest request = WebRequest(url);
                var xmlDocument = new XmlDocument();
                xmlDocument.LoadXml(xml);

                if (!string.IsNullOrEmpty(soapAction))
                    request.Headers.Add("SOAPAction", soapAction);

                using (var stream = request.GetRequestStream())
                    xmlDocument.Save(stream);

                using (var response = request.GetResponse())
                using (var rd = new StreamReader(response.GetResponseStream()))
                {
                    string result = rd.ReadToEnd();
                    var doc = XDocument.Parse(result);
                    return GetResult<T>(service, doc);
                }
            }
            catch (WebException webException)
            {
                var resp = new StreamReader(webException.Response.GetResponseStream()).ReadToEnd();
                switch (service.ToString())
                {
                    case "PurchaseOrderService":
                        var docPOS = XDocument.Parse(resp);
                        XNamespace env = "http://schemas.xmlsoap.org/soap/envelope/";
                        string message = docPOS
                            .Element(env + "Envelope")
                            .Element(env + "Body")
                            .Element(env + "Fault")
                            .Element("faultstring")
                            .Value;
                        throw new Exception(message);
                    default:
                        throw new Exception(string.IsNullOrEmpty(resp) ? webException.Message : resp);
                }
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
            webRequest.ContentType = "text/xml;charset=\"utf-8\"";
            string encoded = System.Convert.ToBase64String(System.Text.Encoding.GetEncoding("ISO-8859-1").GetBytes(_endpointUser + ":" + _endpointPassword));
            webRequest.Headers.Add(HttpRequestHeader.Authorization, "Basic " + encoded);

            return webRequest;
        }

        private T GetResult<T>(Service service, XDocument xmlDoc)
        {
            XNamespace env = string.Empty;
            XNamespace ns0 = string.Empty;
            XNamespace ns0_2 = string.Empty;
            XNamespace ns1 = string.Empty;
            XNamespace ns2 = string.Empty;
            switch (service.ToString())
            {
                case "AccountConbinationService":
                    env = "http://schemas.xmlsoap.org/soap/envelope/";
                    ns0 = "http://xmlns.oracle.com/apps/financials/generalLedger/accounts/codeCombinations/accountCombinationService/types/";
                    ns1 = "http://xmlns.oracle.com/apps/financials/generalLedger/accounts/codeCombinations/accountCombinationService/";
                    ns2 = "http://xmlns.oracle.com/apps/financials/generalLedger/accounts/codeCombinations/accountCombinationService/types/";
                    string status = xmlDoc
                        .Element(env + "Envelope")
                        .Element(env + "Body")
                        .Element(ns0 + "validateAndCreateAccountsResponse")
                        .Element(ns2 + "result")
                        .Element(ns1 + "Status")
                        .Value;
                    string message = xmlDoc
                        .Element(env + "Envelope")
                        .Element(env + "Body")
                        .Element(ns0 + "validateAndCreateAccountsResponse")
                        .Element(ns2 + "result")
                        .Element(ns1 + "Error")
                        .Value;
                    string ccID = xmlDoc
                        .Element(env + "Envelope")
                        .Element(env + "Body")
                        .Element(ns0 + "validateAndCreateAccountsResponse")
                        .Element(ns2 + "result")
                        .Element(ns1 + "CcId")
                        .Value;

                    return (T)Convert.ChangeType(new Tuple<string, string, string>(status, message, ccID), typeof(T));
                case "PurchaseOrderService":
                    env = "http://schemas.xmlsoap.org/soap/envelope/";
                    ns0 = "http://xmlns.oracle.com/apps/prc/po/editDocument/purchaseOrderServiceV2/types/";
                    ns0_2 = "http://xmlns.oracle.com/apps/prc/po/editDocument/purchaseOrderServiceV2/";
                    ns1 = "http://xmlns.oracle.com/apps/prc/po/editDocument/purchaseOrderServiceV2/types/";
                    string poHeaderID = xmlDoc
                        .Element(env + "Envelope")
                        .Element(env + "Body")
                        .Element(ns0 + "createPurchaseOrderResponse")
                        .Element(ns1 + "result")
                        .Element(ns0_2 + "POHeaderId")
                        .Value;
                    string orderNumber = xmlDoc
                        .Element(env + "Envelope")
                        .Element(env + "Body")
                        .Element(ns0 + "createPurchaseOrderResponse")
                        .Element(ns1 + "result")
                        .Element(ns0_2 + "OrderNumber")
                        .Value;

                    return (T)Convert.ChangeType(new Tuple<string, string>(poHeaderID, orderNumber), typeof(T));
                default:
                    var serializer = new XmlSerializer(typeof(T));
                    using (TextReader tr = new StringReader(xmlDoc.ToString()))
                    {
                        return (T)serializer.Deserialize(tr);
                    }
            }
        }

        public string GetServiceEndpoint(Service service)
        {
            string url = string.Empty;
            switch (service.ToString())
            {
                case "AccountConbinationService":
                    url = _endpointSOAPAccountCombination;
                    break;
                case "PurchaseOrderService":
                    url = _endpointSOAPPurchaseOrder;
                    break;
                default:
                    url = string.Empty;
                    break;
            }
            return string.Format("{0}{1}", _endpointBase, url);
        }

        public enum Service
        {
            AccountConbinationService,
            PurchaseOrderService
        }
    }
}