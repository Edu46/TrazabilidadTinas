using LogisticaERP.Clases.TrazabilidadTinas;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace LogisticaERP.Catalogos.TrazabilidadTinas
{
    public partial class ActualizarTaraMontacarga : System.Web.UI.Page
    {
        private static HttpContext _contextoSesion;
        private static HttpClient _httpClient = new HttpClient();

        public class Respuesta
        {
            public string idViaje { get; set; }
            public string mensaje { get; set; }
            public string resultado { get; set; }
            public string codigo { get; set; }
        };

        protected void Page_Load(object sender, EventArgs e)
        {
            /*Se añade esta linea de codigo para evitar que se ingrese desde el URL a la pantalla*/
            /*Se mantiene comentado hasta obtener luz verde de implementacion
            Response.Redirect("~/Inicio.aspx");*/
        }

        [WebMethod]
        public static ListaTaraMontacarga ObtenerTaraMontacarga(int idTipoMontacargas,  int idBascula)
        {
            _contextoSesion = HttpContext.Current;

            string listaTaraMontacargaBus = "";
            ListaTaraMontacarga taraMontacarga = new ListaTaraMontacarga();

            if (_contextoSesion.User.Identity.IsAuthenticated)
            {
                try
                {
                    _httpClient.DefaultRequestHeaders.Add("Accept", "application/json");

                    listaTaraMontacargaBus = _httpClient.GetStringAsync(Constantes.BUS_SERVICES_PATH_TT + "montacargas/taras/alertas?idBascula=" + idBascula + "&idTipoMontacargas=" + idTipoMontacargas).Result;

                    taraMontacarga = JsonConvert.DeserializeObject<ListaTaraMontacarga>(listaTaraMontacargaBus);

                }
                catch (Exception ex)
                {
                    taraMontacarga = null;

                    ManejadorLogsErrores.GuardarLog(ex);
                }
            }

            return taraMontacarga;
        }

        [WebMethod]
        public static ListaTipoMontacarga CargarDDLTipoMontacarga()
        {
            _contextoSesion = HttpContext.Current;

            string listaTipoMontacargaBus = "";
            ListaTipoMontacarga listaTipoMontacargas = new ListaTipoMontacarga();

            if (_contextoSesion.User.Identity.IsAuthenticated)
            {
                try
                {
                    _httpClient.DefaultRequestHeaders.Add("Accept", "application/json");

                    listaTipoMontacargaBus = _httpClient.GetStringAsync(Constantes.BUS_SERVICES_PATH_TT + "montacargas/tipo").Result;

                    listaTipoMontacargas = JsonConvert.DeserializeObject<ListaTipoMontacarga>(listaTipoMontacargaBus);
                }
                catch (Exception ex)
                {
                    listaTipoMontacargas = null;

                    ManejadorLogsErrores.GuardarLog(ex);
                }
            }
            return listaTipoMontacargas;
        }

        [WebMethod]
        public static HttpStatusCode EnviarAlertaMontacarga(TaraMontacargaDetalle taraMontacargaDetalle)
        {
            _contextoSesion = HttpContext.Current;

            var respuestaHttpBus = new HttpStatusCode();

            if (_contextoSesion.User.Identity.IsAuthenticated)
            {
                if (taraMontacargaDetalle != null)
                {
                    taraMontacargaDetalle.usuario = _contextoSesion.User.Identity.Name;

                    string jsonString = JsonConvert.SerializeObject(taraMontacargaDetalle);

                    try
                    {
                        var httpContent = new StringContent(jsonString, Encoding.UTF8, "application/json");

                        var response = _httpClient.PutAsync(Constantes.BUS_SERVICES_PATH_TT + "montacargas/taras/alertas", httpContent).Result;

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
                            ManejadorLogsErrores.GuardarLog(Constantes.ELEMENTO_KENDO_GRID, "Error al obtener los datos del nodo de integración");
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

                    ManejadorLogsErrores.GuardarLog(Constantes.ELEMENTO_BUS, "Error al obtener los datos del grid");
                }
            }
            else
            {
                respuestaHttpBus = HttpStatusCode.MethodNotAllowed;
            }

            return respuestaHttpBus;
        }

        [WebMethod]
        public static HttpStatusCode ActualizarTiempo(ActualizarAlerta taraMontacargaDetalle)
        {
            _contextoSesion = HttpContext.Current;

            var respuestaHttpBus = new HttpStatusCode();

            if (_contextoSesion.User.Identity.IsAuthenticated)
            {
                if (taraMontacargaDetalle != null)
                {
                    taraMontacargaDetalle.usuario = _contextoSesion.User.Identity.Name;

                    string jsonString = JsonConvert.SerializeObject(taraMontacargaDetalle);

                    try
                    {
                        var httpContent = new StringContent(jsonString, Encoding.UTF8, "application/json");

                        var response = _httpClient.PostAsync(Constantes.BUS_SERVICES_PATH_TT + "montacargas/frecuencia/alerta", httpContent).Result;

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
                            ManejadorLogsErrores.GuardarLog(Constantes.ELEMENTO_KENDO_GRID, "Error al obtener los datos del nodo de integración");
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

                    ManejadorLogsErrores.GuardarLog(Constantes.ELEMENTO_BUS, "Error al obtener los datos del grid");
                }
            }
            else
            {
                respuestaHttpBus = HttpStatusCode.MethodNotAllowed;
            }

            return respuestaHttpBus;
        }

        [WebMethod]
        public static HttpStatusCode ActualizarTara(ActualizarTara actualizarTara)
        {
            _contextoSesion = HttpContext.Current;

            var respuestaHttpBus = new HttpStatusCode();

            if (_contextoSesion.User.Identity.IsAuthenticated)
            {
                if (actualizarTara != null)
                {
                    actualizarTara.usuario = _contextoSesion.User.Identity.Name;

                    string jsonString = JsonConvert.SerializeObject(actualizarTara);

                    try
                    {
                        var httpContent = new StringContent(jsonString, Encoding.UTF8, "application/json");

                        var response = _httpClient.PostAsync(Constantes.BUS_SERVICES_PATH_TT + "montacargas/taras", httpContent).Result;

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
                            ManejadorLogsErrores.GuardarLog(Constantes.ELEMENTO_KENDO_GRID, "Error al obtener los datos del nodo de integración");
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

                    ManejadorLogsErrores.GuardarLog(Constantes.ELEMENTO_BUS, "Error al obtener los datos del grid");
                }
            }
            else
            {
                respuestaHttpBus = HttpStatusCode.MethodNotAllowed;
            }

            return respuestaHttpBus;
        }


        [WebMethod]
        public static TransaccionPeso ConsultarTransaccionProveedor(int idBascula)
        {
            _contextoSesion = HttpContext.Current;

            string transaccionPesoProveedorBus = "";
            TransaccionPeso transaccionPesoProveedor = new TransaccionPeso();

            if (_contextoSesion.User.Identity.IsAuthenticated)
            {
                try
                {
                    _httpClient.DefaultRequestHeaders.Add("Accept", "application/json");

                    transaccionPesoProveedorBus = _httpClient.GetStringAsync(Constantes.BUS_SERVICES_PATH_TT + "basculas/transaccion-peso?idBascula=" + idBascula).Result;

                    transaccionPesoProveedor = JsonConvert.DeserializeObject<TransaccionPeso>(transaccionPesoProveedorBus);

                }
                catch (Exception ex)
                {
                    transaccionPesoProveedor = null;

                    ManejadorLogsErrores.GuardarLog(ex);
                }
            }

            return transaccionPesoProveedor;
        }

        [WebMethod]
        public static ListaBascula ObtenerBasculas(bool activo)
        {
            _contextoSesion = HttpContext.Current;
            string listaCatalogosAlertasBus = "";
            ListaBascula listaBasculas = new ListaBascula();

            if (_contextoSesion.User.Identity.IsAuthenticated)
            {
                try
                {
                    _httpClient.DefaultRequestHeaders.Add("Accept", "application/json");

                    listaCatalogosAlertasBus = _httpClient.GetStringAsync(Constantes.BUS_SERVICES_PATH_TT + "catalogos/basculas?activo=" + activo).Result;

                    listaBasculas = JsonConvert.DeserializeObject<ListaBascula>(listaCatalogosAlertasBus);

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
        public static TarajeMontacargas ConsultarTarajeMontacargas(int idBascula)
        {
            _contextoSesion = HttpContext.Current;

            string tarajeMontacargasBus = "";
            TarajeMontacargas tarajeMontacargas = new TarajeMontacargas();

            if (_contextoSesion.User.Identity.IsAuthenticated)
            {
                try
                {
                    _httpClient.DefaultRequestHeaders.Add("Accept", "application/json");

                    tarajeMontacargasBus = _httpClient.GetStringAsync(Constantes.BUS_SERVICES_PATH_TT + "montacargas/taraje?idBascula=" + idBascula).Result;

                    tarajeMontacargas = JsonConvert.DeserializeObject<TarajeMontacargas>(tarajeMontacargasBus);

                }
                catch (Exception ex)
                {
                    tarajeMontacargas = null;

                    ManejadorLogsErrores.GuardarLog(ex);
                }
            }

            return tarajeMontacargas;
        }

        [WebMethod]
        public static HttpStatusCode ActualizarTransaccionProveedor(TransaccionPesoDetalle transaccionPesoDetalle)
        {
            _contextoSesion = HttpContext.Current;

            var respuestaHttpBus = new HttpStatusCode();

            if (_contextoSesion.User.Identity.IsAuthenticated)
            {
                var usuario = _contextoSesion.User.Identity.Name;

                string jsonString = JsonConvert.SerializeObject(new { idBascula = transaccionPesoDetalle.idBascula, idTransaccion = transaccionPesoDetalle.idTransaccionBascula, coincide = transaccionPesoDetalle.coincide });

                try
                {
                    var httpContent = new StringContent(jsonString, Encoding.UTF8, "application/json");

                    var response = _httpClient.PutAsync(Constantes.BUS_SERVICES_PATH_TT + "transaccion/asignaridsip", httpContent).Result;

                    var resultContent = response.Content.ReadAsStringAsync().Result;

                    var laRespuesta = JsonConvert.DeserializeObject<Respuesta>(resultContent);

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
                            respuestaHttpBus = HttpStatusCode.BadRequest;
                            ManejadorLogsErrores.GuardarLog(Constantes.ELEMENTO_BUS, mensajeRespuestaBus);
                        }
                    }
                    else
                    {
                        ManejadorLogsErrores.GuardarLog(Constantes.ELEMENTO_KENDO_GRID, "Error al actualizar el registro de transacción en el nodo de integración");
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
                respuestaHttpBus = HttpStatusCode.MethodNotAllowed;
            }

            return respuestaHttpBus;
        }
    }
}