using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LogisticaERP.Clases.TrazabilidadTinas
{
    public class Antecamara
    {
        public int idAntecamara { get; set; }
        public string nombreAntecamara { get; set; }
        public string descripcionAntecamara { get; set; }
        public bool activo { get; set; }
    }

    public class ListaAntecamara
    {
        public List<Antecamara> antecamaras { get; set; }
    }
}