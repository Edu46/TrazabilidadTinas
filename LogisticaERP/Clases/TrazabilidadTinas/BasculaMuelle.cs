using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LogisticaERP.Clases.TrazabilidadTinas
{
    public class BasculaMuelle
    {
        public int idBasculaMuelle { get; set; }
        public string descripcionBasculaMuelle { get; set; }
        public int idBascula { get; set; }
        public string descripcionBascula { get; set; }
        public int idMuelle { get; set; }
        public string descripcionMuelle { get; set; }
        public bool activo { get; set; }
        public string usuario { get; set; }
    }

    public class ListaBasculaMuelle
    {
        public List<BasculaMuelle> basculasMuelles { get; set; }
    }
}