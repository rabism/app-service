using StocksAPI.Exceptions;
using StocksAPI.Models;
using StocksAPI.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StocksAPI.Services
{
    public class StockService : IStockService
    {
        private IStockRepository _stockRepository;

        public StockService(IStockRepository stockRepository)
        {
            _stockRepository = stockRepository;
        }
        public async Task AddStockAsync(Stock stock)
        {
            //if (!(await _stockRepository.IsCompanyExistsAsync(stock.CompanyCode)))
            //{
            //    throw new CompanyNotRegisteredExaception($"Company code {stock.CompanyCode} not registered!!");
            //}
            //else
            //{
                await _stockRepository.AddStockAsync(stock);
            //}
        }

        public async Task<IReadOnlyList<Stock>> GetAsync(string companyCode, DateTime startDate, DateTime endDate)
        {
            var existingStock = await _stockRepository.GetStockByCompanyCodeAndStartDateEndDate(companyCode, startDate, endDate);
            if (existingStock == null)
            {
                throw new StockNotFoundException($"Stock with company code {companyCode} Does Not Exist !!!");
            }

            return existingStock;
        }

    }
}
