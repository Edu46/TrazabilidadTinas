using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using System.Web;

namespace LogisticaERP.Clases
{
    public class ClaseHttpClienteOracleCloud
    {
        public static HttpClient cliente { get; set; }


        public ClaseHttpClienteOracleCloud()
        {
            if (cliente != null)
                return;

            cliente = new HttpClient();
            cliente.BaseAddress = new Uri(System.Configuration.ConfigurationManager.AppSettings["OracleCloudEndPoint"]);
            cliente.DefaultRequestHeaders.Accept.Clear();
            TimeSpan interval = TimeSpan.FromSeconds(300);
            cliente.Timeout = interval;
            cliente.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            string encoded = System.Convert.ToBase64String(System.Text.Encoding.GetEncoding("ISO-8859-1").GetBytes(System.Configuration.ConfigurationManager.AppSettings["OracleCloudUser"] + ":" + System.Configuration.ConfigurationManager.AppSettings["OracleCloudPassword"]));
            cliente.DefaultRequestHeaders.Add("Authorization", "Basic " + encoded);

            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
        }

    }
}