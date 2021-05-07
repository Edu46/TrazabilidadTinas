using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace LogisticaERP.Controles.BusquedaGenerica
{
    [Serializable]
    internal sealed class ParametroGridView
    {
        public ParametroGridView()
        {
            AlwaysShowPager = true;
            Campos = new List<Campo>();
            DataSource = null;
            FunctionCallback = null;
            HeaderHorizontalAlign = HorizontalAlign.Center;
            NumericButtonCount = 5;
            PageSize = 15;
            ShowSelectCheckbox = false;
        }

        public const string _sessionBusquedaGenerica = "ERP>Controles>BusquedaGenerica>Parametro";

        public bool AlwaysShowPager { get; set; }
        public List<Campo> Campos { get; set; }
        public object DataSource { get; set; }
        public string FunctionCallback { get; set; }
        public HorizontalAlign HeaderHorizontalAlign { get; set; }
        public int NumericButtonCount { get; set; }
        public int PageSize { get; set; }
        public bool ShowSelectCheckbox { get; set; }

        public void EstablecerParametros()
        {
            HttpContext.Current.Session[_sessionBusquedaGenerica] = this;
        }

        /// <summary>
        /// Metodo para mostrar el cuadro de dialogo de busqueda avanzada
        /// </summary>
        /// <param name="control">Control updatepanel donde se encuentran los controles</param>
        /// <param name="alto">Alto del cuadro de dialogo de la forma 75%</param>
        /// <param name="ancho">Ancho del cuadro de dialogo de la forma 60%</param>
        /// <param name="titulo">Titulo de la ventana de busqueda</param>
        /// <param name="scriptName">Nombre del identificador del script</param>
        public void MostrarBusqueda(System.Web.UI.Control control, string alto, string ancho, string titulo, string scriptName)
        {
            try
            {
                this.Mostrar(control, alto, ancho, titulo, scriptName);
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        /// <summary>
        /// Metodo para mostrar el cuadro de dialogo de busqueda avanzada
        /// </summary>
        /// <param name="control">Control updatepanel donde se encuentran los controles</param>
        /// <param name="alto">Alto en % del cuadro de dialogo de la forma</param>
        /// <param name="ancho">Ancho en % del cuadro de dialogo de la forma</param>
        /// <param name="titulo">Titulo de la ventana de busqueda</param>
        /// <param name="scriptName">Nombre del identificador del script</param>
        public void MostrarBusqueda(System.Web.UI.Control control, int alto, int ancho, string titulo, string scriptName)
        {
            try
            {
                this.Mostrar(control, string.Format("{0}%", alto), string.Format("{0}%", ancho), titulo, scriptName);
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        /// <summary>
        /// Metodo para mostrar el cuadro de dialogo de busqueda avanzada
        /// </summary>
        /// <param name="pagina">Parametro tipo pagina, si no contiene un updatepanel</param>
        /// <param name="alto">Alto del cuadro de dialogo de la forma 75%</param>
        /// <param name="ancho">Ancho del cuadro de dialogo de la forma 60%</param>
        /// <param name="titulo">Titulo de la ventana de busqueda</param>
        /// <param name="scriptName">Nombre del identificador del script</param>
        /// <param name="tipo_busqueda">Tipo de busqueda que va a realizar, para mostrar el cuadro de dialogo especificado</param>
        public void MostrarBusqueda(System.Web.UI.Page pagina, string alto, string ancho, string titulo, string scriptName)
        {
            try
            {
                this.Mostrar(pagina, alto, ancho, titulo, scriptName);
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        /// <summary>
        /// Metodo para mostrar el cuadro de dialogo de busqueda avanzada
        /// </summary>
        /// <param name="pagina">Parametro tipo pagina, si no contiene un updatepanel</param>
        /// <param name="alto">Alto del cuadro de dialogo de la forma 75</param>
        /// <param name="ancho">Ancho del cuadro de dialogo de la forma 60</param>
        /// <param name="titulo">Titulo de la ventana de busqueda</param>
        /// <param name="scriptName">Nombre del identificador del script</param>
        /// <param name="tipo_busqueda">Tipo de busqueda que va a realizar, para mostrar el cuadro de dialogo especificado</param>
        public void MostrarBusqueda(System.Web.UI.Page pagina, int alto, int ancho, string titulo, string scriptName)
        {
            try
            {
                this.Mostrar(pagina, string.Format("{0}%", alto), string.Format("{0}%", ancho), titulo, scriptName);
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        private void Mostrar(dynamic contenedor, string alto, string ancho, string titulo, string scriptName)
        {
            try
            {
                EstablecerParametros();
                string busqueda = HttpContext.Current.Request.Url.AbsoluteUri.Replace(HttpContext.Current.Request.Url.AbsolutePath, "/Controles/BusquedaGenerica/Busqueda.aspx");
                string script = String.Format("$(window).MostrarPopup({{ contenedor: null, url: '{0}', alto: '{1}', ancho: '{2}', tituloPopup: '{3}', masterPage: '~/Blank.Master', movible: false, desaparecer: false }});", busqueda, alto, ancho, titulo);
                ScriptManager.RegisterStartupScript(contenedor, contenedor.GetType(), scriptName, script, true);
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }
    }
}