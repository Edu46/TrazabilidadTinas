using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LogisticaERP.Clases.TrazabilidadTinas
{
    public class Calidad
    {
        public int idCalidad { get; set; }
        public string nombre { get; set; }
        public string clave { get; set; }
        public bool activo { get; set; }
        public string usuario { get; set; }
    }

    public class ListaCalidad
    {
        public List<Calidad> calidades { get; set; }
    }
}