using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Configuration;

namespace LogisticaERP.Clases.CalculoTarifaRepresentante.OracleCloud.REST
{
    public class POSupplierSite
    {
        private static readonly string _endpointBase = WebConfigurationManager.AppSettings["OracleCloudEndPoint"];
        private static readonly string _endpointUser = WebConfigurationManager.AppSettings["OracleCloudUser"];
        private static readonly string _endpointPassword = WebConfigurationManager.AppSettings["OracleCloudPassword"];
        private readonly string _endpointRESTGetOneSupplierSite = WebConfigurationManager.AppSettings["OracleCloudEndPointRESTProcurementGetOneSupplierSite"];
        private const int _timeOutValue = 1200000; //20 min.

        public POSupplierSite() { }

        public string GetSupplierSite(long supplierID, string prucurementBU)
        {
            try
            {
                ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;
                string requestUriString = _endpointBase + _endpointRESTGetOneSupplierSite
                    .Replace("{SupplierID}", supplierID.ToString())
                    .Replace("{ProcurementBU}", prucurementBU);
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(requestUriString);
                request.ContentType = "application/vnd.oracle.adf.resourcecollection+json";
                request.Method = "GET";
                request.KeepAlive = false;
                request.Timeout = _timeOutValue;
                request.ReadWriteTimeout = _timeOutValue;
                string encoded = System.Convert.ToBase64String(System.Text.Encoding.GetEncoding("ISO-8859-1").GetBytes(_endpointUser + ":" + _endpointPassword));
                request.Headers.Add("Authorization", "Basic " + encoded);

                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                var responseStream = response.GetResponseStream();
                var reader = new StreamReader(responseStream);

                var supplierSite = JsonConvert.DeserializeObject<SupplierSite.Root>(reader.ReadToEnd());

                // clean
                reader.Close();
                responseStream.Close();
                response.Close();

                return supplierSite.Items.Any() ?
                    supplierSite.Items.FirstOrDefault().SupplierSite :
                    string.Empty;
            }
            catch (WebException webException)
            {
                throw new Exception(webException.Message);
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        #region Classes

        public class SupplierSite
        {
            public class Root
            {
                [JsonProperty("items")]
                public List<Item> Items { get; set; }
                [JsonProperty("count")]
                public long Count { get; set; }
                [JsonProperty("hasMore")]
                public bool HasMore { get; set; }
                [JsonProperty("limit")]
                public long Limit { get; set; }
                [JsonProperty("offset")]
                public long Offset { get; set; }
                [JsonProperty("links")]
                public List<Link> Links { get; set; }
            }

            public class Item
            {
                [JsonProperty("SupplierSite")]
                public string SupplierSite { get; set; }
            }

            public class Link
            {
                [JsonProperty("rel")]
                public string Rel { get; set; }
                [JsonProperty("href")]
                public Uri Href { get; set; }
                [JsonProperty("name")]
                public string Name { get; set; }
                [JsonProperty("kind")]
                public string Kind { get; set; }
            }
        }

        #endregion
    }
}