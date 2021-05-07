using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LogisticaERP.Clases.TrazabilidadTinas
{
    public class UbicacionAreaFrigorifico
    {
        public string ubicacion { get; set; }
        public int linea { get; set; }
        public int columna { get; set; }
        public string idAlmacen { get; set; }
        public string idEmpresa { get; set; }
        public string frigorifico { get; set; }
        public int numeroTinasXUbicacion { get; set; }
    }
    
    public class ListaUbicacionAreaFrigorifico
    {
        public List<UbicacionAreaFrigorifico> ubicaciones { get; set; }
    }
}