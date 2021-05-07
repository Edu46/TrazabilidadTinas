using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Configuration;
using System.Xml.Serialization;

namespace LogisticaERP.Clases.CalculoTarifaRepresentante.OracleCloud
{
    public class ValidateAndCreateAccountReq
    {
        [XmlRoot(ElementName = "validationInputRowList", Namespace = "http://xmlns.oracle.com/apps/financials/generalLedger/accounts/codeCombinations/accountCombinationService/types/")]
        public class ValidationInputRowList
        {
            public ValidationInputRowList()
            {
                DynamicInsertion = string.Empty;
                Segment1 = string.Empty;
                Segment2 = string.Empty;
                Segment3 = string.Empty;
                Segment4 = string.Empty;
                Segment5 = string.Empty;
                Segment6 = string.Empty;
                Segment7 = string.Empty;
                Segment8 = string.Empty;
                Segment9 = string.Empty;
                Segment10 = string.Empty;
                Segment11 = string.Empty;
                Segment12 = string.Empty;
                Segment13 = string.Empty;
                Segment14 = string.Empty;
                Segment15 = string.Empty;
                Segment16 = string.Empty;
                Segment17 = string.Empty;
                Segment18 = string.Empty;
                Segment19 = string.Empty;
                Segment20 = string.Empty;
                Segment21 = string.Empty;
                Segment22 = string.Empty;
                Segment23 = string.Empty;
                Segment24 = string.Empty;
                Segment25 = string.Empty;
                Segment26 = string.Empty;
                Segment27 = string.Empty;
                Segment28 = string.Empty;
                Segment29 = string.Empty;
                Segment30 = string.Empty;
                LedgerName = string.Empty;
                EnabledFlag = string.Empty;
                FromDate = string.Empty;
                ToDate = string.Empty;
            }

