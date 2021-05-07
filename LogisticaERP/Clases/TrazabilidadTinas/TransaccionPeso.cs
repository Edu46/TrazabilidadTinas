using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LogisticaERP.Clases.TrazabilidadTinas
{
    public class TransaccionPesoDetalle
    {
        public int idTransaccionBascula { get; set; }
        public int idTransaccionSIP { get; set; }
        public int idBascula { get; set; }
        public float peso { get; set; }
        public string fechaTransaccion { get; set; }
        public bool coincide { get; set; }
    }

    public class TransaccionPeso
    {
        public TransaccionPesoDetalle transaccion { get; set; }
    }
}