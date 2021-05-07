using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LogisticaERP.Clases.TrazabilidadTinas
{
    public class Zona
    {
        public int idZonaDescarga { get; set; }
        public string descripcion { get; set; }
        public bool activo { get; set; }
        public string usuario { get; set; }
    }

    public class ListaZona
    {
        public List<Zona> zonasDescarga { get; set; }
    }
}