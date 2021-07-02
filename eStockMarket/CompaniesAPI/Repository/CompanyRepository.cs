using CompaniesAPI.DBContexts;
using CompaniesAPI.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CompaniesAPI.Repository
{
    public class CompanyRepository : ICompanyRepository
    {
        private readonly CompanyContext _dbContext;

        public CompanyRepository(CompanyContext dbContext)
        {
            _dbContext = dbContext;
        }
        public Company RegisterCompany(Company company)
        {
            _dbContext.Companies.Add(company);
            _dbContext.SaveChanges();
            return company;
        }

        public Company DeleteCompany(string companyCode)
        {
            var comp = this.GetCompanyByCode(companyCode);
            _dbContext.Companies.Remove(comp);
            _dbContext.SaveChanges();
            return comp;
        }

        public Company GetCompanyByCode(string companyCode)
        {
            return _dbContext.Companies.Include(x => x.Stocks).ToList().Find(x=> x.CompanyCode.Equals(companyCode,StringComparison.OrdinalIgnoreCase));
        }

        public IReadOnlyList<Company> GetAllCompanies()
        {
            return _dbContext.Companies.Include(x=>x.Stocks).ToList();
        }

        public bool IsCompanyExists(string companyCode)
        {
            var comp = GetCompanyByCode(companyCode);
            return comp != null;
        }
        public void UpdateCompanyStock(Stock stock)
        {
            var comp = _dbContext.Companies.Where(x => x.CompanyCode.Equals(stock.CompanyCode)).FirstOrDefault();
            
            if (comp!=null)
            {
                stock.StockId = 0;
                stock.Company = comp;
                _dbContext.Stocks.Add(stock);
                _dbContext.SaveChanges();
            }           
        }
    }
}
