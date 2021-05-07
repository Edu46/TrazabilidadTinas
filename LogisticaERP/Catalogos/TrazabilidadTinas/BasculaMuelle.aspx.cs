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
    public partial class BasculaMuelle : System.Web.UI.Page
    {
        private static HttpContext _contextoSesion;
        private static HttpClient _httpClient = new HttpClient();

        public class Respuesta
        {
            public string mensaje { get; set; }
            public string resultado { get; set; }
        };

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        [WebMethod]
        public static ListaBasculaMuelle ObtenerBasculaMuelle()
        {
            _contextoSesion = HttpContext.Current;
            string listaCatalogosBasculaMuelleBus = "";
            ListaBasculaMuelle listaBasculasMuelles = new ListaBasculaMuelle();

            if (_contextoSesion.User.Identity.IsAuthenticated)
            {
                try
                {
                    _httpClient.DefaultRequestHeaders.Add("Accept", "application/json");

                    listaCatalogosBasculaMuelleBus = _httpClient.GetStringAsync(Constantes.BUS_SERVICES_PATH_TT + "relaciones/basculas-muelles").Result;

                    listaBasculasMuelles = JsonConvert.DeserializeObject<ListaBasculaMuelle>(listaCatalogosBasculaMuelleBus);

                }
                catch (Exception ex)
                {
                    listaBasculasMuelles = null;

                    ManejadorLogsErrores.GuardarLog(ex);
                }
            }

            return listaBasculasMuelles;
        }

        [WebMethod]
        public static ListaBascula ObtenerBasculas()
        {
            _contextoSesion = HttpContext.Current;
            string listaCatalogosBasculaBus = "";
            ListaBascula listaBasculas = new ListaBascula();

            if (_contextoSesion.User.Identity.IsAuthenticated)
            {
                try
                {
                    _httpClient.DefaultRequestHeaders.Add("Accept", "application/json");

                    listaCatalogosBasculaBus = _httpClient.GetStringAsync(Constantes.BUS_SERVICES_PATH_TT + "catalogos/basculas?activo=true").Result;

                    listaBasculas = JsonConvert.DeserializeObject<ListaBascula>(listaCatalogosBasculaBus);

                }
                catch (Exception ex)
                {
                    listaBasculas = null;

                    ManejadorLogsErrores.GuardarLog(ex);
                }
            }

            return listaBasculas;
        }

        [WebMethod]
        public static ListaMuelle ObtenerMuelle()
        {
            _contextoSesion = HttpContext.Current;
            string listaMuellesBus = "";
            ListaMuelle listaMuelle = new ListaMuelle();

            if (_contextoSesion.User.Identity.IsAuthenticated)
            {
                try
                {
                    _httpClient.DefaultRequestHeaders.Add("Accept", "application/json");

                    listaMuellesBus = _httpClient.GetStringAsync(Constantes.BUS_SERVICES_PATH_TT + "catalogos/muelles").Result;

                    listaMuelle = JsonConvert.DeserializeObject<ListaMuelle>(listaMuellesBus);

                }
                catch (Exception ex)
                {
                    listaMuelle = null;

                    ManejadorLogsErrores.GuardarLog(ex);
                }
            }

            return listaMuelle;
        }

        [WebMethod]
        public static HttpStatusCode CrearBasculaMuelle(Clases.TrazabilidadTinas.BasculaMuelle catalogoBasculaMuelle)
        {
            _contextoSesion = HttpContext.Current;

            var respuestaHttpBus = new HttpStatusCode();

            if (_contextoSesion.User.Identity.IsAuthenticated)
            {
                if (catalogoBasculaMuelle != null)
                {
                    catalogoBasculaMuelle.usuario = _contextoSesion.User.Identity.Name;

                    string jsonString = JsonConvert.SerializeObject(catalogoBasculaMuelle);

                    try
                    {
                        var httpContent = new StringContent(jsonString, Encoding.UTF8, "application/json");

                        var response = _httpClient.PostAsync(Constantes.BUS_SERVICES_PATH_TT + "relaciones/basculas-muelles", httpContent).Result;

                        var resultContent = response.Content.ReadAsStringAsync().Result;

                        var laRespuesta = JsonConvert.DeserializeObject<Respuesta>(resultContent);

                        if (response != null)
                        {
                            if (response.IsSuccessStatusCode)
                            {
                                if (laRespuesta.resultado == "NO")
                                {
                                    respuestaHttpBus = HttpStatusCode.NotModified;
                                }
                                else
                                {
                                    respuestaHttpBus = HttpStatusCode.OK;
                                }
                            }
                            else
                            {
                                var contenidoRespuestaBus = response.Content.ReadAsStringAsync();
                                var mensajeRespuestaBus = contenidoRespuestaBus.Result;
                                var c = mensajeRespuestaBus.Split(',')[0];
                                var codigoError = c.Split(':')[1];

                                if (codigoError == "201")
                                {
                                    respuestaHttpBus = HttpStatusCode.Created;
                                }
                                else if (codigoError == "204")
                                {
                                    respuestaHttpBus = HttpStatusCode.NoContent;
                                }


                                ManejadorLogsErrores.GuardarLog(Constantes.ELEMENTO_BUS, mensajeRespuestaBus);
                            }
                        }
                        else
                        {
                            ManejadorLogsErrores.GuardarLog(Constantes.ELEMENTO_KENDO_GRID, "Error al obtener los datos del trazabilidadContenedores del nodo de integración");
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

                    ManejadorLogsErrores.GuardarLog(Constantes.ELEMENTO_BUS, "Error al obtener los datos del trazabilidadContenedores del grid");
                }
            }
            else
            {
                respuestaHttpBus = HttpStatusCode.MethodNotAllowed;
            }

            return respuestaHttpBus;
        }

        [WebMethod]
        public static HttpStatusCode EditarBasculaMuelle(Clases.TrazabilidadTinas.BasculaMuelle catalogoBasculaMuelle)
        {
            _contextoSesion = HttpContext.Current;

            var respuestaHttpBus = new HttpStatusCode();

            if (_contextoSesion.User.Identity.IsAuthenticated)
            {
                if (catalogoBasculaMuelle != null)
                {
                    catalogoBasculaMuelle.usuario = _contextoSesion.User.Identity.Name;

                    string jsonString = JsonConvert.SerializeObject(catalogoBasculaMuelle);

                    try
                    {
                        var httpContent = new StringContent(jsonString, Encoding.UTF8, "application/json");

                        var response = _httpClient.PutAsync(Constantes.BUS_SERVICES_PATH_TT + "relaciones/basculas-muelles", httpContent).Result;

                        var resultContent = response.Content.ReadAsStringAsync().Result;

                        var laRespuesta = JsonConvert.DeserializeObject<Respuesta>(resultContent);

                        if (response != null)
                        {
                            if (response.IsSuccessStatusCode)
                            {
                                if (laRespuesta.resultado == "NO")
                                {
                                    respuestaHttpBus = HttpStatusCode.NotModified;
                                }
                                else
                                {
                                    respuestaHttpBus = HttpStatusCode.OK;
                                }
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