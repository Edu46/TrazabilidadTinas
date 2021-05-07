using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Serialization;

namespace LogisticaERP.Clases.CalculoTarifaRepresentante.OracleCloud
{
    public class CreatePurchaseOrderReq
    {
        [XmlRoot(ElementName = "Quantity", Namespace = "http://xmlns.oracle.com/apps/prc/po/editDocument/purchaseOrderServiceV2/")]
        public class Quantity
        {
            public Quantity()
            {
                UnitCode = string.Empty;
                Text = string.Empty;
            }

            [XmlAttribute(AttributeName = "unitCode")]
            public string UnitCode { get; set; }
            [XmlText]
            public string Text { get; set; }
        }

        [XmlRoot(ElementName = "Price", Namespace = "http://xmlns.oracle.com/apps/prc/po/editDocument/purchaseOrderServiceV2/")]
        public class Price
        {
            public Price()
            {
                CurrencyCode = string.Empty;
                Text = string.Empty;
            }

            [XmlAttribute(AttributeName = "currencyCode")]
            public string CurrencyCode { get; set; }
            [XmlText]
            public string Text { get; set; }
        }

        [XmlRoot(ElementName = "PurchaseOrderEntryDistribution", Namespace = "http://xmlns.oracle.com/apps/prc/po/editDocument/purchaseOrderServiceV2/")]
        public class PurchaseOrderEntryDistribution
        {
            public PurchaseOrderEntryDistribution()
            {
                BudgetDate = string.Empty;
                POChargeAccountId = string.Empty;
                RequesterEmail = string.Empty;
            }

            [XmlElement(ElementName = "BudgetDate", Namespace = "http://xmlns.oracle.com/apps/prc/po/editDocument/purchaseOrderServiceV2/")]
            public string BudgetDate { get; set; }
            [XmlElement(ElementName = "POChargeAccountId", Namespace = "http://xmlns.oracle.com/apps/prc/po/editDocument/purchaseOrderServiceV2/")]
            public string POChargeAccountId { get; set; }
            [XmlElement(ElementName = "RequesterEmail", Namespace = "http://xmlns.oracle.com/apps/prc/po/editDocument/purchaseOrderServiceV2/")]
            public string RequesterEmail { get; set; }
        }

        [XmlRoot(ElementName = "PurchaseOrderEntrySchedule", Namespace = "http://xmlns.oracle.com/apps/prc/po/editDocument/purchaseOrderServiceV2/")]
        public class PurchaseOrderEntrySchedule
        {
            public PurchaseOrderEntrySchedule()
            {
                ShipToOrganizationCode = string.Empty;
                ShipToLocationCode = string.Empty;
                PurchaseOrderEntryDistribution = new List<PurchaseOrderEntryDistribution>();
            }

            [XmlElement(ElementName = "ShipToOrganizationCode", Namespace = "http://xmlns.oracle.com/apps/prc/po/editDocument/purchaseOrderServiceV2/")]
            public string ShipToOrganizationCode { get; set; }
            [XmlElement(ElementName = "ShipToLocationCode", Namespace = "http://xmlns.oracle.com/apps/prc/po/editDocument/purchaseOrderServiceV2/")]
            public string ShipToLocationCode { get; set; }
            [XmlElement(ElementName = "PurchaseOrderEntryDistribution", Namespace = "http://xmlns.oracle.com/apps/prc/po/editDocument/purchaseOrderServiceV2/")]
            public List<PurchaseOrderEntryDistribution> PurchaseOrderEntryDistribution { get; set; }
        }

        [XmlRoot(ElementName = "LineFlexfield", Namespace = "http://xmlns.oracle.com/apps/prc/po/editDocument/purchaseOrderServiceV2/")]
        public class LineFlexfield
        {
            public LineFlexfield()
            {
                Clasificacion = string.Empty;
                FlexContext = string.Empty;
                Viaje = string.Empty;
            }

            [XmlElement(ElementName = "clasificacion", Namespace = "http://xmlns.oracle.com/apps/prc/po/editDocument/flex/draftPurchasingDocumentLine/")]
            public string Clasificacion { get; set; }
            [XmlElement(ElementName = "__FLEX_Context", Namespace = "http://xmlns.oracle.com/apps/prc/po/editDocument/flex/draftPurchasingDocumentLine/")]
            public string FlexContext { get; set; }
            [XmlElement(ElementName = "viaje", Namespace = "http://xmlns.oracle.com/apps/prc/po/editDocument/flex/draftPurchasingDocumentLine/")]
            public string Viaje { get; set; }
        }