            [XmlElement(ElementName = "DynamicInsertion", Namespace = "http://xmlns.oracle.com/apps/financials/generalLedger/accounts/codeCombinations/accountCombinationService/")]
            public string DynamicInsertion { get; set; }
            [XmlElement(ElementName = "Segment1", Namespace = "http://xmlns.oracle.com/apps/financials/generalLedger/accounts/codeCombinations/accountCombinationService/")]
            public string Segment1 { get; set; }
            [XmlElement(ElementName = "Segment2", Namespace = "http://xmlns.oracle.com/apps/financials/generalLedger/accounts/codeCombinations/accountCombinationService/")]
            public string Segment2 { get; set; }
            [XmlElement(ElementName = "Segment3", Namespace = "http://xmlns.oracle.com/apps/financials/generalLedger/accounts/codeCombinations/accountCombinationService/")]
            public string Segment3 { get; set; }
            [XmlElement(ElementName = "Segment4", Namespace = "http://xmlns.oracle.com/apps/financials/generalLedger/accounts/codeCombinations/accountCombinationService/")]
            public string Segment4 { get; set; }
            [XmlElement(ElementName = "Segment5", Namespace = "http://xmlns.oracle.com/apps/financials/generalLedger/accounts/codeCombinations/accountCombinationService/")]
            public string Segment5 { get; set; }
            [XmlElement(ElementName = "Segment6", Namespace = "http://xmlns.oracle.com/apps/financials/generalLedger/accounts/codeCombinations/accountCombinationService/")]
            public string Segment6 { get; set; }
            [XmlElement(ElementName = "Segment7", Namespace = "http://xmlns.oracle.com/apps/financials/generalLedger/accounts/codeCombinations/accountCombinationService/")]
            public string Segment7 { get; set; }
            [XmlElement(ElementName = "Segment8", Namespace = "http://xmlns.oracle.com/apps/financials/generalLedger/accounts/codeCombinations/accountCombinationService/")]
            public string Segment8 { get; set; }
            [XmlElement(ElementName = "Segment9", Namespace = "http://xmlns.oracle.com/apps/financials/generalLedger/accounts/codeCombinations/accountCombinationService/")]
            public string Segment9 { get; set; }
            [XmlElement(ElementName = "Segment10", Namespace = "http://xmlns.oracle.com/apps/financials/generalLedger/accounts/codeCombinations/accountCombinationService/")]
            public string Segment10 { get; set; }
            [XmlElement(ElementName = "Segment11", Namespace = "http://xmlns.oracle.com/apps/financials/generalLedger/accounts/codeCombinations/accountCombinationService/")]
            public string Segment11 { get; set; }
            [XmlElement(ElementName = "Segment12", Namespace = "http://xmlns.oracle.com/apps/financials/generalLedger/accounts/codeCombinations/accountCombinationService/")]
            public string Segment12 { get; set; }
            [XmlElement(ElementName = "Segment13", Namespace = "http://xmlns.oracle.com/apps/financials/generalLedger/accounts/codeCombinations/accountCombinationService/")]
            public string Segment13 { get; set; }
            [XmlElement(ElementName = "Segment14", Namespace = "http://xmlns.oracle.com/apps/financials/generalLedger/accounts/codeCombinations/accountCombinationService/")]
            public string Segment14 { get; set; }
            [XmlElement(ElementName = "Segment15", Namespace = "http://xmlns.oracle.com/apps/financials/generalLedger/accounts/codeCombinations/accountCombinationService/")]
            public string Segment15 { get; set; }
            [XmlElement(ElementName = "Segment16", Namespace = "http://xmlns.oracle.com/apps/financials/generalLedger/accounts/codeCombinations/accountCombinationService/")]
            public string Segment16 { get; set; }
            [XmlElement(ElementName = "Segment17", Namespace = "http://xmlns.oracle.com/apps/financials/generalLedger/accounts/codeCombinations/accountCombinationService/")]
            public string Segment17 { get; set; }
            [XmlElement(ElementName = "Segment18", Namespace = "http://xmlns.oracle.com/apps/financials/generalLedger/accounts/codeCombinations/accountCombinationService/")]
            public string Segment18 { get; set; }
            [XmlElement(ElementName = "Segment19", Namespace = "http://xmlns.oracle.com/apps/financials/generalLedger/accounts/codeCombinations/accountCombinationService/")]
            public string Segment19 { get; set; }
            [XmlElement(ElementName = "Segment20", Namespace = "http://xmlns.oracle.com/apps/financials/generalLedger/accounts/codeCombinations/accountCombinationService/")]
            public string Segment20 { get; set; }
            [XmlElement(ElementName = "Segment21", Namespace = "http://xmlns.oracle.com/apps/financials/generalLedger/accounts/codeCombinations/accountCombinationService/")]
            public string Segment21 { get; set; }
            [XmlElement(ElementName = "Segment22", Namespace = "http://xmlns.oracle.com/apps/financials/generalLedger/accounts/codeCombinations/accountCombinationService/")]
            public string Segment22 { get; set; }
            [XmlElement(ElementName = "Segment23", Namespace = "http://xmlns.oracle.com/apps/financials/generalLedger/accounts/codeCombinations/accountCombinationService/")]
            public string Segment23 { get; set; }
            [XmlElement(ElementName = "Segment24", Namespace = "http://xmlns.oracle.com/apps/financials/generalLedger/accounts/codeCombinations/accountCombinationService/")]
            public string Segment24 { get; set; }
            [XmlElement(ElementName = "Segment25", Namespace = "http://xmlns.oracle.com/apps/financials/generalLedger/accounts/codeCombinations/accountCombinationService/")]
            public string Segment25 { get; set; }
            [XmlElement(ElementName = "Segment26", Namespace = "http://xmlns.oracle.com/apps/financials/generalLedger/accounts/codeCombinations/accountCombinationService/")]
            public string Segment26 { get; set; }
            [XmlElement(ElementName = "Segment27", Namespace = "http://xmlns.oracle.com/apps/financials/generalLedger/accounts/codeCombinations/accountCombinationService/")]
            public string Segment27 { get; set; }
            [XmlElement(ElementName = "Segment28", Namespace = "http://xmlns.oracle.com/apps/financials/generalLedger/accounts/codeCombinations/accountCombinationService/")]
            public string Segment28 { get; set; }
            [XmlElement(ElementName = "Segment29", Namespace = "http://xmlns.oracle.com/apps/financials/generalLedger/accounts/codeCombinations/accountCombinationService/")]
            public string Segment29 { get; set; }
            [XmlElement(ElementName = "Segment30", Namespace = "http://xmlns.oracle.com/apps/financials/generalLedger/accounts/codeCombinations/accountCombinationService/")]
            public string Segment30 { get; set; }
            [XmlElement(ElementName = "LedgerName", Namespace = "http://xmlns.oracle.com/apps/financials/generalLedger/accounts/codeCombinations/accountCombinationService/")]
            public string LedgerName { get; set; }
            [XmlElement(ElementName = "EnabledFlag", Namespace = "http://xmlns.oracle.com/apps/financials/generalLedger/accounts/codeCombinations/accountCombinationService/")]
            public string EnabledFlag { get; set; }
            [XmlElement(ElementName = "FromDate", Namespace = "http://xmlns.oracle.com/apps/financials/generalLedger/accounts/codeCombinations/accountCombinationService/")]
            public string FromDate { get; set; }
            [XmlElement(ElementName = "ToDate", Namespace = "http://xmlns.oracle.com/apps/financials/generalLedger/accounts/codeCombinations/accountCombinationService/")]
            public string ToDate { get; set; }
        }

