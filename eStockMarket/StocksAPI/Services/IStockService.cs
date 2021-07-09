using StocksAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StocksAPI.Services
{
    public interface IStockService
    {
        Task<Stock> AddStockAsync(Stock stock);
        Task<IReadOnlyList<Stock>> GetAsync(string companyCode, DateTime startDate, DateTime endDate);
    }
}
