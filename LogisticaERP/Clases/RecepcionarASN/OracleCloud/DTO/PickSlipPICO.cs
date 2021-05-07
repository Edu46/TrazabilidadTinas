using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Serialization;

namespace LogisticaERP.Clases.RecepcionarASN.OracleCloud.DTO
{
    [Serializable]
    [XmlRoot(ElementName = "G_1")]
    public class PickSlipPICO : ICloneable
    {
        [XmlElement(ElementName = "CREATION_DATE")]
        public string CreationDate { get; set; }
        [XmlElement(ElementName = "ORGANIZATION_CODE_ORIGEN")]
        public string SourceOrganizationCode { get; set; }
        [XmlElement(ElementName = "ORGANIZATION_CODE_DESTINO")]
        public string DestinationOrganizationCode { get; set; }
        [XmlElement(ElementName = "PICK_SLIP_ID")]
        public string PickSlipID { get; set; }
        [XmlElement(ElementName = "NO_ENVIO")]
        public string SalesOrderNumber { get; set; }
        [XmlElement(ElementName = "LINE_NUMBER")]
        public string LineNumber { get; set; }
        [XmlElement(ElementName = "NO_PRODUCTO")]
        public string ItemNumber { get; set; }
        [XmlElement(ElementName = "DESCRIPCION")]
        public string ItemDescription { get; set; }
        [XmlElement(ElementName = "CANTIDAD")]
        public string Quantity { get; set; }
        [XmlElement(ElementName = "UNIT_OF_MEASURE")]
        public string UnitOfMeasure { get; set; }
        [XmlElement(ElementName = "SUB_ALMACEN_ORIGEN")]
        public string SourceSubinventoryCode { get; set; }
        [XmlElement(ElementName = "SUB_ALMACEN_DESTINO")]
        public string DestinationSubinventoryCode { get; set; }
        public decimal CantidadSurtida { get; set; }

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }

    [Serializable]
    [XmlRoot(ElementName = "DATA_DS")]
    public class DSINVPickSplipPICO
    {
        [XmlElement(ElementName = "P_ORG_CODE_ORIG")]
        public string POrgCodeOrig { get; set; }
        [XmlElement(ElementName = "P_ORG_CODE_DEST")]
        public string POrgCodeDest { get; set; }
        [XmlElement(ElementName = "G_1")]
        public List<PickSlipPICO> PickSlips { get; set; }
    }
}