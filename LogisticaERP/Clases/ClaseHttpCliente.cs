using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using System.Web;

namespace LogisticaERP.Clases
{
    public class ClaseHttpCliente 
    {
        public static HttpClient cliente { get; set; }

        public ClaseHttpCliente()
        {
            if (cliente != null)
                return;

            cliente = new HttpClient();
            cliente.BaseAddress = new Uri(System.Configuration.ConfigurationManager.AppSettings["DominioIIB"]);
            cliente.DefaultRequestHeaders.Accept.Clear();
            TimeSpan interval = TimeSpan.FromSeconds(300);
            cliente.Timeout = interval;
            cliente.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        }

        public ClaseHttpCliente(string ambiente)
        {
            if (cliente != null)
                return;

            cliente = new HttpClient();
            cliente.BaseAddress = new Uri(ambiente);
            cliente.DefaultRequestHeaders.Accept.Clear();
            TimeSpan interval = TimeSpan.FromSeconds(9000);
            cliente.Timeout = interval;
            cliente.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

    }
}