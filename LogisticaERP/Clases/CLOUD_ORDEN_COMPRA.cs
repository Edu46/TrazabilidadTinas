using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Web;

namespace LogisticaERP.Clases
{

    public class CLOUD_ORDEN_COMPRA
    {
        public OrdenCompraCloud OrdenCompra { get; set; }

        public CLOUD_ORDEN_COMPRA()
        {
            OrdenCompra = new OrdenCompraCloud();
        }

        public class OrdenCompraCloud 
        {        
            public Int64 procurementBusinessId { get; set;}
            public string procurementBusinessUnit { get; set;}
		    public string requisitioningBusinessUnit { get; set; }
		    public string buyerEmail { get; set; }
		    public string supplier { get; set; }
		    public string currencyCode { get; set; }
		    public string documentDescription {get;set;}
		    public string interfaceSourceCode {get;set;}
		    public string referenceNumber {get;set;}
		    public string ledgerName {get;set;}
            public string orderNumber { get; set; }
            public decimal pOHeaderId { get; set; }
            public string resultado { get; set; }
            public string mensaje { get; set; }
            public List<Linea> Line { get; set;}

            public class Linea {
                public string itemNumber  { get; set;}
			    public string itemDescription { get; set;}
			    public string unitOfMeasure { get; set;}
			    public decimal quantity { get; set;}
                public decimal price { get; set; }
                public string atributo1 { get; set; }
                public List<Entrada> Schedule { get; set; }

                public class Entrada
                {
                    public string shipToOrganizationCode { get; set; }
                    public List<Distribucion> Distribution { get; set; }

                    public class Distribucion {
                        public string chargeAccount  { get; set; }
                        public string budgetDate { get; set;}
                        public string requesterEmail { get; set; }
                    }
                }

            }
        }


        public CLOUD_ORDEN_COMPRA GrabarOrdenCompra(string jsonOrdenCompra)
        {
            CLOUD_ORDEN_COMPRA orden = null;
            string json = "";

            try
            {

                HttpContent inputContent = new StringContent(jsonOrdenCompra, Encoding.UTF8, "application/json");
                HttpResponseMessage response = ClaseHttpCliente.cliente.PostAsync("/ordencompra_api/v1/OrdenCompra", inputContent).GetAwaiter().GetResult();

                json = response.Content.ReadAsStringAsync().Result;
                var jsonObj = JsonConvert.DeserializeObject<JObject>(json).First.First;
                orden = Newtonsoft.Json.JsonConvert.DeserializeObject<CLOUD_ORDEN_COMPRA>(json);

                if (orden.OrdenCompra.resultado == null)
                    throw new Exception(orden.OrdenCompra.mensaje);

            }
            catch (Exception ex)
            {
                throw ex;
            }

            return orden;
        }

    



    }
}