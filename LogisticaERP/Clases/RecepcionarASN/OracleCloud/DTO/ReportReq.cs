using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Serialization;

namespace LogisticaERP.Clases.RecepcionarASN.OracleCloud.DTO
{
    public sealed class ReportReq
    {
        [XmlRoot(ElementName = "values", Namespace = "http://xmlns.oracle.com/oxp/service/PublicReportService")]
        public class Values
        {
            public Values()
            {
                Item = string.Empty;
            }

            [XmlElement(ElementName = "item", Namespace = "http://xmlns.oracle.com/oxp/service/PublicReportService")]
            public string Item { get; set; }
        }

        [XmlRoot(ElementName = "item", Namespace = "http://xmlns.oracle.com/oxp/service/PublicReportService")]
        public class Item
        {
            public Item()
            {
                Name = string.Empty;
                Values = new List<Values>();
            }

            [XmlElement(ElementName = "name", Namespace = "http://xmlns.oracle.com/oxp/service/PublicReportService")]
            public string Name { get; set; }
            [XmlElement(ElementName = "values", Namespace = "http://xmlns.oracle.com/oxp/service/PublicReportService")]
            public List<Values> Values { get; set; }
        }

        [XmlRoot(ElementName = "parameterNameValues", Namespace = "http://xmlns.oracle.com/oxp/service/PublicReportService")]
        public class ParameterNameValues
        {
            public ParameterNameValues()
            {
                Items = new List<Item>();
            }

            [XmlElement(ElementName = "item", Namespace = "http://xmlns.oracle.com/oxp/service/PublicReportService")]
            public List<Item> Items { get; set; }
        }

        [XmlRoot(ElementName = "reportRequest", Namespace = "http://xmlns.oracle.com/oxp/service/PublicReportService")]
        public class ReportRequest
        {
            public ReportRequest()
            {
                ParameterNameValues = new ParameterNameValues();
                ReportAbsolutePath = string.Empty;
                SizeOfDataChunkDownload = string.Empty;
            }

            [XmlElement(ElementName = "parameterNameValues", Namespace = "http://xmlns.oracle.com/oxp/service/PublicReportService")]
            public ParameterNameValues ParameterNameValues { get; set; }
            [XmlElement(ElementName = "reportAbsolutePath", Namespace = "http://xmlns.oracle.com/oxp/service/PublicReportService")]
            public string ReportAbsolutePath { get; set; }
            [XmlElement(ElementName = "sizeOfDataChunkDownload", Namespace = "http://xmlns.oracle.com/oxp/service/PublicReportService")]
            public string SizeOfDataChunkDownload { get; set; }
        }

        [XmlRoot(ElementName = "runReport", Namespace = "http://xmlns.oracle.com/oxp/service/PublicReportService")]
        public class RunReport
        {
            public RunReport()
            {
                ReportRequest = new ReportRequest();
            }

            [XmlElement(ElementName = "reportRequest", Namespace = "http://xmlns.oracle.com/oxp/service/PublicReportService")]
            public ReportRequest ReportRequest { get; set; }
        }

        [XmlRoot(ElementName = "Body", Namespace = "http://www.w3.org/2003/05/soap-envelope")]
        public class Body
        {
            public Body()
            {
                RunReport = new RunReport();
            }

            [XmlElement(ElementName = "runReport", Namespace = "http://xmlns.oracle.com/oxp/service/PublicReportService")]
            public RunReport RunReport { get; set; }
        }

        [XmlRoot(ElementName = "Envelope", Namespace = "http://www.w3.org/2003/05/soap-envelope")]
        public class Envelope
        {
            public Envelope()
            {
                Header = string.Empty;
                Body = new Body();
                Soap = "http://www.w3.org/2003/05/soap-envelope";
                Pub = "http://xmlns.oracle.com/oxp/service/PublicReportService";
            }

            [XmlElement(ElementName = "Header", Namespace = "http://www.w3.org/2003/05/soap-envelope")]
            public string Header { get; set; }
            [XmlElement(ElementName = "Body", Namespace = "http://www.w3.org/2003/05/soap-envelope")]
            public Body Body { get; set; }
            [XmlAttribute(AttributeName = "soap", Namespace = "http://www.w3.org/2000/xmlns/")]
            public string Soap { get; set; }
            [XmlAttribute(AttributeName = "pub", Namespace = "http://www.w3.org/2000/xmlns/")]
            public string Pub { get; set; }
        }
    }
}