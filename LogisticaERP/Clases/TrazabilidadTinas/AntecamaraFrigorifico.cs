using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LogisticaERP.Clases.TrazabilidadTinas
{
    public class AntecamaraFrigorifico
    {
        public int idAntecamaraFrigorifico { get; set; }
        public int idAntecamara { get; set; }
        public string descripcionAntecamara { get; set; }
        public long idFrigorifico { get; set; }
        public string StridFrigorifico { get; set; }
        public string descripcionFrigorifico { get; set; }
        public bool activo { get; set; }
        public string usuario { get; set; }
    }

    public class ListaAntecamaraFrigorifico
    {
        public List<AntecamaraFrigorifico> antecamarasFrigorificos { get; set; }
    }
}