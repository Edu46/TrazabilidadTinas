using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LogisticaERP.Clases.TrazabilidadTinas
{
    public class Frigorifico
    {
        public string idAlmacen { get; set; }
        public string idFrigorifico { get; set; }
        public string idEmpresa { get; set; }
        public string clave { get; set; }
        public string descripcion { get; set; }
        public string razonSocial { get; set; }
        public string claveFrigorifico { get; set; }
        public string descripcionFrigorifico { get; set; }
        public bool activo { get; set; }
    }

    public class ListaFrigorifico
    {
        public List<Frigorifico> frigorificosAntecamaras { get; set; }
    }

    public class ListaFrigorificoAntecamara
    {
        public List<Frigorifico> frigorificos { get; set; }
    }
}