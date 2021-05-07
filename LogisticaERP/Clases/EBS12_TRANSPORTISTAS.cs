using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;


namespace LogisticaERP.Clases
{
    public class EBS12_TRANSPORTISTAS
    {
        public Transportista Transportistas { get; set; }

        public class Transportista
        {
            public string resultado { get; set; }
            public string mensaje { get; set; }
            public List<ItemsTransportistas> items { get; set; }

            [Serializable()]
            public class ItemsTransportistas
            {
                public decimal idTransportista { get; set; }
                public string nombreTransportista { get; set; }
                public string codTransportista { get; set; }

            }
        }

        public bool ObtenerTransportistas()
        {
            string json = "";
            bool resultado = false;

            EBS12_TRANSPORTISTAS tarifa = new EBS12_TRANSPORTISTAS();

            try
            {
                
                ClaseHttpCliente cliente = new ClaseHttpCliente();

                var response = ClaseHttpCliente.cliente.GetAsync("/tarifasViajes/transportistas").GetAwaiter().GetResult();

                if (response.IsSuccessStatusCode)
                {
                    json = response.Content.ReadAsStringAsync().Result;
                    tarifa = Newtonsoft.Json.JsonConvert.DeserializeObject<EBS12_TRANSPORTISTAS>(json);
                    Transportistas = tarifa.Transportistas;


                    if (Transportistas.resultado == "Si")
                        resultado = true;
                }
                else
                    throw new Exception(response.ReasonPhrase);
                
                return resultado;

            }
            catch (Exception exception)
            {
                throw exception;
            }
          
        }

        public bool ObtenerTransportistaId(Int64 idTransportista)
        {
            string json = "";
            bool resultado = false;

            EBS12_TRANSPORTISTAS tarifa = new EBS12_TRANSPORTISTAS();

            try
            {

                ClaseHttpCliente cliente = new ClaseHttpCliente();

                var response = ClaseHttpCliente.cliente.GetAsync("/tarifasViajes/transportistas/" + idTransportista.ToString()).GetAwaiter().GetResult();

                if (response.IsSuccessStatusCode)
                {
                    json = response.Content.ReadAsStringAsync().Result;
                    tarifa = Newtonsoft.Json.JsonConvert.DeserializeObject<EBS12_TRANSPORTISTAS>(json);
                    Transportistas = tarifa.Transportistas;


                    if (Transportistas.resultado == "Si")
                        resultado = true;
                }
                else
                    throw new Exception(response.ReasonPhrase);

                return resultado;

            }
            catch (Exception exception)
            {
                throw exception;
            }

        } 


    }
}