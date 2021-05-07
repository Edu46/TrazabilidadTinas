using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LogisticaERP.Clases.TrazabilidadTinas
{
    public class UbicacionExterna
    {
        public int idUbicacionExterna { get; set; }
        public string ubicacion { get; set; }
        public string descripcion { get; set; }
        public bool activo { get; set; }
        public bool borrado { get; set; }
        public string fechaCreacion { get; set; }
        public string usuarioCreacion { get; set; }
        public string fechaModificacion { get; set; }
        public string usuarioModificacion { get; set; }
    }

    public class ListaUbicacionExterna
    {
        public List<UbicacionExterna> entradas { get; set; }
    }
}