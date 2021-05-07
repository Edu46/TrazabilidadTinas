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
    public class CLOUD_LISTA_VALORES
    {
        public Lookups ListaValores { get; set; }

        public CLOUD_LISTA_VALORES()
        {
            ListaValores = new Lookups();
        }

        public class Lookups
        {

            public string tipoLista { get; set; }
            public string nombreArchivo { get; set; }
            public List<ItemsLookups> items { get; set; }

            [Serializable()]
            public class ItemsLookups
            {
                public string codigo { get; set; }
                public string descripcion { get; set; }
                public string significado { get; set; }
                public string etiqueta { get; set; }
                public string activo { get; set; }
            }

            public Lookups()
            {
                items = new List<ItemsLookups>();
            }

        }

        public class JsonClassResponse
        {
            public string mensaje { get; set; }
            public bool resultado { get; set; }

            public JsonClassResponse()
            {
                mensaje = "";
                resultado = false;
            }

        }



        public bool GenerarArchivotxt(string jsonListaValores)
        {            
            string json = "";
            JsonClassResponse st_Respuesta = new JsonClassResponse();
            bool resultado = false;

            try
            {

                HttpContent inputContent = new StringContent(jsonListaValores, Encoding.UTF8, "application/json");
                HttpResponseMessage response = ClaseHttpCliente.cliente.PostAsync("/listasValores/importarArchivo", inputContent).GetAwaiter().GetResult();

                if (response.IsSuccessStatusCode)
                {
                    json = response.Content.ReadAsStringAsync().Result;

                    var jsonObj = JsonConvert.DeserializeObject<JObject>(json).First.First;
                    st_Respuesta.mensaje = jsonObj["mensaje"].ToString();
                    st_Respuesta.resultado = Convert.ToBoolean(jsonObj["resultado"]);

                    if (st_Respuesta.resultado != true)
                    {
                        resultado = false;
                        throw new Exception(st_Respuesta.mensaje);
                    }
                    else
                        resultado = true;
                }
                else
                {
                    resultado = false;
                    var x = response.RequestMessage;
                    throw new Exception("Se ha producido un error en la llamada del servicio de Generar Archivo txt.");
                }

                /* json = response.Content.ReadAsStringAsync().Result;
                 var jsonObj = JsonConvert.DeserializeObject<JObject>(json).First.First;
                 tarifa = Newtonsoft.Json.JsonConvert.DeserializeObject<EBS12_TARIFAS_REPRESENTANTES>(json);

                 if (tarifa.TarifaRepresentante.resultado == null)
                     throw new Exception(tarifa.TarifaRepresentante.mensaje);*/

            }
            catch (Exception ex)
            {
                throw ex;
            }

            return resultado;
        }
    }
}