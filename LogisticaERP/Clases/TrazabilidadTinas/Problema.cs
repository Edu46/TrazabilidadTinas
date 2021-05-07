using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LogisticaERP.Clases.TrazabilidadTinas
{
    public class Problema
    {
        public ulong idProblema { get; set; }
        public string idProblemaString { get; set; }
        public string problema { get; set; }
        public string clave { get; set; }
        public string descripcion { get; set; }
        public bool restriccion { get; set; }
        public string calidadERP { get; set; }
        public string claveProblemaOracle { get; set; }
        public bool activo { get; set; }
        public string usuario { get; set; }
    } 

    public class ListaProblema
    {
        public List<Problema> problemas { get; set; }
    }
}