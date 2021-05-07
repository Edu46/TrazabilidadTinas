using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LogisticaERP.Clases.TrazabilidadTinas
{
    public class AntecamaraBascula
    {
        public long idAntecamaraBascula { get; set; }
        public int idAntecamara { get; set; }
        public string descripcionAntecamara { get; set; }
        public string idBascula { get; set; }
        public string descripcionBascula { get; set; }
        public bool activo { get; set; }
        public string usuario { get; set; }
    }

    public class ListaAntecamaraBascula
    {
        public List<AntecamaraBascula> antecamarasBasculas { get; set; }
    }
}