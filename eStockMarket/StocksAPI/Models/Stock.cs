using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace StocksAPI.Models
{
    public class Stock
    {
        [BsonId]
        public int StockId { get; set; }

        public decimal StockPrice { get; set; }

        public DateTime StockDateTime { get; set; } = DateTime.Now;
        public string CompanyCode { get; set; }
        [BsonIgnore]
        [JsonIgnore]
        public virtual Company Company { get; set; }
    }
}
