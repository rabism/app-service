using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StocksAPI.Models
{
    public class StockDbContext : IStockDbContext
    {
        private readonly IMongoDatabase _database = null;
        public StockDbContext()
        {
            var client = new MongoClient("mongodb://localhost:27017");
            if (client != null)
                _database = client.GetDatabase("stock_testdb");
        }
        public StockDbContext(IOptions<Settings> settings)
        {
            var client = new MongoClient(settings.Value.ConnectionString);
            if (client != null)
                _database = client.GetDatabase(settings.Value.Database);
        }

        public IMongoCollection<Stock> Stocks
        {
            get
            {
                return _database.GetCollection<Stock>("Stock");
            }
        }
    }
}
