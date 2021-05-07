using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LogisticaERP.Clases.TrazabilidadTinas
{
    public class ViajeBarco
    {
        public string usuario { get; set; }
        public string mensaje { get; set; }
        public long idBarco { get; set; }
        public string StringidBarco { get; set; }
        public string barco { get; set; }
        public string claveBarco { get; set; }
        public long idViaje { get; set; }
        public string viaje { get; set; }
        public string StringidViaje { get; set; }
        public string descripcionViaje { get; set; }
        public int ejercicio { get; set; }
        public string idSteamer { get; set; }
        public string steamer { get; set; }
        public string claveSteamer { get; set; }
        public string propietarioSteamer { get; set; }
        public string folioIdentificador { get; set; }
        public string fechaDescarga { get; set; }
        public string fechaArribo { get; set; }
        public string fechaSalida { get; set; }
        public int viajeEmbarcacion { get; set; }
        public int viajePesca { get; set; }
        public string RSA { get; set; }
        public int idFolio { get; set; }
        public int idEmbarcacion { get; set; }
        //public string restriccion { get; set; }
        public bool descargado { get; set; }
        public bool descargaTinas { get; set; }
        public bool subviaje { get; set; }
        public bool activo { get; set; }
    }

    public class EstatusBarcoViaje
    {
        public string idBarco { get; set; }
        public string idViaje { get; set; }
        public string usuario { get; set; }
    }

    public class ListaBarcoViaje
    {
        public List<ViajeBarco> barcosViajes { get; set; }
    }
}