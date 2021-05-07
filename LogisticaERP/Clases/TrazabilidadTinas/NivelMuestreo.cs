using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LogisticaERP.Clases.TrazabilidadTinas
{
    public class NivelMuestreo
    {
        public int idNivelMuestreo { get; set; }
        public string descripcion { get; set; }
        public bool activo { get; set; }
        public string usuario { get; set; }
    }

    public class ListaNivelMuestreo
    {
        public List<NivelMuestreo> nivelesMuestreo { get; set; }
    }
}