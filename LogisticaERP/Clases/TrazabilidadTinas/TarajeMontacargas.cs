using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LogisticaERP.Clases.TrazabilidadTinas
{
    public class TarajeMontacargasDetalle
    {
        public int idMontacargas { get; set; }
        public string idMontacargasSAI { get; set; }
        public string montacargas { get; set; }
        public float tiempoMaximo { get; set; }
        public float tolerancia { get; set; }
        public float tiempoRestante { get; set; }
        public int idEtiquetaMontacargas { get; set; }
    }

    public class TarajeMontacargas
    {
        public TarajeMontacargasDetalle taraje { get; set; }
    }
}