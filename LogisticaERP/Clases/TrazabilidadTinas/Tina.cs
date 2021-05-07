using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LogisticaERP.Clases.TrazabilidadTinas
{
    public class TinaDetalle
    {
        public string idTina { get; set; }
        public string tina { get; set; }
        public string idEtiquetaTina { get; set; }
        public float taraTina { get; set; }
        public string RSA { get; set; }
        public string idBarco { get; set; }
        public string barco { get; set; }
        public string claveBarco { get; set; }
        public string clave { get; set; }
        public string nombreCorto { get; set; }
        public string idViaje { get; set; }
        public string viaje { get; set; }
        public string ejercicio { get; set; }
        public string claveEjercicio { get; set; }
        public string claveVA { get; set; }
        public string talla { get; set; }
        public string desviacionCalidad { get; set; }
        public string calidad { get; set; }
        public string idCATViajeTanque { get; set; }
        public string tipoCalidad { get; set; }
        public bool sensorial { get; set; }
        public int nivelMuestreo { get; set; }
        public bool tinaConSensor { get; set; }
        public string idTanque { get; set; }
        public string tanque { get; set; }
        public float temperatura { get; set; }
        public string idAlmacen { get; set; }
        public int idEtiquetaMontacargas { get; set; }
        public string posicionExterna { get; set; }
        public string area { get; set; }
        public string linea { get; set; }
        public string columna { get; set; }
        public float pesoTotal { get; set; }
        public int bascula { get; set; }
        public int idTipoEntrada { get; set; }
        public string idProducto { get; set; }
        public string idProblema { get; set; }
        public string claveCalidad { get; set; }
        public string nombreCalidad { get; set; }
        public bool muestreo { get; set; }
        public string nivel { get; set; }
        public string idCalidad { get; set; }
        public bool mediaTina { get; set; }
        public int idMontacargas { get; set; }
        public string idMontacargasSAI { get; set; }
        public string montacargas { get; set; }
        public float taraMontacargas { get; set; }
        public string fechaTaraMontacargas { get; set; }
        public string fecha { get; set; }
        public string usuario { get; set; }
        public List<Certificacion> certificaciones { get; set; }
    }

    public class Certificacion
    {
        public string certificacion { get; set; }
    }

    public class Tina
    {
        public TinaDetalle tinas { get; set; }
    }

    public class ListaTina
    {
        public List<TinaDetalle> detalleUbicacion { get; set; }
    }
}