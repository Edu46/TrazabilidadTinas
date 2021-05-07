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
    public partial class BarcoViajeEditar : System.Web.UI.Page
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

        }

        [WebMethod]
        public static ListaBarcoViaje ObtenerViajes(string fechaInicio, string fechaFin, string idViaje)
        {
            _contextoSesion = HttpContext.Current;

            string listaViajeBus = "";
            ListaBarcoViaje listaViaje = new ListaBarcoViaje();

            if (_contextoSesion.User.Identity.IsAuthenticated)
            {
                try
                {
                    _httpClient.DefaultRequestHeaders.Add("Accept", "application/json");

                    listaViajeBus = _httpClient.GetStringAsync(Constantes.BUS_SERVICES_PATH_TT + "relaciones/barcos-viajes?fechaInicio=" + fechaInicio + "&fechaFin=" + fechaFin + "&idViaje=" + idViaje).Result;

                    listaViaje = JsonConvert.DeserializeObject<ListaBarcoViaje>(listaViajeBus);

                    for (int i = 0; i < listaViaje.barcosViajes.Count; i++)
                    {
                        var listaAlertasParseB = Convert.ToString(listaViaje.barcosViajes[i].idBarco);
                        listaViaje.barcosViajes[i].StringidBarco = listaAlertasParseB;

                        var listaAlertasParseV = Convert.ToString(listaViaje.barcosViajes[i].idViaje);
                        listaViaje.barcosViajes[i].StringidViaje = listaAlertasParseV;
                    }

                }
                catch (Exception ex)
                {
                    listaViaje = null;

                    ManejadorLogsErrores.GuardarLog(ex);
                }
            }

            return listaViaje;
        }

        [WebMethod]
        public static Respuesta CrearSubviaje(ViajeBarco datosViaje)
        {
            _contextoSesion = HttpContext.Current;
            Respuesta respuesta = new Respuesta();

            if (_contextoSesion.User.Identity.IsAuthenticated)
            {
                if (datosViaje != null)
                {
                    datosViaje.usuario = _contextoSesion.User.Identity.Name;

                    string jsonString = JsonConvert.SerializeObject(datosViaje);

                    try
                    {
                        var httpContent = new StringContent(jsonString, Encoding.UTF8, "application/json");

                        var response = _httpClient.PostAsync(Constantes.BUS_SERVICES_PATH_TT + "relaciones/barcos-viajes", httpContent).Result;

                        var resultContent = response.Content.ReadAsStringAsync().Result;

                        var laRespuesta = JsonConvert.DeserializeObject<Respuesta>(resultContent);

                        var contenidoRespuestaBus = response.Content.ReadAsStringAsync();
                        var mensajeRespuestaBus = contenidoRespuestaBus.Result;
                        var c = mensajeRespuestaBus.Split(',')[0];
                        var codigoError = c.Split(':')[1];

                        if (response != null)
                        {
                            if (response.IsSuccessStatusCode)
                            {
                                if (laRespuesta.resultado == "YES")
                                {
                                    respuesta.idViaje = laRespuesta.idViaje;
                                    respuesta.mensaje = laRespuesta.mensaje;
                                    respuesta.resultado = laRespuesta.resultado;
                                    respuesta.codigo = "200";
                                }
                                else
                                {
                                    respuesta.idViaje = laRespuesta.idViaje;
                                    respuesta.mensaje = laRespuesta.mensaje;
                                    respuesta.resultado = laRespuesta.resultado;
                                    respuesta.codigo = "409";
                                }
                            }
                            else
                            {

                                if (respuesta.codigo == "409")
                                {
                                    respuesta.idViaje = laRespuesta.idViaje;
                                    respuesta.mensaje = laRespuesta.mensaje;
                                    respuesta.resultado = laRespuesta.resultado;
                                    respuesta.codigo = "409";
                                }
                                else
                                {
                                    respuesta.idViaje = laRespuesta.idViaje;
                                    respuesta.mensaje = laRespuesta.mensaje;
                                    respuesta.resultado = laRespuesta.resultado;
                                    respuesta.codigo = "400";
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
                        respuesta.codigo = "400";
                        ManejadorLogsErrores.GuardarLog(ex);
                    }
                }
                else
                {
                    respuesta.codigo = "204";

                    ManejadorLogsErrores.GuardarLog(Constantes.ELEMENTO_BUS, "Error al obtener los datos del grid");
                }
            }
            else
            {
                respuesta.codigo = "405";
            }

            return respuesta;
        }

        [WebMethod]
        public static HttpStatusCode EditarBarcoViaje(ViajeBarco viajeBarco)
        {
            _contextoSesion = HttpContext.Current;

            var respuestaHttpBus = new HttpStatusCode();

            if (_contextoSesion.User.Identity.IsAuthenticated)
            {
                if (viajeBarco != null)
                {
                    viajeBarco.usuario = _contextoSesion.User.Identity.Name;

                    string jsonString = JsonConvert.SerializeObject(viajeBarco);

                    try
                    {
                        var httpContent = new StringContent(jsonString, Encoding.UTF8, "application/json");

                        var response = _httpClient.PutAsync(Constantes.BUS_SERVICES_PATH_TT + "relaciones/barcos-viajes", httpContent).Result;

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

        [WebMethod]
        public static HttpStatusCode EditarEstatusBarcoViaje(EstatusBarcoViaje estatusBarcoViaje)
        {
            _contextoSesion = HttpContext.Current;

            var respuestaHttpBus = new HttpStatusCode();

            if (_contextoSesion.User.Identity.IsAuthenticated)
            {
                if (estatusBarcoViaje != null)
                {
                    estatusBarcoViaje.usuario = _contextoSesion.User.Identity.Name;

                    string jsonString = JsonConvert.SerializeObject(estatusBarcoViaje);

                    try
                    {
                        var httpContent = new StringContent(jsonString, Encoding.UTF8, "application/json");

                        var response = _httpClient.PutAsync(Constantes.BUS_SERVICES_PATH_TT + "relaciones/barcos-viajes/estatus", httpContent).Result;

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

        [WebMethod]
        public static ListaTanqueCertificacion ObtenerTanquesCertificaciones(string idViaje)
        {
            _contextoSesion = HttpContext.Current;

            string listaCatalogosTanquesCertificacionesBus = "";
            ListaTanqueCertificacion listaCatalogoTanquesCertificacione = new ListaTanqueCertificacion();

            if (_contextoSesion.User.Identity.IsAuthenticated)
            {
                try
                {
                    _httpClient.DefaultRequestHeaders.Add("Accept", "application/json");

                    listaCatalogosTanquesCertificacionesBus = _httpClient.GetStringAsync(Constantes.BUS_SERVICES_PATH_TT + "relaciones/tanques-certificaciones?idViaje=" + idViaje).Result;

                    listaCatalogoTanquesCertificacione = JsonConvert.DeserializeObject<ListaTanqueCertificacion>(listaCatalogosTanquesCertificacionesBus);

                    for (int i = 0; i < listaCatalogoTanquesCertificacione.tanquesCertificaciones.Count; i++)
                    {
                        var lista = "";
                        var listaId = "";
                        for (int j = 0; j <= listaCatalogoTanquesCertificacione.tanquesCertificaciones[i].certificaciones.Count - 1; j++)
                        {
                            lista = lista + " -  " + listaCatalogoTanquesCertificacione.tanquesCertificaciones[i].certificaciones[j].certificacion;
                            listaId = listaId + listaCatalogoTanquesCertificacione.tanquesCertificaciones[i].certificaciones[j].idCertificacion;

                        }
                        listaCatalogoTanquesCertificacione.tanquesCertificaciones[i].listaCertificaciones = lista;
                        listaCatalogoTanquesCertificacione.tanquesCertificaciones[i].listaIdCertificaciones = listaId;
                    }

                }
                catch (Exception ex)
                {
                    listaCatalogoTanquesCertificacione = null;

                    ManejadorLogsErrores.GuardarLog(ex);
                }
            }

            return listaCatalogoTanquesCertificacione;
        }

        [WebMethod]
        public static ListaTanque ObtenerTanques(string idBarco)
        {
            _contextoSesion = HttpContext.Current;
            string listaTanqueBus = "";
            ListaTanque listaTanque = new ListaTanque();

            if (_contextoSesion.User.Identity.IsAuthenticated)
            {
                try
                {
                    _httpClient.DefaultRequestHeaders.Add("Accept", "application/json");

                    listaTanqueBus = _httpClient.GetStringAsync(Constantes.BUS_SERVICES_PATH_TT + "catalogos/tanques?idBarco=" + idBarco).Result;

                    listaTanque = JsonConvert.DeserializeObject<ListaTanque>(listaTanqueBus);

                    for (int i = 0; i < listaTanque.tanques.Count; i++)
                    {
                        var listaTanqueIdBarcoParse = Convert.ToString(listaTanque.tanques[i].idBarco);
                        listaTanque.tanques[i].StringidBarco = listaTanqueIdBarcoParse;

                        var listaTanqueidTanqueParse = Convert.ToString(listaTanque.tanques[i].idTanque);
                        listaTanque.tanques[i].StingidTanque = listaTanqueidTanqueParse;
                    }
                }
                catch (Exception ex)
                {
                    listaTanque = null;

                    ManejadorLogsErrores.GuardarLog(ex);
                }
            }

            return listaTanque;
        }

        [WebMethod]
        public static ListaCertificacion ObtenerCertificaciones()
        {
            _contextoSesion = HttpContext.Current;
            string listaCertificacionBus = "";
            ListaCertificacion listaCertificacion = new ListaCertificacion();

            if (_contextoSesion.User.Identity.IsAuthenticated)
            {
                try
                {
                    _httpClient.DefaultRequestHeaders.Add("Accepti", "application/json");

                    listaCertificacionBus = _httpClient.GetStringAsync(Constantes.BUS_SERVICES_PATH_TT + "catalogos/certificaciones?activo=true").Result;

                    listaCertificacion = JsonConvert.DeserializeObject<ListaCertificacion>(listaCertificacionBus);
                }
                catch (Exception ex)
                {
                    listaCertificacion = null;

                    ManejadorLogsErrores.GuardarLog(ex);
                }
            }

            return listaCertificacion;
        }

        [WebMethod]
        public static HttpStatusCode CrearTanqueCertificacion(TanqueCertificacion tanqueCertificacion)
        {
            _contextoSesion = HttpContext.Current;

            var respuestaHttpBus = new HttpStatusCode();

            if (_contextoSesion.User.Identity.IsAuthenticated)
            {
                if (tanqueCertificacion != null)
                {
                    tanqueCertificacion.usuario = _contextoSesion.User.Identity.Name;

                    string jsonString = JsonConvert.SerializeObject(tanqueCertificacion);

                    try
                    {
                        var httpContent = new StringContent(jsonString, Encoding.UTF8, "application/json");

                        var response = _httpClient.PostAsync(Constantes.BUS_SERVICES_PATH_TT + "relaciones/viajes/tanques-certificaciones", httpContent).Result;

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
        public static HttpStatusCode EditarTanqueCertificacion(TanqueCertificacion tanqueCertificacion)
        {
            _contextoSesion = HttpContext.Current;

            var respuestaHttpBus = new HttpStatusCode();

            if (_contextoSesion.User.Identity.IsAuthenticated)
            {
                if (tanqueCertificacion != null)
                {
                    tanqueCertificacion.usuario = _contextoSesion.User.Identity.Name;

                    string jsonString = JsonConvert.SerializeObject(tanqueCertificacion);

                    try
                    {
                        var httpContent = new StringContent(jsonString, Encoding.UTF8, "application/json");

                        var response = _httpClient.PutAsync(Constantes.BUS_SERVICES_PATH_TT + "relaciones/viajes/tanques-certificaciones", httpContent).Result;

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

        [WebMethod]
        public static ListaViajeProducto ObtenerViajesProductos(string idViaje)
        {
            _contextoSesion = HttpContext.Current;

            string listalistaviajesProductosBus = "";
            ListaViajeProducto listaviajesProductos = new ListaViajeProducto();

            if (_contextoSesion.User.Identity.IsAuthenticated)
            {
                try
                {
                    _httpClient.DefaultRequestHeaders.Add("Accept", "application/json");

                    listalistaviajesProductosBus = _httpClient.GetStringAsync(Constantes.BUS_SERVICES_PATH_TT + "relaciones/viajes-productos?idViaje=" + idViaje).Result;

                    listaviajesProductos = JsonConvert.DeserializeObject<ListaViajeProducto>(listalistaviajesProductosBus);

                }
                catch (Exception ex)
                {
                    listaviajesProductos = null;

                    ManejadorLogsErrores.GuardarLog(ex);
                }
            }

            return listaviajesProductos;
        }

        [WebMethod]
        public static ListaProductos ObtenerProducto()
        {
            _contextoSesion = HttpContext.Current;
            string listaProductoBus = "";
            ListaProductos listaProducto = new ListaProductos();

            if (_contextoSesion.User.Identity.IsAuthenticated)
            {
                try
                {
                    _httpClient.DefaultRequestHeaders.Add("Accept", "application/json");

                    listaProductoBus = _httpClient.GetStringAsync(Constantes.BUS_SERVICES_PATH_TT + "catalogos/producto").Result;

                    listaProducto = JsonConvert.DeserializeObject<ListaProductos>(listaProductoBus);

                    for (int i = 0; i < listaProducto.catalogoProductos.Count; i++)
                    {
                        var listaProductoIdProductoParse = Convert.ToString(listaProducto.catalogoProductos[i].idProducto);
                        listaProducto.catalogoProductos[i].StringidProducto = listaProductoIdProductoParse;
                    }
                }
                catch (Exception ex)
                {
                    listaProducto = null;

                    ManejadorLogsErrores.GuardarLog(ex);
                }
            }

            return listaProducto;
        }

        [WebMethod]
        public static HttpStatusCode CrearProducto(Producto producto)
        {
            _contextoSesion = HttpContext.Current;

            var respuestaHttpBus = new HttpStatusCode();

            if (_contextoSesion.User.Identity.IsAuthenticated)
            {
                if (producto != null)
                {
                    producto.usuario = _contextoSesion.User.Identity.Name;

                    string jsonString = JsonConvert.SerializeObject(producto);

                    try
                    {
                        var httpContent = new StringContent(jsonString, Encoding.UTF8, "application/json");

                        var response = _httpClient.PostAsync(Constantes.BUS_SERVICES_PATH_TT + "relaciones/viajes-productos", httpContent).Result;

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
        public static HttpStatusCode EditarProducto(Producto producto)
        {
            _contextoSesion = HttpContext.Current;

            var respuestaHttpBus = new HttpStatusCode();

            if (_contextoSesion.User.Identity.IsAuthenticated)
            {
                if (producto != null)
                {
                    producto.usuario = _contextoSesion.User.Identity.Name;

                    string jsonString = JsonConvert.SerializeObject(producto);

                    try
                    {
                        var httpContent = new StringContent(jsonString, Encoding.UTF8, "application/json");

                        var response = _httpClient.PutAsync(Constantes.BUS_SERVICES_PATH_TT + "relaciones/viajes-productos", httpContent).Result;

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

        [WebMethod]
        public static HttpStatusCode EliminarProductos(string idViajeProducto, string idViaje, string idProducto)
        {
            _contextoSesion = HttpContext.Current;

            var respuestaHttpBus = new HttpStatusCode();

            if (_contextoSesion.User.Identity.IsAuthenticated)
            {
                try
                {
                    var response = _httpClient.DeleteAsync(Constantes.BUS_SERVICES_PATH_TT + "relaciones/viajes-productos?idViajeProducto=" + idViajeProducto + "&idViaje=" + idViaje + "&idProducto=" + idProducto).Result;

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
                            var c = mensajeRespuestaBus.Split(',')[0];
                            var codigoError = c.Split(':')[1];

                            if (codigoError == "701")
                            {
                                respuestaHttpBus = HttpStatusCode.Conflict;
                            }
                            else
                            {
                                respuestaHttpBus = HttpStatusCode.BadRequest;
                            }
                        }
                    }
                    else
                    {
                        respuestaHttpBus = HttpStatusCode.NoContent;

                        ManejadorLogsErrores.GuardarLog(Constantes.ELEMENTO_BUS, "Error al eliminar el dato");
                    }
                }
                catch (Exception ex)
                {
                    respuestaHttpBus = HttpStatusCode.BadRequest;

                    ManejadorLogsErrores.GuardarLog(ex);
                }
            }

            return respuestaHttpBus;
        }

        [WebMethod]
        public static HttpStatusCode EliminarTanquesCertificaciones(string idBarco, string idViaje, string idTanque)
        {
            _contextoSesion = HttpContext.Current;

            var respuestaHttpBus = new HttpStatusCode();

            if (_contextoSesion.User.Identity.IsAuthenticated)
            {
                try
                {
                    var response = _httpClient.DeleteAsync(Constantes.BUS_SERVICES_PATH_TT + "relaciones/viajes/tanques-certificaciones?idBarco=" + idBarco + "&idViaje=" + idViaje + "&idTanque=" + idTanque).Result;

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
                            var c = mensajeRespuestaBus.Split(',')[0];
                            var codigoError = c.Split(':')[1];

                            if (codigoError == "701")
                            {
                                respuestaHttpBus = HttpStatusCode.Conflict;
                            }
                            else
                            {
                                respuestaHttpBus = HttpStatusCode.BadRequest;
                            }
                        }
                    }
                    else
                    {
                        respuestaHttpBus = HttpStatusCode.NoContent;

                        ManejadorLogsErrores.GuardarLog(Constantes.ELEMENTO_BUS, "Error al eliminar el dato");
                    }
                }
                catch (Exception ex)
                {
                    respuestaHttpBus = HttpStatusCode.BadRequest;

                    ManejadorLogsErrores.GuardarLog(ex);
                }
            }

            return respuestaHttpBus;
        }

    }
}