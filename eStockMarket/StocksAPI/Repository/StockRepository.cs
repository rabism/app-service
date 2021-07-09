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
        readonly IStockDbContext context;
        public StockRepository(IStockDbContext dbContext)
        {
            context = dbContext;
        }
        public async Task AddStockAsync(Stock stock)
        {
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
