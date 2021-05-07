using LogisticaERP.Clases.TrazabilidadTinas;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web;
using System.Web.Services;

namespace LogisticaERP.Catalogos.TrazabilidadTinas
{
    public partial class BarcoViajeCrear : System.Web.UI.Page
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
        public static ListaBarco ObtenerBarcos()
        {
            _contextoSesion = HttpContext.Current;

            string listaCatalogosBarcosBus = "";
            ListaBarco listaBarco = new ListaBarco();

            if (_contextoSesion.User.Identity.IsAuthenticated)
            {
                try
                {
                    _httpClient.DefaultRequestHeaders.Add("Accept", "application/json");

                    listaCatalogosBarcosBus = _httpClient.GetStringAsync(Constantes.BUS_SERVICES_PATH_TT + "catalogos/barcos").Result;

                    listaBarco = JsonConvert.DeserializeObject<ListaBarco>(listaCatalogosBarcosBus);

                    for (int i = 0; i < listaBarco.catalogoBarcos.Count; i++)
                    {
                        var listaBarcoParse = Convert.ToString(listaBarco.catalogoBarcos[i].idBarco);
                        listaBarco.catalogoBarcos[i].StringIdBarco = listaBarcoParse;

                        var listaidEmpresaParse = Convert.ToString(listaBarco.catalogoBarcos[i].idEmpresa);
                        listaBarco.catalogoBarcos[i].StringidEmpresa = listaidEmpresaParse;

                    }
                }
                catch (Exception ex)
                {
                    listaBarco = null;

                    ManejadorLogsErrores.GuardarLog(ex);
                }
            }

            return listaBarco;
        }

        [WebMethod]
        public static ListaBarco ObtenerDetalleBarcos(string idBarco)
        {
            _contextoSesion = HttpContext.Current;

            string listaDetalleBarcosBus = "";
            ListaBarco listaDetalleBarco = new ListaBarco();

            if (_contextoSesion.User.Identity.IsAuthenticated)
            {
                try
                {
                    _httpClient.DefaultRequestHeaders.Add("Accept", "application/json");

                    listaDetalleBarcosBus = _httpClient.GetStringAsync(Constantes.BUS_SERVICES_PATH_TT + "catalogos/barcos?idBarco=" + idBarco).Result;

                    listaDetalleBarco = JsonConvert.DeserializeObject<ListaBarco>(listaDetalleBarcosBus);

                    for (int i = 0; i < listaDetalleBarco.catalogoBarcos.Count; i++)
                    {
                        var listaBarcoParse = Convert.ToString(listaDetalleBarco.catalogoBarcos[i].idBarco);
                        listaDetalleBarco.catalogoBarcos[i].StringIdBarco = listaBarcoParse;

                    }
                }
                catch (Exception ex)
                {
                    listaDetalleBarco = null;

                    ManejadorLogsErrores.GuardarLog(ex);
                }
            }

            return listaDetalleBarco;
        }

        [WebMethod]
        public static ListaSteamer ObtenerSteamer()
        {
            _contextoSesion = HttpContext.Current;

            string listaSteamerBus = "";
            ListaSteamer listaSteamer = new ListaSteamer();

            if (_contextoSesion.User.Identity.IsAuthenticated)
            {
                try
                {
                    _httpClient.DefaultRequestHeaders.Add("Accept", "application/json");

                    listaSteamerBus = _httpClient.GetStringAsync(Constantes.BUS_SERVICES_PATH_TT + "catalogos/steamers").Result;

                    listaSteamer = JsonConvert.DeserializeObject<ListaSteamer>(listaSteamerBus);

                    for (int i = 0; i < listaSteamer.steamers.Count; i++)
                    {
                        var listaSteamerParse = Convert.ToString(listaSteamer.steamers[i].idSteamer);
                        listaSteamer.steamers[i].StringidSteamer = listaSteamerParse;
                    }
                }
                catch (Exception ex)
                {
                    listaSteamer = null;

                    ManejadorLogsErrores.GuardarLog(ex);
                }
            }

            return listaSteamer;
        }

        [WebMethod]
        public static ListaSteamer ObtenerDetalleSteamer(string idSteamer)
        {
            _contextoSesion = HttpContext.Current;

            string listaSteamerBus = "";
            ListaSteamer listaSteamer = new ListaSteamer();

            if (_contextoSesion.User.Identity.IsAuthenticated)
            {
                try
                {
                    _httpClient.DefaultRequestHeaders.Add("Accept", "application/json");

                    listaSteamerBus = _httpClient.GetStringAsync(Constantes.BUS_SERVICES_PATH_TT + "catalogos/steamers?idSteamer=" + idSteamer).Result;

                    listaSteamer = JsonConvert.DeserializeObject<ListaSteamer>(listaSteamerBus);

                    for (int i = 0; i < listaSteamer.steamers.Count; i++)
                    {
                        var listaSteamerParse = Convert.ToString(listaSteamer.steamers[i].idSteamer);
                        listaSteamer.steamers[i].StringidSteamer = listaSteamerParse;
                    }
                }
                catch (Exception ex)
                {
                    listaSteamer = null;

                    ManejadorLogsErrores.GuardarLog(ex);
                }
            }

            return listaSteamer;
        }

