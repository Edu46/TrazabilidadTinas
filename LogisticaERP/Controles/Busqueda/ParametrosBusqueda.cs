using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
using System.Web.UI;

namespace GRUPOPINSA.Controles.Busqueda
{
    [Serializable]
    public class ParametrosBusqueda
    {

        public enum ModuloBusqueda
        {
            INVENTARIOS,
            COMPRAS,
            MANTENIMIENTO,
            PROYECTOS,
            GRUPOPINSA,
            SEGURIDAD
        }

        public enum TipoBusqueda
        {
            BUSQUEDA_AVANZADA,
            BUSQUEDA_ARTICULO,
            BUSQUEDA_ARTICULO_EXISTENCIA,
            BUSQUEDA_ARTICULO_NOPARTE,
            BUSQUEDA_EMPLEADO,
            CUENTAS_CONTABLES,
            SELECCIONAR_CUENTA
        }

        public List<CampoFiltrar> Filtros { get; set; }
        public List<CampoResultado> Resultados { get; set; }
        //public BusquedaAvanzadaeTablasBusqueda TablaBusqueda { get; set; }
        public Object TablaBusqueda { get; set; }
        public List<FiltroPersonalizado> FiltroPersonalizado { get; set; }
        public ModuloBusqueda BuscaModulo { get; set; }
        public String Token { get; set; }
        public string functionCallback { get; set; }
        public bool ActivarFiltro { get; set; }
        public bool VisualizarAutofiltro { get; set; }
        public string FunctionPopupCallback { get; set; }
        public bool PreCargarDatos { get; set; }

        public ParametrosBusqueda()
        {
            this.Filtros = new List<CampoFiltrar>();
            this.Resultados = new List<CampoResultado>();
            //this.TablaBusqueda = new BusquedaAvanzadaeTablasBusqueda();
            this.TablaBusqueda = new Object();
            this.Token = String.Empty;
            this.FiltroPersonalizado = new List<FiltroPersonalizado>();
            this.BuscaModulo = new ModuloBusqueda();
            this.ActivarFiltro = false;
            this.VisualizarAutofiltro = false;
            this.PreCargarDatos = false;
        }


        public String ObtieneParametros(Boolean Ligar)
        {
            String parametros = String.Empty;
            try
            {

                //parametros = String.Format("{0}CamposFiltro={1}&CamposResultado={2}&TablaBusqueda={3}&Token={4}&FiltroPersonalizado={5}&Modulo={6}&functionCallback={7}",
                //                            Ligar ? "&" : "",
                //                            Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(this.Filtros))),
                //                            Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(this.Resultados))),
                //                            Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(this.TablaBusqueda))),
                //                            Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(this.Token))),
                //                            Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(this.FiltroPersonalizado))),
                //                            Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(this.BuscaModulo))),
                //                            Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(this.functionCallback)))
                //                          );


                SessionHelperBusqueda.Parametros_de_busqueda = this;

                return parametros;
            }
            catch (Exception)
            {

                throw;
            }
        }