        [XmlRoot(ElementName = "validateAndCreateAccounts", Namespace = "http://xmlns.oracle.com/apps/financials/generalLedger/accounts/codeCombinations/accountCombinationService/types/")]
        public class ValidateAndCreateAccounts
        {
            public ValidateAndCreateAccounts()
            {
                ValidationInputRowList = new List<ValidationInputRowList>();
            }

            [XmlElement(ElementName = "validationInputRowList", Namespace = "http://xmlns.oracle.com/apps/financials/generalLedger/accounts/codeCombinations/accountCombinationService/types/")]
            public List<ValidationInputRowList> ValidationInputRowList { get; set; }
        }

        [XmlRoot(ElementName = "Body", Namespace = "http://schemas.xmlsoap.org/soap/envelope/")]
        public class Body
        {
            public Body()
            {
                ValidateAndCreateAccounts = new ValidateAndCreateAccounts();
            }

            [XmlElement(ElementName = "validateAndCreateAccounts", Namespace = "http://xmlns.oracle.com/apps/financials/generalLedger/accounts/codeCombinations/accountCombinationService/types/")]
            public ValidateAndCreateAccounts ValidateAndCreateAccounts { get; set; }
        }

        [XmlRoot(ElementName = "Envelope", Namespace = "http://schemas.xmlsoap.org/soap/envelope/")]
        public class Envelope
        {
            public Envelope()
            {
                Header = string.Empty;
                Body = new Body();
                Soapenv = "http://schemas.xmlsoap.org/soap/envelope/";
                Typ = "http://xmlns.oracle.com/apps/financials/generalLedger/accounts/codeCombinations/accountCombinationService/types/";
                Acc = "http://xmlns.oracle.com/apps/financials/generalLedger/accounts/codeCombinations/accountCombinationService/";
            }

            [XmlElement(ElementName = "Header", Namespace = "http://schemas.xmlsoap.org/soap/envelope/")]
            public string Header { get; set; }
            [XmlElement(ElementName = "Body", Namespace = "http://schemas.xmlsoap.org/soap/envelope/")]
            public Body Body { get; set; }
            [XmlAttribute(AttributeName = "soapenv", Namespace = "http://www.w3.org/2000/xmlns/")]
            public string Soapenv { get; set; }
            [XmlAttribute(AttributeName = "typ", Namespace = "http://www.w3.org/2000/xmlns/")]
            public string Typ { get; set; }
            [XmlAttribute(AttributeName = "acc", Namespace = "http://www.w3.org/2000/xmlns/")]
            public string Acc { get; set; }
        }
    }
}