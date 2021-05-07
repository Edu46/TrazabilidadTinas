using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LogisticaERP.Clases.TrazabilidadTinas
{
    public class Steamer
    {
        public long idSteamer { get; set; }
        public string StringidSteamer { get; set; }
        public string steamer { get; set; }
        public string claveSteamer { get; set; }
        public long idPropietario { get; set; }
        public string nombreComercial { get; set; }
    }

    public class ListaSteamer
    {
        public List<Steamer> steamers { get; set; }
    }
}