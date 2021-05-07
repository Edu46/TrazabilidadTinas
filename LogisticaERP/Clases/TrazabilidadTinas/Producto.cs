using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LogisticaERP.Clases.TrazabilidadTinas
{
    public class Producto
    {
        public string idProducto { get; set; }
        public string StringidProducto { get; set; }
        public string clave { get; set; }
        public int claveRapida { get; set; }
        public string producto { get; set; }
        public int claveERP { get; set; }
        public string identificadorProd { get; set; }
        public long idTallas { get; set; }
        public string StringidTallas { get; set; }
        public string talla { get; set; }
        public long idVariedad { get; set; }
        public string StringidVariedad { get; set; }
        public string variedad { get; set; }
        public string claveVariedad { get; set; }
        public string idViaje { get; set; }
        public string idViajeProducto { get; set; }
        public string ejercicio { get; set; }
        public long cantidad { get; set; }
        public bool activo { get; set; }
        public bool borrado { get; set; }
        public string usuario { get; set; }

    }

    public class ListaProductos
    {
        public List<Producto> catalogoProductos { get; set; }
    }
}