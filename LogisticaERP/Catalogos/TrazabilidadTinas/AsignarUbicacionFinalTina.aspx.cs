using LogisticaERP.Clases.TrazabilidadTinas;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.NetworkInformation;
using System.Text;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace LogisticaERP.Catalogos.TrazabilidadTinas
{
    public partial class AsignarUbicacionFinalTina : System.Web.UI.Page
    {
        private static HttpContext _contextoSesion;
        private static HttpClient _httpClient = new HttpClient();

        public class RespuestaServicio
        {
            public string mensaje { get; set; }
            public string resultado { get; set; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            
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
        public static ListaTipoEntrada ObtenerTiposEntrada()
        {
            _contextoSesion = HttpContext.Current;

            string listaTipoEntradaBus = "";
            ListaTipoEntrada listaTipoEntrada = new ListaTipoEntrada();

            if (_contextoSesion.User.Identity.IsAuthenticated)
            {
                try
                {
                    _httpClient.DefaultRequestHeaders.Add("Accept", "application/json");

                    listaTipoEntradaBus = _httpClient.GetStringAsync(Constantes.BUS_SERVICES_PATH_TT + "catalogos/tipo-entrada").Result;

                    listaTipoEntrada = JsonConvert.DeserializeObject<ListaTipoEntrada>(listaTipoEntradaBus);
                }
                catch (Exception ex)
                {
                    listaTipoEntrada = null;

                    ManejadorLogsErrores.GuardarLog(ex);
                }
            }

            return listaTipoEntrada;
        }

        [WebMethod]
        public static ListaFrigorifico ObtenerFrigorificos()
        {
            _contextoSesion = HttpContext.Current;

            string listaFrigorificoBus = "";
            ListaFrigorifico listaFrigorifico = new ListaFrigorifico();

            if (_contextoSesion.User.Identity.IsAuthenticated)
            {
                try
                {
                    _httpClient.DefaultRequestHeaders.Add("Accept", "application/json");

                    listaFrigorificoBus = _httpClient.GetStringAsync(Constantes.BUS_SERVICES_PATH_TT + "catalogos/frigorificos/antecamaras").Result;

                    listaFrigorifico = JsonConvert.DeserializeObject<ListaFrigorifico>(listaFrigorificoBus);
                }
                catch (Exception ex)
                {
                    listaFrigorifico = null;

                    ManejadorLogsErrores.GuardarLog(ex);
                }
            }

            return listaFrigorifico;
        }

        [WebMethod]
        public static ListaUbicacionExterna ObtenerUbicacionesExternas()
        {
            _contextoSesion = HttpContext.Current;

            string listaUbicacionExternaBus = "";
            ListaUbicacionExterna listaUbicacionExterna = new ListaUbicacionExterna();

            if (_contextoSesion.User.Identity.IsAuthenticated)
            {
                try
                {
                    _httpClient.DefaultRequestHeaders.Add("Accept", "application/json");

                    listaUbicacionExternaBus = _httpClient.GetStringAsync(Constantes.BUS_SERVICES_PATH_TT + "ubicaciones/externas").Result;

                    listaUbicacionExterna = JsonConvert.DeserializeObject<ListaUbicacionExterna>(listaUbicacionExternaBus);

                }
                catch (Exception ex)
                {
                    listaUbicacionExterna = null;

                    ManejadorLogsErrores.GuardarLog(ex);
                }
            }

            return listaUbicacionExterna;
        }

        [WebMethod]
        public static Tina ObtenerDatosBasculaActual(int idBascula)
        {
            _contextoSesion = HttpContext.Current;

            string datosBasculaActualBus = "";
            Tina datosBasculaActual = new Tina();

            if (_contextoSesion.User.Identity.IsAuthenticated)
            {
                try
                {
                    _httpClient.DefaultRequestHeaders.Add("Accept", "application/json");

                    datosBasculaActualBus = _httpClient.GetStringAsync(Constantes.BUS_SERVICES_PATH_TT + "tinas/bascula-actual?idBascula=" + idBascula).Result;

                    datosBasculaActual = JsonConvert.DeserializeObject<Tina>(datosBasculaActualBus);

                }
                catch (Exception ex)
                {
                    datosBasculaActual = null;

                    ManejadorLogsErrores.GuardarLog(ex);
                }
            }

            return datosBasculaActual;
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
        public static ListaAreaFrigorifico ConsultarAreasFrigorifico(string idAlmacen)
        {
            _contextoSesion = HttpContext.Current;

            string listaAreaFrigorificoBus = "";
            ListaAreaFrigorifico listaAreaFrigorifico = new ListaAreaFrigorifico();

            if (_contextoSesion.User.Identity.IsAuthenticated)
            {
                try
                {
                    _httpClient.DefaultRequestHeaders.Add("Accept", "application/json");

                    listaAreaFrigorificoBus = _httpClient.GetStringAsync(Constantes.BUS_SERVICES_PATH_TT + "ubicaciones/almacen/area?idAlmacen=" + idAlmacen).Result;

                    listaAreaFrigorifico = JsonConvert.DeserializeObject<ListaAreaFrigorifico>(listaAreaFrigorificoBus);
                }
                catch (Exception ex)
                {
                    listaAreaFrigorifico = null;

                    ManejadorLogsErrores.GuardarLog(ex);
                }
            }

            return listaAreaFrigorifico;
        }

        [WebMethod]
        public static AreaFrigorifico ConsultarDetalleArea(string idAlmacen, string area)
        {
            _contextoSesion = HttpContext.Current;

            string detalleAreaBus = "";
            AreaFrigorifico detalleArea = new AreaFrigorifico();

            if (_contextoSesion.User.Identity.IsAuthenticated)
            {
                try
                {
                    _httpClient.DefaultRequestHeaders.Add("Accept", "application/json");

                    detalleAreaBus = _httpClient.GetStringAsync(Constantes.BUS_SERVICES_PATH_TT + "ubicaciones/almacen/lineas-columnas?idAlmacen=" + idAlmacen + "&area=" + area).Result;

                    detalleArea = JsonConvert.DeserializeObject<AreaFrigorifico>(detalleAreaBus);

                }
                catch (Exception ex)
                {
                    detalleArea = null;

                    ManejadorLogsErrores.GuardarLog(ex);
                }
            }

            return detalleArea;
        }

        [WebMethod]
        public static ListaUbicacionAreaFrigorifico ConsultarDetalleUbicaciones(string idAlmacen, string area)
        {
            _contextoSesion = HttpContext.Current;

            string listaUbicacionAreaFrigorificoBus = "";
            ListaUbicacionAreaFrigorifico listaUbicacionAreaFrigorifico = new ListaUbicacionAreaFrigorifico();

            if (_contextoSesion.User.Identity.IsAuthenticated)
            {
                try
                {
                    _httpClient.DefaultRequestHeaders.Add("Accept", "application/json");

                    listaUbicacionAreaFrigorificoBus = _httpClient.GetStringAsync(Constantes.BUS_SERVICES_PATH_TT + "ubicaciones/almacen/generales?idAlmacen=" + idAlmacen + "&area=" + area).Result;

                    listaUbicacionAreaFrigorifico = JsonConvert.DeserializeObject<ListaUbicacionAreaFrigorifico>(listaUbicacionAreaFrigorificoBus);
                }
                catch (Exception ex)
                {
                    listaUbicacionAreaFrigorifico = null;

                    ManejadorLogsErrores.GuardarLog(ex);
                }
            }

            return listaUbicacionAreaFrigorifico;
        }

        [WebMethod]
        public static ListaTina ObtenerDesgloseTina(string idUbicacion, string idAlmacen)
        {
            _contextoSesion = HttpContext.Current;

            string desgloseTinaBus = "";
            ListaTina desgloseTina = new ListaTina();

            if (_contextoSesion.User.Identity.IsAuthenticated)
            {
                try
                {
                    _httpClient.DefaultRequestHeaders.Add("Accept", "application/json");

                    desgloseTinaBus = _httpClient.GetStringAsync(Constantes.BUS_SERVICES_PATH_TT + "ubicaciones/almacen/tinas?idUbicacion=" + idUbicacion + "&idAlmacen=" + idAlmacen).Result;

                    desgloseTina = JsonConvert.DeserializeObject<ListaTina>(desgloseTinaBus);

                }
                catch (Exception ex)
                {
                    desgloseTina = null;

                    ManejadorLogsErrores.GuardarLog(ex);
                }
            }

            return desgloseTina;
        }

        [WebMethod]
        public static HttpStatusCode ActualizarTransaccionProveedor(TransaccionPesoDetalle transaccionPesoDetalle)
        {
            _contextoSesion = HttpContext.Current;

            var respuestaHttpBus = new HttpStatusCode();

            if (_contextoSesion.User.Identity.IsAuthenticated)
            {
                if (transaccionPesoDetalle != null)
                {
                    string jsonString = JsonConvert.SerializeObject(new { idBascula = transaccionPesoDetalle.idBascula, idTransaccion = transaccionPesoDetalle.idTransaccionBascula, coincide = transaccionPesoDetalle.coincide });

                    try
                    {
                        var httpContent = new StringContent(jsonString, Encoding.UTF8, "application/json");

                        var response = _httpClient.PutAsync(Constantes.BUS_SERVICES_PATH_TT + "transaccion/asignaridsip", httpContent).Result;

                        if (response != null)
                        {
                            var resultContent = response.Content.ReadAsStringAsync().Result;

                            var respuestaServicio = JsonConvert.DeserializeObject<RespuestaServicio>(resultContent);

                            if (response.IsSuccessStatusCode)
                            {
                                if (respuestaServicio.resultado == "NO")
                                {
                                    respuestaHttpBus = HttpStatusCode.Conflict;

                                    ManejadorLogsErrores.GuardarLog(Constantes.ELEMENTO_BUS, respuestaServicio.mensaje);
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
                    respuestaHttpBus = HttpStatusCode.NoContent;

                    ManejadorLogsErrores.GuardarLog(Constantes.ELEMENTO_BUS, "Error al obtener los datos del pesaje para actualizar");
                }
            }
            else
            {
                respuestaHttpBus = HttpStatusCode.MethodNotAllowed;
            }

            return respuestaHttpBus;
        }

        [WebMethod]
        public static HttpStatusCode AsignarUbicacionTinaSAI(TinaDetalle tina)
        {
            _contextoSesion = HttpContext.Current;

            var respuestaHttpBus = new HttpStatusCode();

            if (_contextoSesion.User.Identity.IsAuthenticated)
            {
                if (tina != null)
                {
                    tina.usuario = _contextoSesion.User.Identity.Name;

                    string jsonString = JsonConvert.SerializeObject(tina);

                    try
                    {
                        var httpContent = new StringContent(jsonString, Encoding.UTF8, "application/json");

                        var response = _httpClient.PostAsync(Constantes.BUS_SERVICES_PATH_TT + "tinas/entrada-movimiento", httpContent).Result;

                        if (response != null)
                        {
                            var resultContent = response.Content.ReadAsStringAsync().Result;

                            var respuestaServicio = JsonConvert.DeserializeObject<RespuestaServicio>(resultContent);

                            if (response.IsSuccessStatusCode)
                            {
                                if (respuestaServicio.resultado == "NO")
                                {
                                    respuestaHttpBus = HttpStatusCode.Conflict;

                                    ManejadorLogsErrores.GuardarLog(Constantes.ELEMENTO_BUS, respuestaServicio.mensaje);
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
                                respuestaHttpBus = HttpStatusCode.BadRequest;
                                ManejadorLogsErrores.GuardarLog(Constantes.ELEMENTO_BUS, mensajeRespuestaBus);
                            }
                        }
                        else
                        {
                            ManejadorLogsErrores.GuardarLog(Constantes.ELEMENTO_KENDO_GRID, "Error al obtener la respuesta del nodo de integración (SAI)");
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

                    ManejadorLogsErrores.GuardarLog(Constantes.ELEMENTO_BUS, "Error al obtener los datos de la tina para su asignación");
                }
            }
            else
            {
                respuestaHttpBus = HttpStatusCode.MethodNotAllowed;
            }

            return respuestaHttpBus;
        }

        [WebMethod]
        public static HttpStatusCode AsignarUbicacionTinaSIP(TinaDetalle tina)
        {
            _contextoSesion = HttpContext.Current;

            var respuestaHttpBus = new HttpStatusCode();

            if (_contextoSesion.User.Identity.IsAuthenticated)
            {
                if (tina != null)
                {
                    tina.usuario = _contextoSesion.User.Identity.Name;

                    string jsonString = JsonConvert.SerializeObject(tina);

                    try
                    {
                        var httpContent = new StringContent(jsonString, Encoding.UTF8, "application/json");

                        var response = _httpClient.PostAsync(Constantes.BUS_SERVICES_PATH_TT + "tinas/asignar", httpContent).Result;

                        if (response != null)
                        {
                            var resultContent = response.Content.ReadAsStringAsync().Result;

                            var respuestaServicio = JsonConvert.DeserializeObject<RespuestaServicio>(resultContent);

                            if (response.IsSuccessStatusCode)
                            {
                                if (respuestaServicio.resultado == "NO")
                                {
                                    respuestaHttpBus = HttpStatusCode.Conflict;

                                    ManejadorLogsErrores.GuardarLog(Constantes.ELEMENTO_BUS, respuestaServicio.mensaje);
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
                                respuestaHttpBus = HttpStatusCode.BadRequest;
                                ManejadorLogsErrores.GuardarLog(Constantes.ELEMENTO_BUS, mensajeRespuestaBus);
                            }
                        }
                        else
                        {
                            ManejadorLogsErrores.GuardarLog(Constantes.ELEMENTO_KENDO_GRID, "Error al obtener la respuesta del nodo de integración (SIP)");
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

                    ManejadorLogsErrores.GuardarLog(Constantes.ELEMENTO_BUS, "Error al obtener los datos de la tina para su asignación");
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