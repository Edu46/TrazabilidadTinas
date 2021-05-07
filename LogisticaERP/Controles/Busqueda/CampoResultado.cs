using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GRUPOPINSA.Controles.Busqueda
{
    /// <summary>
    /// Clase para indicar los campos que se mostraran como resultado en el grid
    /// </summary>
    [Serializable]
    public class CampoResultado
    {
        /// <summary>
        /// Titulo del campo que se mostrara en el encabezado de la columna
        /// </summary>
        public string Titulo { get; set; }
        /// <summary>
        /// Nombre del campo al cual se hace referencia en la tabla
        /// </summary>
        public string NombreCampo { get; set; }
        /// <summary>
        /// Indica si este campo se regresara como respuesta cuando se seleccione el registro
        /// </summary>
        public bool Respuesta { get; set; }
        /// <summary>
        /// Indica si este campo estara visible en el resultado
        /// </summary>
        public bool Visible { get; set; }

         /// <summary>
        /// Indica el tipo del campo resultado para su visualizacion
        /// </summary>
        public System.Type Tipo { get; set; }

        /// <summary>
        /// Indica el formato de como se muestra la informacion en la columna
        /// </summary>
        public string DisplayFormatString { get; set; }

        public CampoResultado()
        {
            this.Titulo = string.Empty;
            this.NombreCampo = string.Empty;
            this.Respuesta = false;
            this.Visible = false;
            this.DisplayFormatString = string.Empty;
        }

        /// <summary>
        /// Inicializa la clase para indicar los campos que se mostraran como resultado en el grid
        /// </summary>
        /// <param name="Titulo">Titulo del campo que se mostrara en el encabezado de la columna</param>
        /// <param name="NombreCampo">Nombre del campo al cual se hace referencia en la tabla</param>
        /// <param name="Respuesta">Indica si este campo se regresara como respuesta cuando se seleccione el registro</param>
        /// <param name="Visible">Indica si este campo estara visible en el resultado</param>
        public CampoResultado(String Titulo, String NombreCampo, Boolean Respuesta, Boolean Visible)
        {
            this.Titulo = Titulo;
            this.NombreCampo = NombreCampo;
            this.Respuesta = Respuesta;
            this.Visible = Visible;
            this.Tipo = typeof(string);
            this.DisplayFormatString = string.Empty;
        }

        /// <summary>
        /// Inicializa la clase para indicar los campos que se mostraran como resultado en el grid
        /// </summary>
        /// <param name="Titulo">Titulo del campo que se mostrara en el encabezado de la columna</param>
        /// <param name="NombreCampo">Nombre del campo al cual se hace referencia en la tabla</param>
        /// <param name="Respuesta">Indica si este campo se regresara como respuesta cuando se seleccione el registro</param>
        /// <param name="Visible">Indica si este campo estara visible en el resultado</param>
        /// <param name="Tipo">Indica el tipo del resultado para su visualizacion</param>
        public CampoResultado(String Titulo, String NombreCampo, Boolean Respuesta, Boolean Visible, System.Type Tipo) 
            : this(Titulo,NombreCampo,Respuesta,Visible) 
        {
            //this.Titulo = Titulo;
            //this.NombreCampo = NombreCampo;
            //this.Respuesta = Respuesta;
            //this.Visible = Visible;
            this.Tipo = Tipo;

        }

        /// <summary>
        /// Inicializa la clase para indicar los campos que se mostraran como resultado en el grid
        /// </summary>
        /// <param name="Titulo">Titulo del campo que se mostrara en el encabezado de la columna</param>
        /// <param name="NombreCampo">Nombre del campo al cual se hace referencia en la tabla</param>
        /// <param name="Respuesta">Indica si este campo se regresara como respuesta cuando se seleccione el registro</param>
        /// <param name="Visible">Indica si este campo estara visible en el resultado</param>
        /// <param name="DisplayFormatString">Indica el formato de como se muestra la informacion en la columna</param>
        public CampoResultado(String Titulo, String NombreCampo, Boolean Respuesta, Boolean Visible, string DisplayFormatString)
            : this(Titulo, NombreCampo, Respuesta, Visible) 
        {
            //this.Titulo = Titulo;
            //this.NombreCampo = NombreCampo;
            //this.Respuesta = Respuesta;
            //this.Visible = Visible;
            //this.Tipo = typeof(string);
            this.DisplayFormatString = DisplayFormatString;
        }

        public CampoResultado(String Titulo, String NombreCampo, Boolean Respuesta, Boolean Visible, System.Type Tipo, string DisplayFormatString)
            : this(Titulo, NombreCampo, Respuesta, Visible)
        {
            //this.Titulo = Titulo;
            //this.NombreCampo = NombreCampo;
            //this.Respuesta = Respuesta;
            //this.Visible = Visible;
            this.Tipo = Tipo;
            this.DisplayFormatString = DisplayFormatString;
 
        }



    }
}