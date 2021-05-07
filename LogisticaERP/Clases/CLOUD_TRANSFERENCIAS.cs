using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LogisticaERP.Clases
{
    public class CLOUD_TRANSFERENCIAS 
    {
        public Tranferencia Transferencias { get; set; }

        public class Tranferencia
        {
            
            public decimal numTransferencia { get; set; }
            public string almacenOrigen { get; set; }
            public string codAlmacenOrigen { get; set; }
            public string transportista { get; set; }
            public string cliente { get; set; }
            public decimal noCajas { get; set; }
            public string descripcion { get; set; }
            public string viaje { get; set; }
            public decimal ordenTranferencia { get; set; }
            public string almacenDestino { get; set; }
            public string codAlmacenDestino { get; set; }
            public string resultado { get; set; }
            public string mensaje { get; set; }

        }

        public bool ObtenerTransferencia(string NoViaje)
        {
            string json = "";
            bool resultado = false;

            CLOUD_TRANSFERENCIAS TransferenciasCloud = new CLOUD_TRANSFERENCIAS();

            try
            {

                ClaseHttpCliente cliente = new ClaseHttpCliente();
                var response = ClaseHttpCliente.cliente.GetAsync("/tarifasViajes/transferencias/" + NoViaje.ToString()).GetAwaiter().GetResult();

                if (response.IsSuccessStatusCode)
                {
                    json = response.Content.ReadAsStringAsync().Result;
                    TransferenciasCloud = Newtonsoft.Json.JsonConvert.DeserializeObject<CLOUD_TRANSFERENCIAS>(json);
                    Transferencias = TransferenciasCloud.Transferencias;

                    if (Transferencias.resultado == "Si")
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