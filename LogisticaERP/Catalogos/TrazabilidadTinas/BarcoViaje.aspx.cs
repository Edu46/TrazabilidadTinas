using LogisticaERP.Clases.TrazabilidadTinas;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Web;
using System.Web.Services;


namespace LogisticaERP.Catalogos.TrazabilidadTinas
{
    public partial class BarcoViaje : System.Web.UI.Page
    {
        private static HttpContext _contextoSesion;
        private static HttpClient _httpClient = new HttpClient();

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        [WebMethod]
        public static ListaBarcoViaje ObtenerCatalogoBarco(string idBarco, string fechaInicio, string fechaFin)
        {
            _contextoSesion = HttpContext.Current;

            string listaCatalogoBarcoViajeBus = "";
            ListaBarcoViaje listaCatalogoBarcoViaje = new ListaBarcoViaje();

            if (_contextoSesion.User.Identity.IsAuthenticated)
            {
                try
                {
                    _httpClient.DefaultRequestHeaders.Add("Accept", "application/json");

                    listaCatalogoBarcoViajeBus = _httpClient.GetStringAsync(Constantes.BUS_SERVICES_PATH_TT + "relaciones/barcos-viajes?idBarco=" + idBarco + "&fechaInicio=" + fechaInicio + "&fechaFin=" + fechaFin).Result;

                    listaCatalogoBarcoViaje = JsonConvert.DeserializeObject<ListaBarcoViaje>(listaCatalogoBarcoViajeBus);

                    for (int i = 0; i < listaCatalogoBarcoViaje.barcosViajes.Count; i++)
                    {
                        var listaidBarcoViajeParse = Convert.ToString(listaCatalogoBarcoViaje.barcosViajes[i].idBarco);
                        listaCatalogoBarcoViaje.barcosViajes[i].StringidBarco = listaidBarcoViajeParse;

                        var listaidViajeParse = Convert.ToString(listaCatalogoBarcoViaje.barcosViajes[i].idViaje);
                        listaCatalogoBarcoViaje.barcosViajes[i].StringidViaje = listaidViajeParse;
                    }
                }
                catch (Exception ex)
                {
                    listaCatalogoBarcoViaje = null;

                    ManejadorLogsErrores.GuardarLog(ex);
                }
            }
            return listaCatalogoBarcoViaje;
        }

        [WebMethod]
        public static ListaBarcoViaje CargarDDLBarco()
        {
            _contextoSesion = HttpContext.Current;

            string listaCatalogoBarcoViajeBus = "";
            ListaBarcoViaje listaCatalogoBarcoViaje = new ListaBarcoViaje();

            if (_contextoSesion.User.Identity.IsAuthenticated)
            {
                try
                {
                    _httpClient.DefaultRequestHeaders.Add("Accept", "application/json");

                    listaCatalogoBarcoViajeBus = _httpClient.GetStringAsync(Constantes.BUS_SERVICES_PATH_TT + "relaciones/barcos-viajes").Result;

                    listaCatalogoBarcoViaje = JsonConvert.DeserializeObject<ListaBarcoViaje>(listaCatalogoBarcoViajeBus);

                    for (int i = 0; i < listaCatalogoBarcoViaje.barcosViajes.Count; i++)
                    {
                        var listaidBarcoViajeParse = Convert.ToString(listaCatalogoBarcoViaje.barcosViajes[i].idBarco);
                        listaCatalogoBarcoViaje.barcosViajes[i].StringidBarco = listaidBarcoViajeParse;

                        var listaidViajeParse = Convert.ToString(listaCatalogoBarcoViaje.barcosViajes[i].idViaje);
                        listaCatalogoBarcoViaje.barcosViajes[i].StringidViaje = listaidViajeParse;
                    }
                }
                catch (Exception ex)
                {
                    listaCatalogoBarcoViaje = null;

                    ManejadorLogsErrores.GuardarLog(ex);
                }
            }
            return listaCatalogoBarcoViaje;
        }

    }
}