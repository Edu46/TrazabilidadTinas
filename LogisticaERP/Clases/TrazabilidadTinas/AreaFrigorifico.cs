using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LogisticaERP.Clases.TrazabilidadTinas
{
    public class AreaFrigorificoDetalle
    {
        public string idAlmacen { get; set; }
        public string frigorifico { get; set; }
        public string area { get; set; }
        public int numeroLineas { get; set; }
        public int numeroColumnas { get; set; }
    }

    public class AreaFrigorifico
    {
        public AreaFrigorificoDetalle area { get; set; }
    }

    public class ListaAreaFrigorifico
    {
        public List<AreaFrigorificoDetalle> ubicaciones { get; set; }
    }
}