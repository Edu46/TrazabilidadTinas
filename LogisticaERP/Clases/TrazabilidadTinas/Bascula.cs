using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LogisticaERP.Clases.TrazabilidadTinas
{
    public class Bascula
    {
        public string idBascula { get; set; }
        public string descripcion { get; set; }
        public bool activo { get; set; }
        public string usuario { get; set; }
    }

    public class ListaBascula
    {
        public List<Bascula> basculas { get; set; }
    }
}