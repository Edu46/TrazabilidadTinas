using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LogisticaERP.Clases.CalculoTarifaRepresentante.OracleCloud.SOAP
{
    public class PurchaseOrder : SOAPBase
    {
        public PurchaseOrder() { }

        public Tuple<string, string> CrearOrdenCompraCloud(CreatePurchaseOrderReq.Envelope envelope)
        {
            string xml = envelope.SerializeToXML();
            var response = this.Send<Tuple<string, string>>(
                xml,
                SOAPBase.Service.PurchaseOrderService,
                @"http://xmlns.oracle.com/apps/prc/po/editDocument/purchaseOrderServiceV2/createPurchaseOrder");

            return response;
        }
    }
}