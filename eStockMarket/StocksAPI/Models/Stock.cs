using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Text.Json.Serialization;
using MongoDB.Bson;

namespace StocksAPI.Models
{
    public class Stock 
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string StockId { get; set; }
        public decimal StockPrice { get; set; }
        public DateTime StockDateTime { get; set; } = DateTime.Now;
        public string CompanyCode { get; set; }
        [BsonIgnore]
        public string time {get;set;}
        [BsonIgnore]
        [JsonIgnore]
        public virtual Company Company { get; set; }
    }
}
