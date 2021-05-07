using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Serialization;

namespace LogisticaERP.Clases.RecepcionarASN.OracleCloud.DTO
{
    [XmlRoot(ElementName = "G_1")]
    public class Organization
    {
        [XmlElement(ElementName = "LEGAL_ENTITY_ID")]
        public string LegalEntityID { get; set; }
        [XmlElement(ElementName = "PRIMARY_LEDGER_ID")]
        public string PrimaryLedgerID { get; set; }
        [XmlElement(ElementName = "DEFAULT_SET_ID")]
        public string DefaultSetID { get; set; }
        [XmlElement(ElementName = "CHART_OF_ACCOUNTS_ID")]
        public string ChartOfAccountsID { get; set; }
        [XmlElement(ElementName = "CURRENCY_CODE")]
        public string CurrencyCode { get; set; }
        [XmlElement(ElementName = "PERIOD_SET_NAME")]
        public string PeriodSetName { get; set; }
        [XmlElement(ElementName = "BUSINESS_UNIT_ID")]
        public string BusinessUnitID { get; set; }
        [XmlElement(ElementName = "BU_NAME")]
        public string BusinessUnitName { get; set; }
        [XmlElement(ElementName = "ORGANIZATION_ID")]
        public string OrganizationID { get; set; }
        [XmlElement(ElementName = "ORGANIZATION_CODE")]
        public string OrganizationCode { get; set; }
        [XmlElement(ElementName = "ORGANIZATION_NAME")]
        public string OrganizationName { get; set; }
        [XmlElement(ElementName = "SUBINVENTORY_ID")]
        public string SubinventoryID { get; set; }
        [XmlElement(ElementName = "SUBINVENTORY_NAME")]
        public string SubincentoryName { get; set; }
        [XmlElement(ElementName = "SUBINVENTORY_DESCRIPTION")]
        public string SubinventoryDescription { get; set; }
    }

    [XmlRoot(ElementName = "DATA_DS")]
    public class DSOrganization
    {
        [XmlElement(ElementName = "P_BUSINESS_UNIT_ID")]
        public string PBusinessUnitID { get; set; }
        [XmlElement(ElementName = "G_1")]
        public List<Organization> Organizations { get; set; }
    }
}