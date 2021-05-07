using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LogisticaERP.Clases.RecepcionarASN.OracleCloud.DTO
{
    [Serializable]
    public class PickTransaction
    {
        public List<PickLines> pickLines { get; set; }

        public PickTransaction()
        {
            pickLines = new List<PickLines>();
        }
    }

    [Serializable]
    public class PickLines
    {
        public long PickSlip { get; set; }
        public long PickSlipLine { get; set; }
        public decimal PickedQuantity { get; set; }
        public string SubinventoryCode { get; set; }
        public List<LotItemLots> lotItemLots { get; set; }

        public PickLines()
        {
            PickSlip = 0;
            PickSlipLine = 0;
            PickedQuantity = 0;
            SubinventoryCode = string.Empty;
            lotItemLots = new List<LotItemLots>();
        }
    }

    [Serializable]
    public class LotItemLots
    {
        public string Lot { get; set; }
        public decimal Quantity { get; set; }

        public LotItemLots()
        {
            Lot = string.Empty;
            Quantity = 0;
        }
    }
}