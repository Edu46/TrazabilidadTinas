using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LogisticaERP.Clases.TrazabilidadTinas
{
    public class TaraMontacargaDetalle
    {
        public int idMontacargas { get; set; }
        public long idEtiquetaMontacargas { get; set; }
        public string idMontacargasSAI { get; set; }
        public string montacargasDescripcion { get; set; }
        public int enBascula { get; set; }
        public int enAlerta { get; set; }
        public string descripcionTipoMontacargas { get; set; }
        public string tara { get; set; }
        public string fechaTara { get; set; }
        public string fechaActual { get; set; }
        public int idTipoMontacargas { get; set; }
        public string descripcion { get; set; }
        public int tiempoMaximo { get; set; }
        public int tolerancia { get; set; }
        public string exposicionTara { get; set; }
        public string tiempoRestante { get; set; }
        public float tiempoRestanteCalculado { get; set; }
        public float tiempo { get; set; }
        public string fechaAlerta { get; set; }
        public string usuario { get; set; }
        public bool activo { get; set; }
        public bool borrado { get; set; }
    }

    public class ActualizarAlerta
    {
        public int idTipoMontacargas { get; set; }
        public int tiempoMaximo { get; set; }
        public int tolerancia { get; set; }
        public string usuario { get; set; }
    }

    public class ActualizarTara
    {
        public long idEtiquetaMontacargas { get; set; }
        public long idMontacargas { get; set; }
        public string tara { get; set; }
        public string usuario { get; set; }
    }

    public class ListaTaraMontacarga
    {
        public List<TaraMontacargaDetalle> montacargasAlertas { get; set; }
    }

    public class ListaTipoMontacarga
    {
        public List<TaraMontacargaDetalle> tiposMontacargas { get; set; }
    }

}