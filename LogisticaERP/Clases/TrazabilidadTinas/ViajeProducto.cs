using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LogisticaERP.Clases.TrazabilidadTinas
{
    public class ViajeProducto
    {
        public string idViajeProducto { get; set; }
        public string idViaje { get; set; }
        public string idProducto { get; set; }
        public int cantidad { get; set; }
        public int precio { get; set; }
        public string descripcionProducto { get; set; }
        public int ejercicio { get; set; }
        public bool activo { get; set; }
        public string usuario { get; set; }
        public bool borrado { get; set; }

    }

    public class ListaViajeProducto
    {
        public List<ViajeProducto> viajesProductos { get; set; }
    }
}