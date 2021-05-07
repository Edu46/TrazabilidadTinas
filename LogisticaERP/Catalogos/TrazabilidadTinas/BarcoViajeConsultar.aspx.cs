using LogisticaERP.Clases.TrazabilidadTinas;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Web;
using System.Web.Services;

namespace LogisticaERP.Catalogos.TrazabilidadTinas
{
    public partial class BarcoViajeConsultar : System.Web.UI.Page
    {
        private static HttpContext _contextoSesion;
        private static HttpClient _httpClient = new HttpClient();

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        [WebMethod]
        public static ListaBarcoViaje ObtenerViaje(string fechaInicio, string fechaFin, string idViaje)
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
                        for (int j = 0; j < listaCatalogoTanquesCertificacione.tanquesCertificaciones[i].certificaciones.Count; j++)
                        {
                            lista = lista + " - " + listaCatalogoTanquesCertificacione.tanquesCertificaciones[i].certificaciones[j].certificacion;
                        }
                        listaCatalogoTanquesCertificacione.tanquesCertificaciones[i].listaCertificaciones = lista;
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
    }
}