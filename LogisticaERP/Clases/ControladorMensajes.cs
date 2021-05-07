using System;
using System.Web;
using System.Linq;
using System.Text;
using System.Web.UI;
using System.ComponentModel;
using System.Collections.Generic;

namespace LogisticaERP.Clases
{
    /// <summary>
    /// Clase controladora de enviar mensajes al cliente
    /// </summary>
    public sealed class ControladorMensajes
    {
        private const string FUNCIONMENSAJES_JS = "MostrarCajaMensajes";
        private const string TEXTO_NEGRITAS = "StrongText";
        private const string TEXTO_NORMAL = "NormalText";

        private static List<Tuple<string, string>> lista = new List<Tuple<string, string>>();

        /// <summary>
        /// Tipo de Enumeradores para la caja de mensajes
        /// </summary>
        public enum TipoMensaje
        {
            [Description("SUCCESSBOX")]
            Exito = 1,
            [Description("INFORMATIONBOX")]
            Informativo = 2,
            [Description("WARNINGBOX")]
            Advertencia = 3,
            [Description("ERRORBOX")]
            Error = 4
        }

        /// <summary>
        /// Contructor privado para no inicializar valores
        /// </summary>
        private ControladorMensajes() { }

        /// <summary>
        /// Metodo para mostrar mensaje en el cliente
        /// </summary>
        /// <param name="control">Tipo Web.UI.Control</param>
        /// <param name="identificadorScript">Identificador de script</param>
        /// <param name="tipoMensaje">Enumerador para tipos de mensaje</param>
        /// <param name="strongText">Mensaje en letra negrita</param>
        /// <param name="tiempoMostrar">Tiempo en ms que se mostrará el mensaje, puede ser nulo y el mensaje no se ocultara</param>
        public static void MostrarMensaje(Control control, string identificadorScript, TipoMensaje tipoMensaje, string strongText, int? tiempoMostrar = null)
        {
            lista.Add(new Tuple<string, string>(strongText, null));
            MostrarMensaje(control, identificadorScript, tipoMensaje, lista, tiempoMostrar);
        }

        /// <summary>
        /// Metodo para mostrar mensaje en el cliente
        /// </summary>
        /// <param name="control">Tipo Web.UI.Control</param>
        /// <param name="identificadorScript">Identificador de script</param>
        /// <param name="tipoMensaje">Enumerador para tipos de mensaje</param>
        /// <param name="strongText">Mensaje en letra negrita</param>
        /// <param name="normalText">Mensaje en letra normal</param>
        /// <param name="tiempoMostrar">Tiempo en ms que se mostrará el mensaje, puede ser nulo y el mensaje no se ocultara</param>
        public static void MostrarMensaje(Control control, string identificadorScript, TipoMensaje tipoMensaje, string strongText, string normalText, int? tiempoMostrar = null)
        {
            lista.Add(new Tuple<string, string>(strongText, normalText));
            MostrarMensaje(control, identificadorScript, tipoMensaje, lista, tiempoMostrar);
        }

        /// <summary>
        /// Metodo para mostrar mensaje en el cliente
        /// </summary>
        /// <param name="pagina">Tipo pagina web </param>
        /// <param name="identificadorScript">Identificador de script</param>
        /// <param name="tipoMensaje">Enumerador para tipos de mensaje</param>
        /// <param name="strongText">Mensaje en letra negrita</param>
        /// <param name="tiempoMostrar">Tiempo en ms que se mostrará el mensaje, puede ser nulo y el mensaje no se ocultara</param>
        public static void MostrarMensaje(Page pagina, string identificadorScript, TipoMensaje tipoMensaje, string strongText, int? tiempoMostrar = null)
        {
            lista.Add(new Tuple<string, string>(strongText, null));
            MostrarMensaje(pagina, identificadorScript, tipoMensaje, lista, tiempoMostrar);
        }

        /// <summary>
        /// Metodo para mostrar mensaje en el cliente
        /// </summary>
        /// <param name="pagina">Tipo pagina web</param>
        /// <param name="identificadorScript">Identificador de script</param>
        /// <param name="tipoMensaje">Enumerador para tipos de mensaje</param>
        /// <param name="strongText">Mensaje en letra negrita</param>
        /// <param name="normalText">Mensaje en letra normal</param>
        /// <param name="tiempoMostrar">Tiempo en ms que se mostrará el mensaje, puede ser nulo y el mensaje no se ocultara</param>
        public static void MostrarMensaje(Page pagina, string identificadorScript, TipoMensaje tipoMensaje, string strongText, string normalText, int? tiempoMostrar = null)
        {
            lista.Add(new Tuple<string, string>(strongText, normalText));
            MostrarMensaje(pagina, identificadorScript, tipoMensaje, lista, tiempoMostrar);
        }

