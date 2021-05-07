using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LogisticaERP.Clases.TrazabilidadTinas
{
    public class TanqueCertificacion
    {
        public int idTanqueCertificacion { get; set; }
        public string idTanque { get; set; }
        public string descripcionTanque { get; set; }
        public long idBarco { get; set; }
        public long idViaje { get; set; }
        public List<CertificacionTanqueLista> certificaciones { get; set; }
        public string listaCertificaciones { get; set; }
        public string listaIdCertificaciones { get; set; }
        public int idCertificaciones { get; set; }
        public int dondeEstoy { get; set; }
        public bool objFlot { get; set; }
        public bool activo { get; set; }
        public bool certificacionActiva { get; set; }
        public bool dolphinSave { get; set; }
        public bool bloqueo { get; set; }
        public bool borrado { get; set; }
        public string usuario { get; set; }
        public string[] idCertificacion { get; set; }
    }

    public class CertificacionTanqueLista
    {
        public string certificacion { get; set; }
        public int idCertificacion { get; set; }
    }

    public class ListaTanqueCertificacion
    {
        public List<TanqueCertificacion> tanquesCertificaciones { get; set; }
    }

}