        [XmlRoot(ElementName = "PurchaseOrderEntryLine", Namespace = "http://xmlns.oracle.com/apps/prc/po/editDocument/purchaseOrderServiceV2/")]
        public class PurchaseOrderEntryLine
        {
            public PurchaseOrderEntryLine()
            {
                ItemNumber = string.Empty;
                ItemDescription = string.Empty;
                UnitOfMeasure = string.Empty;
                Quantity = new Quantity();
                Price = new Price();
                PurchaseOrderEntrySchedule = new List<PurchaseOrderEntrySchedule>();
                LineFlexfield = new List<LineFlexfield>();
            }

            [XmlElement(ElementName = "ItemNumber", Namespace = "http://xmlns.oracle.com/apps/prc/po/editDocument/purchaseOrderServiceV2/")]
            public string ItemNumber { get; set; }
            [XmlElement(ElementName = "ItemDescription", Namespace = "http://xmlns.oracle.com/apps/prc/po/editDocument/purchaseOrderServiceV2/")]
            public string ItemDescription { get; set; }
            [XmlElement(ElementName = "UnitOfMeasure", Namespace = "http://xmlns.oracle.com/apps/prc/po/editDocument/purchaseOrderServiceV2/")]
            public string UnitOfMeasure { get; set; }
            [XmlElement(ElementName = "Quantity", Namespace = "http://xmlns.oracle.com/apps/prc/po/editDocument/purchaseOrderServiceV2/")]
            public Quantity Quantity { get; set; }
            [XmlElement(ElementName = "Price", Namespace = "http://xmlns.oracle.com/apps/prc/po/editDocument/purchaseOrderServiceV2/")]
            public Price Price { get; set; }
            [XmlElement(ElementName = "PurchaseOrderEntrySchedule", Namespace = "http://xmlns.oracle.com/apps/prc/po/editDocument/purchaseOrderServiceV2/")]
            public List<PurchaseOrderEntrySchedule> PurchaseOrderEntrySchedule { get; set; }
            [XmlElement(ElementName = "LineFlexfield", Namespace = "http://xmlns.oracle.com/apps/prc/po/editDocument/purchaseOrderServiceV2/")]
            public List<LineFlexfield> LineFlexfield { get; set; }
        }

        [XmlRoot(ElementName = "createOrderEntry", Namespace = "http://xmlns.oracle.com/apps/prc/po/editDocument/purchaseOrderServiceV2/types/")]
        public class CreateOrderEntry
        {
            public CreateOrderEntry()
            {
                ProcurementBusinessUnit = string.Empty;
                RequisitioningBusinessUnit = string.Empty;
                Supplier = string.Empty;
                BuyerId = string.Empty;
                SupplierSiteCode = string.Empty;
                CurrencyCode = string.Empty;
                DocumentDescription = string.Empty;
                InterfaceSourceCode = string.Empty;
                ReferenceNumber = string.Empty;
                PurchaseOrderEntryLine = new List<PurchaseOrderEntryLine>();
            }

            [XmlElement(ElementName = "ProcurementBusinessUnit", Namespace = "http://xmlns.oracle.com/apps/prc/po/editDocument/purchaseOrderServiceV2/")]
            public string ProcurementBusinessUnit { get; set; }
            [XmlElement(ElementName = "RequisitioningBusinessUnit", Namespace = "http://xmlns.oracle.com/apps/prc/po/editDocument/purchaseOrderServiceV2/")]
            public string RequisitioningBusinessUnit { get; set; }
            [XmlElement(ElementName = "Supplier", Namespace = "http://xmlns.oracle.com/apps/prc/po/editDocument/purchaseOrderServiceV2/")]
            public string Supplier { get; set; }
            [XmlElement(ElementName = "BuyerId", Namespace = "http://xmlns.oracle.com/apps/prc/po/editDocument/purchaseOrderServiceV2/")]
            public string BuyerId { get; set; }
            [XmlElement(ElementName = "SupplierSiteCode", Namespace = "http://xmlns.oracle.com/apps/prc/po/editDocument/purchaseOrderServiceV2/")]
            public string SupplierSiteCode { get; set; }
            [XmlElement(ElementName = "CurrencyCode", Namespace = "http://xmlns.oracle.com/apps/prc/po/editDocument/purchaseOrderServiceV2/")]
            public string CurrencyCode { get; set; }
            [XmlElement(ElementName = "DocumentDescription", Namespace = "http://xmlns.oracle.com/apps/prc/po/editDocument/purchaseOrderServiceV2/")]
            public string DocumentDescription { get; set; }
            [XmlElement(ElementName = "InterfaceSourceCode", Namespace = "http://xmlns.oracle.com/apps/prc/po/editDocument/purchaseOrderServiceV2/")]
            public string InterfaceSourceCode { get; set; }
            [XmlElement(ElementName = "ReferenceNumber", Namespace = "http://xmlns.oracle.com/apps/prc/po/editDocument/purchaseOrderServiceV2/")]
            public string ReferenceNumber { get; set; }
            [XmlElement(ElementName = "PurchaseOrderEntryLine", Namespace = "http://xmlns.oracle.com/apps/prc/po/editDocument/purchaseOrderServiceV2/")]
            public List<PurchaseOrderEntryLine> PurchaseOrderEntryLine { get; set; }
        }

