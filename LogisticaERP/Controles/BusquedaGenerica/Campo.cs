using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;

namespace LogisticaERP.Controles.BusquedaGenerica
{
    /// <summary>
    /// Clase para indicar los campos que se mostraran como resultado en el grid
    /// </summary>
    [Serializable]
    internal sealed class Campo
    {
        /// <summary>
        /// Inicializa la clase para indicar los campos que se mostraran como resultado en el grid
        /// </summary>
        /// <param name="fieldName">Nombre del campo de basede de datos asignado a la columna actual</param>
        /// <param name="isKeydFieldName">Nombre del campo de clave de origen de datos</param>
        public Campo(string fieldName, bool isKeydFieldName)
        {
            FieldName = fieldName;
            IsKeyFieldName = isKeydFieldName;
        }

        /// <summary>
        /// Inicializa la clase para indicar los campos que se mostraran como resultado en el grid
        /// </summary>
        /// <param name="caption">Texto que se muestra dentro del encabezado de columna</param>
        /// <param name="fieldName">Nombre del campo de basede de datos asignado a la columna actual</param>
        /// <param name="width">Ancho de la columna</param>
        /// <param name="dataType">Tipo de dato de la columna actual</param>
        public Campo(string caption, string fieldName, int width, Type dataType, HorizontalAlign cellHorizontalAlign)
        {
            Caption = caption;
            FieldName = fieldName;
            Width = width;
            DataType = dataType;
            CellHorizontalAlign = cellHorizontalAlign;
        }

        /// <summary>
        /// Inicializa la clase para indicar los campos que se mostraran como resultado en el grid
        /// </summary>
        /// <param name="caption">Texto que se muestra dentro del encabezado de columna</param>
        /// <param name="fieldName">Nombre del campo de basede de datos asignado a la columna actual</param>
        /// <param name="width">Ancho de la columna</param>
        /// <param name="dataType">Tipo de dato de la columna actual</param>
        /// /// <param name="displayFormatString">Patrón utilizado para formatear el valor del editor para fines de visualización</param>
        public Campo(string caption, string fieldName, int width, Type dataType, HorizontalAlign cellHorizontalAlign, string displayFormatString)
        {
            Caption = caption;
            FieldName = fieldName;
            Width = width;
            DataType = dataType;
            CellHorizontalAlign = cellHorizontalAlign;
            DisplayFormatString = displayFormatString;
        }

        /// <summary>
        /// Inicializa la clase para indicar los campos que se mostraran como resultado en el grid
        /// </summary>
        /// <param name="caption">Texto que se muestra dentro del encabezado de columna</param>
        /// <param name="fieldName">Nombre del campo de basede de datos asignado a la columna actual</param>
        /// <param name="isKeydFieldName">Nombre del campo de clave de origen de datos</param>
        /// <param name="width">Ancho de la columna</param>
        /// <param name="dataType">Tipo de dato de la columna actual</param>
        /// <param name="cellHorizontalAlign">Alineación horizontal de elementos dentro de un contenedor</param>
        public Campo(string caption, string fieldName, bool isKeydFieldName, int width, Type dataType, HorizontalAlign cellHorizontalAlign)
        {
            Caption = caption;
            FieldName = fieldName;
            IsKeyFieldName = isKeydFieldName;
            Width = width;
            DataType = dataType;
            CellHorizontalAlign = cellHorizontalAlign;
        }

