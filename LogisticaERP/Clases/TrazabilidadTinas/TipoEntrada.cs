using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LogisticaERP.Clases.TrazabilidadTinas
{
    public class TipoEntrada
    {
        public int idEntrada { get; set; }
        public string descripcionEntrada { get; set; }
        public string tipoEntrada { get; set; }
        public bool activo { get; set; }
        public bool borrado { get; set; }
    }

    public class ListaTipoEntrada
    {
        public List<TipoEntrada> entradas { get; set; }
    }
}