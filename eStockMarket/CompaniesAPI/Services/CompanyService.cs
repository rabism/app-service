using CompaniesAPI.Exceptions;
using CompaniesAPI.Models;
using CompaniesAPI.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CompaniesAPI.Services
{
    public class CompanyService : ICompanyService
    {
        readonly ICompanyRepository repository;
        public CompanyService(ICompanyRepository companyRepository)
        {
            repository = companyRepository;
        }

        public Company Register(Company company)
        {
            if (repository.IsCompanyExists(company.CompanyCode))
            {
                throw new CompanyAlreadyExistsException($"Company {company.CompanyCode} Already Exists !!!");
            }
            else
            {
                return repository.RegisterCompany(company);
            }
        }

        public Company Delete(string companyCode)
        {
            var existingCompany = repository.GetCompanyByCode(companyCode);
            if (existingCompany == null)
            {
                throw new CompanyNotFoundException($"Company with code {companyCode} Does Not Exist !!!");
            }
            else
            {
                return repository.DeleteCompany(companyCode);
            }
        }

        public Company GetCompany(string companyCode)
        {
            var comp = repository.GetCompanyByCode(companyCode);
            if (comp == null)
                throw new CompanyNotFoundException("Company with this company code does not exist");
            return comp;
        }

        public IReadOnlyList<Company> GetAllCompanies()
        {
            return repository.GetAllCompanies();
        }
        public void UpdateCompanyStock(Stock stock)
        {
            var comp = repository.GetCompanyByCode(stock.CompanyCode);
            if (comp == null)
                throw new CompanyNotFoundException("Company with this code not found");
            repository.UpdateCompanyStock(stock);
        }
    }
}
