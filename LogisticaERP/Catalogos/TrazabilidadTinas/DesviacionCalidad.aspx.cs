using LogisticaERP.Clases.TrazabilidadTinas;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web;
using System.Web.Services;

namespace LogisticaERP.Catalogos.TrazabilidadTinas
{
    public partial class DesviacionCalidad : System.Web.UI.Page
    {
        private static HttpContext _contextoSesion;
        private static HttpClient _httpClient = new HttpClient();

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        [WebMethod]
        public static ListaProblema ObtenerDesviacionCalidad()
        {
            _contextoSesion = HttpContext.Current;

            string listaDesviacionCalidad = "";
            ListaProblema listaProblema = new ListaProblema();

            if (_contextoSesion.User.Identity.IsAuthenticated)
            {
                try
                {
                    _httpClient.DefaultRequestHeaders.Add("Accept", "application/json");

                    listaDesviacionCalidad = _httpClient.GetStringAsync(Constantes.BUS_SERVICES_PATH_TT + "catalogos/problemas").Result;

                    listaProblema = JsonConvert.DeserializeObject<ListaProblema>(listaDesviacionCalidad);

                    for (int i = 0; i < listaProblema.problemas.Count; i++)
                    {
                        var listaAlertasParse = Convert.ToString(listaProblema.problemas[i].idProblema);
                        listaProblema.problemas[i].idProblemaString = listaAlertasParse;
                    }

                }
                catch (Exception ex)
                {
                    listaProblema = null;

                    ManejadorLogsErrores.GuardarLog(ex);
                }
            }

             return listaProblema;
        }

        [WebMethod]
        public static HttpStatusCode CrearDesviacionCalidad(Problema problema)
        {
            _contextoSesion = HttpContext.Current;

            var respuestaHttpBus = new HttpStatusCode();

            if (_contextoSesion.User.Identity.IsAuthenticated)
            {
                if (problema != null)
                {
                    problema.usuario = _contextoSesion.User.Identity.Name;

                    string jsonString = JsonConvert.SerializeObject(problema);

                    try
                    {
                        var httpContent = new StringContent(jsonString, Encoding.UTF8, "application/json");

                        var response = _httpClient.PostAsync(Constantes.BUS_SERVICES_PATH_TT + "catalogos/problemas", httpContent).Result;

                        if (response != null)
                        {
                            if (response.IsSuccessStatusCode)
                            {
                                respuestaHttpBus = HttpStatusCode.OK;
                            }
                            else
                            {
                                var contenidoRespuestaBus = response.Content.ReadAsStringAsync();
                                var mensajeRespuestaBus = contenidoRespuestaBus.Result;
                                var c = mensajeRespuestaBus.Split(',')[0];
                                var codigoError = c.Split(':')[1];

                                if (codigoError == "409")
                                {
                                    respuestaHttpBus = HttpStatusCode.Conflict;
                                }
                                else
                                {
                                    respuestaHttpBus = HttpStatusCode.BadRequest;
                                }

                                ManejadorLogsErrores.GuardarLog(Constantes.ELEMENTO_BUS, mensajeRespuestaBus);
                            }
                        }
                        else
                        {
                            ManejadorLogsErrores.GuardarLog(Constantes.ELEMENTO_KENDO_GRID, "Error al obtener los datos de la Alerta del nodo de integración");
                        }
                    }
                    catch (Exception ex)
                    {
                        respuestaHttpBus = HttpStatusCode.BadRequest;

                        ManejadorLogsErrores.GuardarLog(ex);
                    }
                }
                else
                {
                    respuestaHttpBus = HttpStatusCode.NoContent;

                    ManejadorLogsErrores.GuardarLog(Constantes.ELEMENTO_BUS, "Error al obtener los datos de la Alerta del grid");
                }
            }
            else
            {
                respuestaHttpBus = HttpStatusCode.MethodNotAllowed;
            }

            return respuestaHttpBus;
        }

        [WebMethod]
        public static HttpStatusCode EditarDesviacionCalidad(Problema problema)
        {
            _contextoSesion = HttpContext.Current;

            var respuestaHttpBus = new HttpStatusCode();

            if (_contextoSesion.User.Identity.IsAuthenticated)
            {
                if (problema != null)
                {
                    problema.usuario = _contextoSesion.User.Identity.Name;

                    string jsonString = JsonConvert.SerializeObject(problema);

                    try
                    {
                        var httpContent = new StringContent(jsonString, Encoding.UTF8, "application/json");

                        var response = _httpClient.PutAsync(Constantes.BUS_SERVICES_PATH_TT + "catalogos/problemas", httpContent).Result;

                        if (response != null)
                        {
                            if (response.IsSuccessStatusCode)
                            {
                                respuestaHttpBus = HttpStatusCode.OK;
                            }
                            else
                            {
                                var contenidoRespuestaBus = response.Content.ReadAsStringAsync();
                                var mensajeRespuestaBus = contenidoRespuestaBus.Result;
                                var c = mensajeRespuestaBus.Split(',')[0];
                                var codigoError = c.Split(':')[1];

                                respuestaHttpBus = HttpStatusCode.BadRequest;

                                ManejadorLogsErrores.GuardarLog(Constantes.ELEMENTO_BUS, mensajeRespuestaBus);
                            }
                        }
                        else
                        {
                            ManejadorLogsErrores.GuardarLog(Constantes.ELEMENTO_KENDO_GRID, "Error al obtener los datos");
                        }
                    }
                    catch (Exception ex)
                    {
                        respuestaHttpBus = HttpStatusCode.BadRequest;

                        ManejadorLogsErrores.GuardarLog(ex);
                    }
                }
                else
                {
                    respuestaHttpBus = HttpStatusCode.NoContent;

                    ManejadorLogsErrores.GuardarLog(Constantes.ELEMENTO_BUS, "Error al obtener los datos");
                }
            }
            else
            {
                respuestaHttpBus = HttpStatusCode.MethodNotAllowed;
            }

            return respuestaHttpBus;
        }
    }
}