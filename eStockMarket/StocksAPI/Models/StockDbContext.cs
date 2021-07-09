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
            string connectionString = Environment.GetEnvironmentVariable("MONGO_CONNECTION_STRING");
            string database = Environment.GetEnvironmentVariable("MONGO_DATABASE_NAME");
            if (connectionString == null || database == null)
            {
                connectionString = "mongodb://localhost:27017";
                database = "StockDB";
            }
            
            var client = new MongoClient(connectionString);
            if (client != null)
                _database = client.GetDatabase(database);
        }
       /*
        public StockDbContext(IOptions<Settings> settings)
        {
            var client = new MongoClient(settings.Value.ConnectionString);
            if (client != null)
                _database = client.GetDatabase(settings.Value.Database);
        }
        */
       public IMongoCollection<Stock> Stocks
        {
            get
            {
                return _database.GetCollection<Stock>("Stock");
            }
        }
    }
}
