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
    public partial class AntecamaraBasculaFrigorifico : System.Web.UI.Page
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
        public static ListaAntecamaraBascula ObtenerAntecamaraBascula()
        {
            _contextoSesion = HttpContext.Current;

            string listaAntecamaraBasculaBus = "";
            ListaAntecamaraBascula listaAntecamaraBascula = new ListaAntecamaraBascula();

            if (_contextoSesion.User.Identity.IsAuthenticated)
            {
                try
                {
                    _httpClient.DefaultRequestHeaders.Add("Accept", "application/json");

                    listaAntecamaraBasculaBus = _httpClient.GetStringAsync(Constantes.BUS_SERVICES_PATH_TT + "relaciones/antecamaras-basculas").Result;

                    listaAntecamaraBascula = JsonConvert.DeserializeObject<ListaAntecamaraBascula>(listaAntecamaraBasculaBus);
                }
                catch (Exception ex)
                {
                    listaAntecamaraBascula = null;

                    ManejadorLogsErrores.GuardarLog(ex);
                }
            }
            return listaAntecamaraBascula;
        }

        [WebMethod]
        public static ListaAntecamaraFrigorifico ObtenerAntecamaraFrigorifico()
        {
            _contextoSesion = HttpContext.Current;

            string listaAntecamaraFrigorificoBus = "";
            ListaAntecamaraFrigorifico listaAntecamaraFrigorifico = new ListaAntecamaraFrigorifico();

            if (_contextoSesion.User.Identity.IsAuthenticated)
            {
                try
                {
                    _httpClient.DefaultRequestHeaders.Add("Accept", "application/json");

                    listaAntecamaraFrigorificoBus = _httpClient.GetStringAsync(Constantes.BUS_SERVICES_PATH_TT + "relaciones/antecamaras-frigorificos").Result;

                    listaAntecamaraFrigorifico = JsonConvert.DeserializeObject<ListaAntecamaraFrigorifico>(listaAntecamaraFrigorificoBus);

                    for (int i = 0; i < listaAntecamaraFrigorifico.antecamarasFrigorificos.Count; i++)
                    {
                        var stridFrigorifico = Convert.ToString(listaAntecamaraFrigorifico.antecamarasFrigorificos[i].idFrigorifico);
                        listaAntecamaraFrigorifico.antecamarasFrigorificos[i].StridFrigorifico = stridFrigorifico;

                    }
                }
                catch (Exception ex)
                {
                    listaAntecamaraFrigorifico = null;

                    ManejadorLogsErrores.GuardarLog(ex);
                }
            }
            return listaAntecamaraFrigorifico;
        }

        [WebMethod]
        public static ListaBascula ObtenerBasculasActivas()
        {
            _contextoSesion = HttpContext.Current;
            string listaBasculasBus = "";
            ListaBascula listaBasculas = new ListaBascula();

            if (_contextoSesion.User.Identity.IsAuthenticated)
            {
                try
                {
                    _httpClient.DefaultRequestHeaders.Add("Accept", "application/json");

                    listaBasculasBus = _httpClient.GetStringAsync(Constantes.BUS_SERVICES_PATH_TT + "catalogos/basculas?activo=true").Result;

                    listaBasculas = JsonConvert.DeserializeObject<ListaBascula>(listaBasculasBus);

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
        public static ListaAntecamara ObtenerAntecamara()
        {
            _contextoSesion = HttpContext.Current;
            string listaCatalogosAlertasBus = "";
            ListaAntecamara listaAntecamara = new ListaAntecamara();

            if (_contextoSesion.User.Identity.IsAuthenticated)
            {
                try
                {
                    _httpClient.DefaultRequestHeaders.Add("Accept", "application/json");

                    listaCatalogosAlertasBus = _httpClient.GetStringAsync(Constantes.BUS_SERVICES_PATH_TT + "catalogos/antecamaras").Result;

                    listaAntecamara = JsonConvert.DeserializeObject<ListaAntecamara>(listaCatalogosAlertasBus);

                }
                catch (Exception ex)
                {
                    listaAntecamara = null;

                    ManejadorLogsErrores.GuardarLog(ex);
                }
            }

            return listaAntecamara;
        }

        [WebMethod]
        public static ListaFrigorificoAntecamara ObtenerFrigorifico(bool activo)
        {
            _contextoSesion = HttpContext.Current;
            string listaFrigorificoBus = "";
            ListaFrigorificoAntecamara listaFrigorifico = new ListaFrigorificoAntecamara();

            if (_contextoSesion.User.Identity.IsAuthenticated)
            {
                try
                {
                    _httpClient.DefaultRequestHeaders.Add("Accept", "application/json");

                    listaFrigorificoBus = _httpClient.GetStringAsync(Constantes.BUS_SERVICES_PATH_TT + "catalogos/frigorificos?activo=" + activo).Result;

                    listaFrigorifico = JsonConvert.DeserializeObject<ListaFrigorificoAntecamara>(listaFrigorificoBus);

                }
                catch (Exception ex)
                {
                    listaFrigorifico = null;

                    ManejadorLogsErrores.GuardarLog(ex);
                }
            }

            return listaFrigorifico;
        }

        //Antecamara Bascula

        [WebMethod]
        public static HttpStatusCode CrearAntecamaraBascula(AntecamaraBascula antecamarasBascula)
        {
            _contextoSesion = HttpContext.Current;

            var respuestaHttpBus = new HttpStatusCode();

            if (_contextoSesion.User.Identity.IsAuthenticated)
            {
                if (antecamarasBascula != null)
                {
                    antecamarasBascula.usuario = _contextoSesion.User.Identity.Name;

                    string jsonString = JsonConvert.SerializeObject(antecamarasBascula);

                    try
                    {
                        var httpContent = new StringContent(jsonString, Encoding.UTF8, "application/json");

                        var response = _httpClient.PostAsync(Constantes.BUS_SERVICES_PATH_TT + "relaciones/antecamaras-basculas", httpContent).Result;

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
        public static HttpStatusCode EditarAntecamaraBascula(AntecamaraBascula antecamarasBascula)
        {
            _contextoSesion = HttpContext.Current;

            var respuestaHttpBus = new HttpStatusCode();

            if (_contextoSesion.User.Identity.IsAuthenticated)
            {
                if (antecamarasBascula != null)
                {
                    antecamarasBascula.usuario = _contextoSesion.User.Identity.Name;

                    string jsonString = JsonConvert.SerializeObject(antecamarasBascula);

                    try
                    {
                        var httpContent = new StringContent(jsonString, Encoding.UTF8, "application/json");

                        var response = _httpClient.PutAsync(Constantes.BUS_SERVICES_PATH_TT + "relaciones/antecamaras-basculas", httpContent).Result;

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

        //Antecamara Frigorifico

        [WebMethod]
        public static HttpStatusCode CrearAntecamarFrigorifico(AntecamaraFrigorifico antecamaraFrigorifico)
        {
            _contextoSesion = HttpContext.Current;

            var respuestaHttpBus = new HttpStatusCode();

            if (_contextoSesion.User.Identity.IsAuthenticated)
            {
                if (antecamaraFrigorifico != null)
                {
                    antecamaraFrigorifico.usuario = _contextoSesion.User.Identity.Name;

                    string jsonString = JsonConvert.SerializeObject(antecamaraFrigorifico);

                    try
                    {
                        var httpContent = new StringContent(jsonString, Encoding.UTF8, "application/json");

                        var response = _httpClient.PostAsync(Constantes.BUS_SERVICES_PATH_TT + "relaciones/antecamaras-frigorificos", httpContent).Result;

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
        public static HttpStatusCode EditarAntecamaraFrigorifico(AntecamaraFrigorifico antecamaraFrigorifico)
        {
            _contextoSesion = HttpContext.Current;

            var respuestaHttpBus = new HttpStatusCode();

            if (_contextoSesion.User.Identity.IsAuthenticated)
            {
                if (antecamaraFrigorifico != null)
                {
                    antecamaraFrigorifico.usuario = _contextoSesion.User.Identity.Name;

                    string jsonString = JsonConvert.SerializeObject(antecamaraFrigorifico);

                    try
                    {
                        var httpContent = new StringContent(jsonString, Encoding.UTF8, "application/json");

                        var response = _httpClient.PutAsync(Constantes.BUS_SERVICES_PATH_TT + "relaciones/antecamaras-frigorificos", httpContent).Result;

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