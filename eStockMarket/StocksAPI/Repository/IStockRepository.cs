using StocksAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StocksAPI.Repository
{
    public interface IStockRepository
    {
        Task AddStockAsync(Stock stock);
        Task<IReadOnlyList<Stock>> GetStockByCompanyCodeAndStartDateEndDate(string companyCode, DateTime startDate, DateTime endDate);
        Task<IReadOnlyList<Stock>> GetStockByCompanyCode(string companyCode);
        Task<bool> IsCompanyExistsAsync(string companyCode);
    }
}
