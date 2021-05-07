using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LogisticaERP.Clases
{
    public class EBS12_CODIGOS_MOTIVOS
    {
        public List<CodigoMotivo> codigos_motivos { get; set; }
        public string resultado { get; set; }
        public string mensaje { get; set; }
        public string error { get; set; }


        [Serializable]
        public class CodigoMotivo
        {
            public string codigo { get; set; }
            public string descripcion { get; set; }

            public CodigoMotivo()
            {
                codigo = string.Empty;
                descripcion = string.Empty;
            }

        }

        [Serializable]
        public class CodigoMotivoGrid
        {
            public decimal IdIndex { get; set; }
            public string Codigo { get; set; }
            public string Descripcion { get; set; }

            public CodigoMotivoGrid()
            {
                IdIndex = 0;
                Codigo = string.Empty;
                Descripcion = string.Empty;
            }
        }


        public List<CodigoMotivo> ObtenerCodigosMotivos()
        {
            EBS12_CODIGOS_MOTIVOS codigoMotivo = new EBS12_CODIGOS_MOTIVOS();
            string json;

            try
            {
                ClaseHttpCliente cliente = new ClaseHttpCliente();
                var response = ClaseHttpCliente.cliente.GetAsync("/cancelacion/pv/ebs12/1.0/codigos-motivos").GetAwaiter().GetResult();

                if (response.IsSuccessStatusCode)
                {
                    json = response.Content.ReadAsStringAsync().Result;
                    codigoMotivo = Newtonsoft.Json.JsonConvert.DeserializeObject<EBS12_CODIGOS_MOTIVOS>(json);
                }

                if (codigoMotivo.resultado == null || codigoMotivo.resultado == "NO")
                {
                    if (codigoMotivo.error == "ERROR_VALIDACION")
                        throw new JsonException(codigoMotivo.mensaje);
                    else
                        throw new Exception(codigoMotivo.mensaje);
                }
                return codigoMotivo.codigos_motivos;

            }
            catch (JsonException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
