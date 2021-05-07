using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LogisticaERP.Clases.TrazabilidadTinas
{
    public class CertificacionDetalle
    {
        public int idCertificacion { get; set; }
        public string certificacion { get; set; }
        public string descripcion { get; set; }
        public string activo { get; set; }
        public string usuario { get; set; }
    }


    public class ListaCertificacion
    {
        public List<CertificacionDetalle> certificaciones { get; set; }
    }
}