        /// <summary>
        /// Metodo para mostrar el cuadro de dialogo de busqueda avanzada
        /// </summary>
        /// <param name="control">Control updatepanel donde se encuentran los controles</param>
        /// <param name="alto">Alto del cuadro de dialogo de la forma 75%</param>
        /// <param name="ancho">Ancho del cuadro de dialogo de la forma 60%</param>
        /// <param name="titulo">Titulo de la ventana de busqueda</param>
        /// <param name="script_name">Nombre del identificador del script</param>
        /// <param name="tipo_busqueda">Tipo de busqueda que va a realizar, para mostrar el cuadro de dialogo especificado</param>
        public void MostrarBusqueda(System.Web.UI.Control control, string alto, string ancho, string titulo, string script_name, TipoBusqueda tipo_busqueda)
        {
            try
            {
                this.Mostrar(control, alto, ancho, titulo, script_name, tipo_busqueda);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Metodo para mostrar el cuadro de dialogo de busqueda avanzada
        /// </summary>
        /// <param name="control">Control updatepanel donde se encuentran los controles</param>
        /// <param name="alto">Alto del cuadro de dialogo de la forma 75</param>
        /// <param name="ancho">Ancho del cuadro de dialogo de la forma 60</param>
        /// <param name="titulo">Titulo de la ventana de busqueda</param>
        /// <param name="script_name">Nombre del identificador del script</param>
        /// <param name="tipo_busqueda">Tipo de busqueda que va a realizar, para mostrar el cuadro de dialogo especificado</param>
        public void MostrarBusqueda(System.Web.UI.Control control, int alto, int ancho, string titulo, string script_name, TipoBusqueda tipo_busqueda)
        {
            try
            {
                this.Mostrar(control, string.Format("{0}%", alto), string.Format("{0}%", ancho), titulo, script_name, tipo_busqueda);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Metodo para mostrar el cuadro de dialogo de busqueda avanzada
        /// </summary>
        /// <param name="pagina">Parametro tipo pagina, si no contiene un updatepanel</param>
        /// <param name="alto">Alto del cuadro de dialogo de la forma 75%</param>
        /// <param name="ancho">Ancho del cuadro de dialogo de la forma 60%</param>
        /// <param name="titulo">Titulo de la ventana de busqueda</param>
        /// <param name="script_name">Nombre del identificador del script</param>
        /// <param name="tipo_busqueda">Tipo de busqueda que va a realizar, para mostrar el cuadro de dialogo especificado</param>
        public void MostrarBusqueda(System.Web.UI.Page pagina, string alto, string ancho, string titulo, string script_name, TipoBusqueda tipo_busqueda)
        {
            try
            {
                this.Mostrar(pagina, alto, ancho, titulo, script_name, tipo_busqueda);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Metodo para mostrar el cuadro de dialogo de busqueda avanzada
        /// </summary>
        /// <param name="pagina">Parametro tipo pagina, si no contiene un updatepanel</param>
        /// <param name="alto">Alto del cuadro de dialogo de la forma 75</param>
        /// <param name="ancho">Ancho del cuadro de dialogo de la forma 60</param>
        /// <param name="titulo">Titulo de la ventana de busqueda</param>
        /// <param name="script_name">Nombre del identificador del script</param>
        /// <param name="tipo_busqueda">Tipo de busqueda que va a realizar, para mostrar el cuadro de dialogo especificado</param>
        public void MostrarBusqueda(System.Web.UI.Page pagina, int alto, int ancho, string titulo, string script_name, TipoBusqueda tipo_busqueda)
        {
            try
            {
                this.Mostrar(pagina, string.Format("{0}%", alto), string.Format("{0}%", ancho), titulo, script_name, tipo_busqueda);
            }
            catch (Exception)
            {
                throw;
            }
        }


        private void Mostrar(dynamic contenedor, string alto, string ancho, string titulo, string script_name, TipoBusqueda tipo_busqueda)
        {
            string script;
            string busqueda;
            try
            {


                busqueda = this.Regresa_Pagina_Busqueda(tipo_busqueda);

                script = String.Format("$(window).MostrarPopup({{ contenedor: null, url: '{0}', alto: '{1}', ancho: '{2}', tituloPopup: \"{3}\", masterPage: \"~/Blank.Master{4}\", movible: false, desaparecer: false {5}}});", busqueda, alto, ancho, titulo, this.ObtieneParametros(true), this.FunctionPopupCallback != string.Empty ? ", FuncionCallback: \"" + this.FunctionPopupCallback + "\"" : string.Empty);

                ScriptManager.RegisterStartupScript(contenedor, contenedor.GetType(), script_name, script, true);
            }
            catch (Exception)
            {
                throw;
            }
        }

        private string Regresa_Pagina_Busqueda(TipoBusqueda tipo_busqueda)
        {
            string respuesta = string.Empty;
            try
            {

                switch (tipo_busqueda)
                {
                    case TipoBusqueda.BUSQUEDA_AVANZADA:
                        //respuesta = "../Controles/Busqueda/Busqueda.aspx";
                        respuesta = HttpContext.Current.Request.Url.AbsoluteUri.Replace(HttpContext.Current.Request.Url.AbsolutePath, "/Controles/Busqueda/Busqueda.aspx");
                        break;
                    case TipoBusqueda.BUSQUEDA_ARTICULO:
                        //respuesta = "../Controles/BusquedaArticulo/BusquedasArticulos.aspx";
                        respuesta = HttpContext.Current.Request.Url.AbsoluteUri.Replace(HttpContext.Current.Request.Url.AbsolutePath, "/Controles/BusquedaArticulo/BusquedasArticulos.aspx");
                        break;
                    case TipoBusqueda.BUSQUEDA_ARTICULO_EXISTENCIA:
                        //respuesta = "../Controles/BusquedaArticuloExistencia/BusquedaArticuloExistencia.aspx";
                        respuesta = HttpContext.Current.Request.Url.AbsoluteUri.Replace(HttpContext.Current.Request.Url.AbsolutePath, "/Controles/BusquedaArticuloExistencia/BusquedaArticuloExistencia.aspx");
                        break;
                    case TipoBusqueda.BUSQUEDA_ARTICULO_NOPARTE:
                        //respuesta = "../Controles/BusquedaArticuloNoParte/BusquedasArticulos.aspx";
                        respuesta = HttpContext.Current.Request.Url.AbsoluteUri.Replace(HttpContext.Current.Request.Url.AbsolutePath, "/Controles/BusquedaArticuloNoParte/BusquedasArticulos.aspx");
                        break;
                    case TipoBusqueda.BUSQUEDA_EMPLEADO:
                        //respuesta = "../Controles/BusquedaEmpleado/BusquedaEmpleado.aspx";
                        respuesta = HttpContext.Current.Request.Url.AbsoluteUri.Replace(HttpContext.Current.Request.Url.AbsolutePath, "/Controles/BusquedaEmpleado/BusquedaEmpleado.aspx");
                        break;
                    case TipoBusqueda.CUENTAS_CONTABLES:
                        //respuesta = "../Controles/CuentasContables/CombinacionesContables.aspx";
                        respuesta = HttpContext.Current.Request.Url.AbsoluteUri.Replace(HttpContext.Current.Request.Url.AbsolutePath, "/Controles/CuentasContables/CombinacionesContables.aspx");
                        break;
                    case TipoBusqueda.SELECCIONAR_CUENTA:
                        //respuesta = "../Controles/CuentasContables/SeleccionarCuenta.aspx";
                        respuesta = HttpContext.Current.Request.Url.AbsoluteUri.Replace(HttpContext.Current.Request.Url.AbsolutePath, "/Controles/CuentasContables/SeleccionarCuenta.aspx");
                        break;

                }


                return respuesta;
            }
            catch (Exception)
            {

                throw;
            }

        }
    }
}