        [WebMethod]
        public static ListaFolioIdentificador ObtenerFolioIdentificador(string idEmpresa)
        {
            _contextoSesion = HttpContext.Current;

            string listaFolioIdentificadorBus = "";
            ListaFolioIdentificador listaFolioIdentificador = new ListaFolioIdentificador();

            if (_contextoSesion.User.Identity.IsAuthenticated)
            {
                try
                {
                    _httpClient.DefaultRequestHeaders.Add("Accept", "application/json");

                    listaFolioIdentificadorBus = _httpClient.GetStringAsync(Constantes.BUS_SERVICES_PATH_TT + "catalogos/folio-identificador?idEmpresa=" + idEmpresa).Result;

                    listaFolioIdentificador = JsonConvert.DeserializeObject<ListaFolioIdentificador>(listaFolioIdentificadorBus);

                }
                catch (Exception ex)
                {
                    listaFolioIdentificador = null;

                    ManejadorLogsErrores.GuardarLog(ex);
                }
            }

            return listaFolioIdentificador;
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

                    for (int i = 0; i <= listaCatalogoTanquesCertificacione.tanquesCertificaciones.Count -1; i++)
                    {
                        var lista = "";
                        var listaId = "";
                        for (int j = 0; j <= listaCatalogoTanquesCertificacione.tanquesCertificaciones[i].certificaciones.Count - 1; j++)
                        {
                            lista = lista + " - " + listaCatalogoTanquesCertificacione.tanquesCertificaciones[i].certificaciones[j].certificacion;
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
        public static SiguienteViaje ObtenerViaje(string idBarco, string ejercicio)
        {
            _contextoSesion = HttpContext.Current;
            string siguienteViajeBus = "";
            SiguienteViaje siguienteViaje = new SiguienteViaje();

            if (_contextoSesion.User.Identity.IsAuthenticated)
            {
                try
                {
                    _httpClient.DefaultRequestHeaders.Add("Accept", "application/json");

                    siguienteViajeBus = _httpClient.GetStringAsync(Constantes.BUS_SERVICES_PATH_TT + "catalogos/siguiente/viaje?idBarco=" + idBarco + "&ejercicio=" + ejercicio).Result;

                    siguienteViaje = JsonConvert.DeserializeObject<SiguienteViaje>(siguienteViajeBus);
                }
                catch (Exception ex)
                {
                    siguienteViaje = null;

                    ManejadorLogsErrores.GuardarLog(ex);
                }
            }

            return siguienteViaje;
        }

        [WebMethod]
        public static ListaViajePesca ObtenerViajePesca(string idBarco)
        {
            _contextoSesion = HttpContext.Current;
            string listaViajePescaBus = "";
            ListaViajePesca listaViajePesca = new ListaViajePesca();

            if (_contextoSesion.User.Identity.IsAuthenticated)
            {
                try
                {
                    _httpClient.DefaultRequestHeaders.Add("Accept", "application/json");

                    listaViajePescaBus = _httpClient.GetStringAsync(Constantes.BUS_SERVICES_PATH_TT + "catalogos/viajes-pesca?idBarco=" + idBarco).Result;

                    listaViajePesca = JsonConvert.DeserializeObject<ListaViajePesca>(listaViajePescaBus);

                    for (int i = 0; i < listaViajePesca.viajesPesca.Count; i++)
                    {
                        var listaProductoIdProductoParse = Convert.ToString(listaViajePesca.viajesPesca[i].idBarco);
                        listaViajePesca.viajesPesca[i].StringidBarco = listaProductoIdProductoParse;
                    }
                }
                catch (Exception ex)
                {
                    listaViajePesca = null;

                    ManejadorLogsErrores.GuardarLog(ex);
                }
            }

            return listaViajePesca;
        }

        [WebMethod]
        public static ListaViajeProducto ObtenerViajesProductos(string idViaje)
        {
            _contextoSesion = HttpContext.Current;
            string listaProductoBus = "";
            ListaViajeProducto listaViajeProducto = new ListaViajeProducto();

            if (_contextoSesion.User.Identity.IsAuthenticated)
            {
                try
                {
                    _httpClient.DefaultRequestHeaders.Add("Accept", "application/json");

                    listaProductoBus = _httpClient.GetStringAsync(Constantes.BUS_SERVICES_PATH_TT + "relaciones/viajes-productos?idViaje=" + idViaje).Result;

                    listaViajeProducto = JsonConvert.DeserializeObject<ListaViajeProducto>(listaProductoBus);

                }
                catch (Exception ex)
                {
                    listaViajeProducto = null;

                    ManejadorLogsErrores.GuardarLog(ex);
                }
            }

            return listaViajeProducto;
        }


        [WebMethod]
        public static Respuesta CrearBarcoViaje(ViajeBarco viajeBarco)
        {
            _contextoSesion = HttpContext.Current;
            Respuesta respuesta = new Respuesta();

            if (_contextoSesion.User.Identity.IsAuthenticated)
            {
                if (viajeBarco != null)
                {
                    viajeBarco.usuario = _contextoSesion.User.Identity.Name;

                    string jsonString = JsonConvert.SerializeObject(viajeBarco);

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