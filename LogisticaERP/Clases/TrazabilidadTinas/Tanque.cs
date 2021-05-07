using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LogisticaERP.Clases.TrazabilidadTinas
{
    public class Tanque
    {
        public long idTanque { get; set; }
        public string StingidTanque { get; set; }
        public long idBarco { get; set; }
        public string StringidBarco { get; set; }
        public string tanque { get; set; }
        public bool bloqueo { get; set; }
    }

    public class ListaTanque
    {
        public List<Tanque> tanques { get; set; }
    }
}