using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LogisticaERP.Clases.TrazabilidadTinas
{
    public class Viaje
    {
        public long idBarco { get; set; }
        public string StringidBarco { get; set; }
        public string nombreBarco { get; set; }
        public long idViaje { get; set; }
        public string StringidViaje { get; set; }
        public string viaje { get; set; }
        public int ejercicio { get; set; }
        public int RSA { get; set; }
        public string fechaArribo { get; set; }
        public string fechaSalida { get; set; }
        public string fechaDescarga { get; set; }
        public long idTinaInicio { get; set; }
        public long idTinaFin { get; set; }
        public string numeroViaje { get; set; }
    }

    public class SiguienteViajeDetalle
    {
        public string idBarco { get; set; }
        public string ejercicio { get; set; }
        public string numeroViaje { get; set; }
    }

    public class ListaViaje
    {
        public List<Viaje> viajes { get; set; }
    }

    public class SiguienteViaje
    {
        public SiguienteViajeDetalle sigViaje { get; set; }
    }
}