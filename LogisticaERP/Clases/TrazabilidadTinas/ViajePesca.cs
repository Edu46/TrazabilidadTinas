using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LogisticaERP.Clases.TrazabilidadTinas
{
    public class ViajePesca
    {
        public int idEmbarcacion { get; set; }
        public long idBarco { get; set; }
        public string StringidBarco { get; set; }
        public string barco { get; set; }
        public long idPais { get; set; }
        public string pais { get; set; }
        public int viaje { get; set; }
        public int anio { get; set; }
        public int idSteamer { get; set; }
        public string fechaIni { get; set; }
        public string fechaFin { get; set; }
        public bool activo { get; set; }
    }

    public class ListaViajePesca
    {
        public List<ViajePesca> viajesPesca { get; set; }
    }
}