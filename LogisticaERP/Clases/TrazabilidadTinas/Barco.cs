using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LogisticaERP.Clases.TrazabilidadTinas
{
    public class Barco
    {
        public long idBarco { get; set; }
        public string StringIdBarco { get; set; }
        public string barco { get; set; }
        public string clave { get; set; }
        public string nombreComercial { get; set; }
        public long idEmpresa { get; set; }
        public string StringidEmpresa { get; set; }
    }

    public class ListaBarco
    {
        public List<Barco> catalogoBarcos { get; set; }
    }
}