        /// <summary>
        /// Metodo para mostrar mensaje en el cliente
        /// </summary>
        /// <param name="control">Tipo Web.UI.Control</param>
        /// <param name="identificadorScript">Identificador de script</param>
        /// <param name="tipoMensaje">Enumerador para tipos de mensaje</param>
        /// <param name="ListaMensajes">Lista de mensajes, el primer valor es strong text, el segundo normal text</param>
        /// <param name="tiempoMostrar">Tiempo en ms que se mostrará el mensaje, puede ser nulo y el mensaje no se ocultara</param>
        public static void MostrarMensaje(Control control, string identificadorScript, TipoMensaje tipoMensaje, List<Tuple<string, string>> ListaMensajes, int? tiempoMostrar = null)
        {
            Mensaje(control, identificadorScript, tipoMensaje, ListaMensajes, tiempoMostrar);
        }

        /// <summary>
        /// Metodo para mostrar mensaje en el cliente
        /// </summary>
        /// <param name="pagina">Tipo pagina web</param>
        /// <param name="identificadorScript">Identificador de script</param>
        /// <param name="tipoMensaje">Enumerador para tipos de mensaje</param>
        /// <param name="ListaMensajes">Lista de mensajes, el primer valor es strong text, el segundo normal text</param>
        /// <param name="tiempoMostrar">Tiempo en ms que se mostrará el mensaje, puede ser nulo y el mensaje no se ocultara</param>
        public static void MostrarMensaje(Page pagina, string identificadorScript, TipoMensaje tipoMensaje, List<Tuple<string, string>> ListaMensajes, int? tiempoMostrar = null)
        {
            Mensaje(pagina, identificadorScript, tipoMensaje, ListaMensajes, tiempoMostrar);
        }

        /// <summary>
        /// Metodo privado para armar y mostrar el mensaje
        /// </summary>
        /// <param name="contenedor">contenedor puede ser pagina o control</param>
        /// <param name="identificadorScript">Identificador de script</param>
        /// <param name="tipoMensaje">Enumerador para tipos de mensaje</param>
        /// <param name="ListaMensajes">Lista de mensajes, el primer valor es strong text, el segundo normal text</param>
        /// <param name="tiempoMostrar">Tiempo en ms que se mostrará el mensaje, puede ser nulo y el mensaje no se ocultara</param>
        private static void Mensaje(dynamic contenedor, string identificadorScript, TipoMensaje tipoMensaje, List<Tuple<string, string>> ListaMensajes, int? tiempoMostrar = null)
        {
            try
            {
                string mensaje = CrearScriptMensaje(tipoMensaje, ListaMensajes, tiempoMostrar);
                ScriptManager.RegisterStartupScript(contenedor, contenedor.GetType(), identificadorScript, mensaje, false);
            }
            catch (Exception) { }

            lista.Clear();
        }

        /// <summary>
        /// Metodo que forma el mensaje en javascript
        /// </summary>
        /// <param name="tipoMensaje">Enumerador del tipo de mensaje</param>
        /// <param name="ListaMensajes">Lista de mensajes, el primer valor es strong text, el segundo normal text</param>
        /// <param name="tiempoMostrar">Tiempo en ms que se mostrará el mensaje, puede ser nulo y el mensaje no se ocultara</param>
        /// <returns>mensaje en javascript</returns>
        private static string CrearScriptMensaje(TipoMensaje tipoMensaje, List<Tuple<string, string>> listaMensajes, int? tiempoMostrar = null)
        {
            StringBuilder constructor = new StringBuilder();
            int cantidadMensajes = listaMensajes.Count();

            if (listaMensajes.Count > 0)
            {
                constructor.Append(string.Format("<script type='text/javascript'>{0}(", FUNCIONMENSAJES_JS)).Append(tipoMensaje.Descripcion()).Append(", [");

                listaMensajes.ForEach(e =>
                {
                    constructor.Append("{ ");

                    if (!string.IsNullOrEmpty(e.Item1))
                    {
                        constructor.Append(string.Format("'{0}': '", TEXTO_NEGRITAS)).Append(e.Item1.Replace("'", "\"").Replace("\r", " ").Replace("\n", " ")).Append("'");
                    }

                    if (!string.IsNullOrEmpty(e.Item2))
                    {
                        constructor.Append(string.IsNullOrEmpty(e.Item1) ? string.Empty : ",").Append(string.Format(" '{0}': '", TEXTO_NORMAL)).Append(e.Item2.Replace("'", "\"").Replace("\r", " ").Replace("\n", " ")).Append("'");
                    }

                    constructor.Append(" },");
                });

                if (constructor[constructor.Length - 1] == ',')
                {
                    constructor[constructor.Length - 1] = ' ';
                }

                constructor.Append("]").Append(tiempoMostrar.HasValue ? ", " + tiempoMostrar.ToString() : string.Empty).Append("); </script>");
            }

            return constructor.ToString();
        }
    }
}