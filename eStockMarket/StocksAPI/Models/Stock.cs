using MongoDB.Bson.Serialization.Attributes;
using StocksAPI.EventSourcing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace StocksAPI.Models
{
    public class Stock : BaseAggregateRoot<Stock, Guid>
    {
        [BsonId]
        public int StockId { get; set; }

        public decimal StockPrice { get; set; }

        public DateTime StockDateTime { get; set; } = DateTime.Now;
        public string CompanyCode { get; set; }
        [BsonIgnore]
        [JsonIgnore]
        public virtual Company Company { get; set; }

        public Stock(Guid id, int stockId, decimal stockPrice, string companyCode) : base(id)
        {
            if (stockId <= 0)
                throw new ArgumentOutOfRangeException(nameof(stockId));
            if (stockPrice <= 0)
                throw new ArgumentOutOfRangeException(nameof(stockPrice));
            if (string.IsNullOrWhiteSpace(companyCode))
                throw new ArgumentOutOfRangeException(nameof(companyCode));

            StockId = stockId;
            StockPrice = stockPrice;
            CompanyCode = companyCode;
            StockDateTime = DateTime.Now;

            this.AddEvent(new StockAdded(this));
        }
        protected override void Apply(IDomainEvent<Guid> @event)
        {
            switch (@event)
            {
                case StockAdded s:
                    this.Id = s.AggregateId;
                    this.StockId = s.stockId;
                    this.StockPrice = s.stockPrice;
                    this.StockDateTime = s.StockDateTime;
                    this.CompanyCode = s.companyCode;
                    break;
            }
        }

        public static Stock Add(int stockId, decimal stockPrice, string companyCode)
        {
            return new Stock(Guid.NewGuid(), stockId, stockPrice, companyCode);
        }
    }
}
