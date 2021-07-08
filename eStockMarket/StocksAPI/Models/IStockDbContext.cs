using MongoDB.Driver;

namespace StocksAPI.Models
{
    public interface IStockDbContext
    {
        IMongoCollection<Stock> Stocks { get; }
    }
}