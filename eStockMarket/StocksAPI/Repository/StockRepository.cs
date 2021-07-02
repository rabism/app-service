using MongoDB.Driver;
using StocksAPI.Exceptions;
using StocksAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StocksAPI.Repository
{
    public class StockRepository : IStockRepository
    {
        readonly StockDbContext context;
        public StockRepository(StockDbContext dbContext)
        {
            context = dbContext;
        }
        public async Task AddStockAsync(Stock stock)
        {
            int id = 101;
            var stockList = await this.GetStockByCompanyCode(stock.CompanyCode);
            if (stockList.Count > 0)
                id = stockList.Max(d => d.StockId) + 1;
            stock.StockId = id;
            await context.Stocks.InsertOneAsync(stock);
        }

        public async Task<IReadOnlyList<Stock>> GetStockByCompanyCodeAndStartDateEndDate(string companyCode, DateTime startDate, DateTime endDate)
        {
            return await context.Stocks.FindAsync(x => x.CompanyCode.Equals(companyCode) && x.StockDateTime > startDate && x.StockDateTime < endDate).Result.ToListAsync();
        }
        public async Task<IReadOnlyList<Stock>> GetStockByCompanyCode(string companyCode)
        {
            return await context.Stocks.FindAsync(x => x.CompanyCode.Equals(companyCode)).Result.ToListAsync();
        }
        public async Task<bool> IsCompanyExistsAsync(string companyCode)
        {
            var comp = await GetStockByCompanyCode(companyCode);
            return comp != null && comp.Count > 0;
        }
    }
}