        /// <summary>
        /// Inicializa la clase para indicar los campos que se mostraran como resultado en el grid
        /// </summary>
        /// <param name="caption">Texto que se muestra dentro del encabezado de columna</param>
        /// <param name="fieldName">Nombre del campo de basede de datos asignado a la columna actual</param>
        /// <param name="isKeydFieldName">Nombre del campo de clave de origen de datos</param>
        /// <param name="width">Ancho de la columna</param>
        /// <param name="dataType">Tipo de dato de la columna actual</param>
        /// <param name="cellHorizontalAlign">Alineación horizontal de elementos dentro de un contenedor</param>
        /// <param name="displayFormatString">Patrón utilizado para formatear el valor del editor para fines de visualización</param>
        public Campo(string caption, string fieldName, bool isKeydFieldName, int width, Type dataType, HorizontalAlign cellHorizontalAlign, string displayFormatString)
        {
            Caption = caption;
            FieldName = fieldName;
            IsKeyFieldName = isKeydFieldName;
            Width = width;
            DataType = dataType;
            CellHorizontalAlign = cellHorizontalAlign;
            DisplayFormatString = displayFormatString;
        }

        /// <summary>
        /// Inicializa la clase para indicar los campos de tipo boton que se mostraran como resultado en una columna del grid
        /// </summary>
        /// <param name="caption">Texto que se muestra dentro del encabezado de columna</param>
        /// <param name="width">Ancho de la columna</param>
        /// <param name="listaCampoTipoBoton">Lista de botones a desplegar</param>
        /// <param name="clientSideEventsCustomButtonClick">cadena con las funciones javascript que se ejecutaran del lado del cliente. Ejemplo: function(s, e) { if (e.buttonID == 'b1') alert('Hola Mundo'); }</param>
        public Campo(string caption, int width, List<CampoTipoBoton> listaCampoTipoBoton, string clientSideEventsCustomButtonClick)
        {
            Caption = caption;
            Width = width;
            ListaCampoTipoBoton = listaCampoTipoBoton;
            ClientSideEventsCustomButtonClick = clientSideEventsCustomButtonClick;
            DataType = typeof(Button);
        }

        /// <summary>
        /// Obtiene o establece el texto que se muestra dentro del encabezado de columna.
        /// </summary>
        public string Caption { get; set; }
        /// <summary>
        /// Especifica la alineación horizontal de elementos dentro de un contenedor.
        /// </summary>
        public HorizontalAlign CellHorizontalAlign { get; set; }
        /// <summary>
        /// Obtiene o establece el tipo de dato de la columna actual.
        /// </summary>
        public Type DataType { get; set; }
        /// <summary>
        /// Obtiene o establece el patrón utilizado para formatear el valor del editor para fines de visualización.
        /// </summary>
        public string DisplayFormatString { get; set; }
        /// <summary>
        /// Obtiene o establece el nombre del campo de base de datos asignado a la columna actual.
        /// </summary>
        public string FieldName { get; set; }
        /// <summary>
        /// Obtiene o establece el nombre del campo de clave de origen de datos
        /// </summary>
        public bool IsKeyFieldName { get; set; }
        /// <summary>
        /// Obtiene o establece el ancho de la columna.
        /// </summary>
        public int Width { get; set; }
        /// <summary>
        /// Obtiene o establece la lista de botones personalizados dentro de una columna.
        /// </summary>
        public List<CampoTipoBoton> ListaCampoTipoBoton { get; set; }
        /// <summary>
        /// Obtiene o establece una cadena con las funciones javsacript que se ejecutaran del lado del cliente. Ejemplo: function(s, e) { if (e.buttonID == 'b1') alert('Hola Mundo'); }
        /// </summary>
        public string ClientSideEventsCustomButtonClick { get; set; }
    }

    [Serializable]
    internal sealed class CampoTipoBoton
    {
        /// <summary>
        /// Obtiene o establece el indice del boton personalizado.
        /// </summary>
        public int Index { get; set; }
        /// <summary>
        /// Obtiene o establece el id del boton personalizado.
        /// </summary>
        public string Id { get; set; }
        /// <summary>
        /// Obtiene o establece el id de la imagen del boton personalizado.
        /// </summary>
        public string IconId { get; set; }
        /// <summary>
        /// Obtiene o establece el tooltip del boton personalizado.
        /// </summary>
        public string Tooltip { get; set; }
    }
}