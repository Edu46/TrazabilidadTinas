using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LogisticaERP.Clases.RecepcionarASN.OracleCloud.DTO
{
    public class PickTransactionResponse
    {
        public class PickTransaction
        {
            [JsonProperty("TransactionHeaderId")]
            public long TransactionHeaderId { get; set; }

            [JsonProperty("ReturnStatus")]
            public string ReturnStatus { get; set; }

            [JsonProperty("ErrorCode")]
            public object ErrorCode { get; set; }

            [JsonProperty("ErrorExplanation")]
            public object ErrorExplanation { get; set; }

            [JsonProperty("PickSlip")]
            public object PickSlip { get; set; }

            [JsonProperty("PickSlipLine")]
            public object PickSlipLine { get; set; }

            [JsonProperty("OverpickAndMoveFlag")]
            [JsonConverter(typeof(ParseStringConverter))]
            public bool OverpickAndMoveFlag { get; set; }

            [JsonProperty("pickLines")]
            public List<PickLine> PickLines { get; set; }

            [JsonProperty("links")]
            public List<Link> Links { get; set; }
        }

        public class Link
        {
            [JsonProperty("rel")]
            public Rel Rel { get; set; }

            [JsonProperty("href")]
            public Uri Href { get; set; }

            [JsonProperty("name")]
            public Name Name { get; set; }

            [JsonProperty("kind")]
            public Kind Kind { get; set; }

            [JsonProperty("properties", NullValueHandling = NullValueHandling.Ignore)]
            public Properties Properties { get; set; }
        }

        public class Properties
        {
            [JsonProperty("changeIndicator")]
            public string ChangeIndicator { get; set; }
        }

        public class PickLine
        {
            [JsonProperty("Locator")]
            public object Locator { get; set; }

            [JsonProperty("PickSlipLine")]
            public long PickSlipLine { get; set; }

            [JsonProperty("PickSlip")]
            public long PickSlip { get; set; }

            [JsonProperty("PickedQuantity")]
            public long PickedQuantity { get; set; }

            [JsonProperty("SecondaryPickedQuantity")]
            public object SecondaryPickedQuantity { get; set; }

            [JsonProperty("SubinventoryCode")]
            public string SubinventoryCode { get; set; }

            [JsonProperty("TransactionDate")]
            public object TransactionDate { get; set; }

            [JsonProperty("LicensePlateNumber")]
            public object LicensePlateNumber { get; set; }

            [JsonProperty("DestinationLocator")]
            public object DestinationLocator { get; set; }

            [JsonProperty("DestinationSubinventoryCode")]
            public object DestinationSubinventoryCode { get; set; }

            [JsonProperty("lotItemLots")]
            public List<LotItemLot> LotItemLots { get; set; }

            [JsonProperty("links")]
            public List<Link> Links { get; set; }
        }

        public class LotItemLot
        {
            [JsonProperty("Lot")]
            public string Lot { get; set; }

            [JsonProperty("Quantity")]
            public long Quantity { get; set; }

            [JsonProperty("SecondaryQuantity")]
            public object SecondaryQuantity { get; set; }

            [JsonProperty("links")]
            public List<Link> Links { get; set; }
        }

        public enum Kind { Collection, Item };

        public enum Name { Dffs, InventoryAttributesDff, LotItemLots, LotSerialItemLots, PickExceptions, PickLines, PickTransactions, SerialItemSerials };

        public enum Rel { Canonical, Child, Parent, Self };

        internal class ParseStringConverter : JsonConverter
        {
            public override bool CanConvert(Type t)
            {
                return t == typeof(bool) || t == typeof(bool?);
            }

            public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
            {
                if (reader.TokenType == JsonToken.Null) return null;
                var value = serializer.Deserialize<string>(reader);
                bool b;
                if (Boolean.TryParse(value, out b))
                {
                    return b;
                }
                throw new Exception("Cannot unmarshal type bool");
            }

            public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
            {
                if (untypedValue == null)
                {
                    serializer.Serialize(writer, null);
                    return;
                }
                var value = (bool)untypedValue;
                var boolString = value ? "true" : "false";
                serializer.Serialize(writer, boolString);
                return;
            }

            public static readonly ParseStringConverter Singleton = new ParseStringConverter();
        }
    }
}