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
    public class EBS12_TARIFAS_REPRESENTANTES
    {
        public TarifaRepre TarifaRepresentante { get; set; }

        public EBS12_TARIFAS_REPRESENTANTES()
        {
            TarifaRepresentante = new TarifaRepre();
        }

        public class TarifaRepre
        {

            public List<ItemsTarifaRepre> items { get; set; }

            [Serializable()]
            public class ItemsTarifaRepre
            {
                public decimal idTarifaRepresentante { get; set; }
                public string origen { get; set; }
                public string codOrganizacion { get; set; }
                public string destEstado { get; set; }
                public string destCiudad { get; set; }
                public string zona { get; set; }
                public decimal cajas { get; set; }
                public decimal kilometros { get; set; }
                public decimal kilometrosMin { get; set; }
                public decimal costoCaja { get; set; }
                public Int32 activo { get; set; }

            }

            public string resultado { get; set; }
            public string mensaje { get; set; }

        }


        public EBS12_TARIFAS_REPRESENTANTES GrabarTarifaRepresentante(string jsonRepresentante)
        {
            EBS12_TARIFAS_REPRESENTANTES tarifa = null;
            string json = "";

            try
            {

                HttpContent inputContent = new StringContent(jsonRepresentante, Encoding.UTF8, "application/json");
                HttpResponseMessage response = ClaseHttpCliente.cliente.PostAsync("/tarifasViajes/tarifaRepresentante", inputContent).GetAwaiter().GetResult();

                json = response.Content.ReadAsStringAsync().Result;
                var jsonObj = JsonConvert.DeserializeObject<JObject>(json).First.First;
                tarifa = Newtonsoft.Json.JsonConvert.DeserializeObject<EBS12_TARIFAS_REPRESENTANTES>(json);

                if (tarifa.TarifaRepresentante.resultado == null)
                    throw new Exception(tarifa.TarifaRepresentante.mensaje);

            }
            catch (Exception ex)
            {
                throw ex;
            }

            return tarifa;
        }
    }
}