        [XmlRoot(ElementName = "createPurchaseOrder", Namespace = "http://xmlns.oracle.com/apps/prc/po/editDocument/purchaseOrderServiceV2/types/")]
        public class CreatePurchaseOrder
        {
            public CreatePurchaseOrder()
            {
                CreateOrderEntry = new CreateOrderEntry();
            }

            [XmlElement(ElementName = "createOrderEntry", Namespace = "http://xmlns.oracle.com/apps/prc/po/editDocument/purchaseOrderServiceV2/types/")]
            public CreateOrderEntry CreateOrderEntry { get; set; }
        }

        [XmlRoot(ElementName = "Body", Namespace = "http://schemas.xmlsoap.org/soap/envelope/")]
        public class Body
        {
            public Body()
            {
                CreatePurchaseOrder = new CreatePurchaseOrder();
            }

            [XmlElement(ElementName = "createPurchaseOrder", Namespace = "http://xmlns.oracle.com/apps/prc/po/editDocument/purchaseOrderServiceV2/types/")]
            public CreatePurchaseOrder CreatePurchaseOrder { get; set; }
        }

        [XmlRoot(ElementName = "Envelope", Namespace = "http://schemas.xmlsoap.org/soap/envelope/")]
        public class Envelope
        {
            public Envelope()
            {
                Header = string.Empty;
                Body = new Body();
                Soapenv = "http://schemas.xmlsoap.org/soap/envelope/";
                Typ = "http://xmlns.oracle.com/apps/prc/po/editDocument/purchaseOrderServiceV2/types/";
                Pur = "http://xmlns.oracle.com/apps/prc/po/editDocument/purchaseOrderServiceV2/";
                Draf = "http://xmlns.oracle.com/apps/prc/po/editDocument/flex/draftPurchaseOrderDistribution/";
                Pjc = "http://xmlns.oracle.com/apps/prc/po/commonPo/flex/PJCPoDraftDistribution/";
                Draf1 = "http://xmlns.oracle.com/apps/prc/po/editDocument/flex/draftPurchasingDocumentSchedule/";
                Draf2 = "http://xmlns.oracle.com/apps/prc/po/editDocument/flex/draftPurchasingDocumentLine/";
                Draf3 = "http://xmlns.oracle.com/apps/prc/po/editDocument/flex/draftPurchasingDocumentHeader/";
            }

            [XmlElement(ElementName = "Header", Namespace = "http://schemas.xmlsoap.org/soap/envelope/")]
            public string Header { get; set; }
            [XmlElement(ElementName = "Body", Namespace = "http://schemas.xmlsoap.org/soap/envelope/")]
            public Body Body { get; set; }
            [XmlAttribute(AttributeName = "soapenv", Namespace = "http://www.w3.org/2000/xmlns/")]
            public string Soapenv { get; set; }
            [XmlAttribute(AttributeName = "typ", Namespace = "http://www.w3.org/2000/xmlns/")]
            public string Typ { get; set; }
            [XmlAttribute(AttributeName = "pur", Namespace = "http://www.w3.org/2000/xmlns/")]
            public string Pur { get; set; }
            [XmlAttribute(AttributeName = "draf", Namespace = "http://www.w3.org/2000/xmlns/")]
            public string Draf { get; set; }
            [XmlAttribute(AttributeName = "pjc", Namespace = "http://www.w3.org/2000/xmlns/")]
            public string Pjc { get; set; }
            [XmlAttribute(AttributeName = "draf1", Namespace = "http://www.w3.org/2000/xmlns/")]
            public string Draf1 { get; set; }
            [XmlAttribute(AttributeName = "draf2", Namespace = "http://www.w3.org/2000/xmlns/")]
            public string Draf2 { get; set; }
            [XmlAttribute(AttributeName = "draf3", Namespace = "http://www.w3.org/2000/xmlns/")]
            public string Draf3 { get; set; }
        }
    }
}