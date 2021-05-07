using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LogisticaERP.Clases.TrazabilidadTinas
{
    public class Muelle
    {
        public int idMuelle { get; set; }
        public string descripcion { get; set; }
        public bool activo { get; set; }
    }

    public class ListaMuelle
    {
        public List<Muelle> muelles { get; set; }
    }
}