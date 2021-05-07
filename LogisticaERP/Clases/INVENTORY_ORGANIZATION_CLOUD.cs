using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Text;
using System.Net;
using System.Web.Configuration;
using System.IO;

namespace LogisticaERP.Clases
{
    public class INVENTORY_ORGANIZATION_CLOUD
    {
        private readonly string _endpointUser = WebConfigurationManager.AppSettings["OracleCloudUser"];
        private readonly string _endpointPassword = WebConfigurationManager.AppSettings["OracleCloudPassword"];
        private readonly string _endpoint = WebConfigurationManager.AppSettings["OracleCloudEndPoint"];
        private const int _timeOutValue = 1200000; //20 min.

        public OrganizacionInv invOrganization { get; set; }
       

        public INVENTORY_ORGANIZATION_CLOUD()
        {
            invOrganization = new OrganizacionInv();
        }

        public class OrganizacionInv 
        {
            public List<invOrg> invOrgParameters { get; set; }

            public OrganizacionInv()
            {
                invOrgParameters = new List<invOrg>();
            }
        }

        public class invOrg
        {
            public string AllowNegativeOnhandTransactionsFlag { get; set; }
            public bool NegativeInvReceiptFlag { get; set; }

            public invOrg()
            {
                AllowNegativeOnhandTransactionsFlag = "";
                NegativeInvReceiptFlag = false;
            }

        }


        public bool ActualizarAlmacen(OrganizacionInv invOrg, decimal organizacionId)
        {
            try
            {
                bool resultado = false;

                ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;
          
                string requestUriString = string.Format("{0}{1}", _endpoint, "/fscmRestApi/resources/11.13.18.05/inventoryOrganizations" + "/" + organizacionId.ToString());
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(requestUriString);
                request.ContentType = "application/json; charset=utf-8"; //"text/xml" or application/soap+xml for SOAP 1.2 or application/json for JSON

                request.Method = "PATCH";
                request.KeepAlive = false;
                request.Timeout = _timeOutValue;
                request.ReadWriteTimeout = _timeOutValue;
                string encoded = System.Convert.ToBase64String(System.Text.Encoding.GetEncoding("ISO-8859-1").GetBytes(_endpointUser + ":" + _endpointPassword));
                request.Headers.Add("Authorization", "Basic " + encoded);

                string payload = JsonConvert.SerializeObject(invOrg);
                byte[] byteArray = Encoding.UTF8.GetBytes(payload);
                request.ContentLength = byteArray.Length;

                Stream requestStream = request.GetRequestStream();
                requestStream.Write(byteArray, 0, byteArray.Length);
                requestStream.Close();

                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                var responseStream = response.GetResponseStream();
                var reader = new StreamReader(responseStream);
                string result = reader.ReadToEnd();

                var jsonObj = JsonConvert.DeserializeObject<JObject>(result).First;

                if (jsonObj != null)
                    resultado = true;

                reader.Close();
                requestStream.Close();
                responseStream.Close();
                response.Close();

                return resultado;
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


        public decimal? GetOrganization(string almacen)
        {
            string json = "";
            decimal? OrganizationId;

            try
            {

                ClaseHttpClienteOracleCloud cliente = new ClaseHttpClienteOracleCloud();

                var response = ClaseHttpClienteOracleCloud.cliente.GetAsync("/fscmRestApi/resources/11.13.18.05/inventoryOrganizations" + "?q=OrganizationCode=" + almacen).GetAwaiter().GetResult();

                if (response.IsSuccessStatusCode)
                {
                    json = response.Content.ReadAsStringAsync().Result;
                    var jsonObj = JsonConvert.DeserializeObject<JObject>(json).First.First.First;
                                        
                    OrganizationId = Convert.ToDecimal(jsonObj["OrganizationId"]);

                }
                else
                    throw new Exception(response.ReasonPhrase);
                
                return OrganizationId;

            }
            catch (Exception exception)
            {
                throw exception;
            }

        } 



    }
}