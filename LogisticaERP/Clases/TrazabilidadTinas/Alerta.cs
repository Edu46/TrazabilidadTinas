using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LogisticaERP.Clases.TrazabilidadTinas
{
    public class Alerta
    {
        public int idFrecuenciaAlerta { get; set; }
        public string descripcion { get; set; }
        public int maximaExposicion { get; set; }
        public int rangoAlerta { get; set; }
        public int tolerancia { get; set; }
        public string talla { get; set; }
        public string usuario { get; set; }
        public bool activo { get; set; }
    }

    public class ListaAlerta
    {
        public List<Alerta> alertas { get; set; }
    }
}