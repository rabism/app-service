using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CompaniesAPI.Models;

namespace CompaniesAPI.Services
{
    public interface ICompanyService
    {
        Company Register(Company company);
        Company Delete(string companyCode);
        Company GetCompany(string companyCode);
        void UpdateCompanyStock(Stock stock);
        IReadOnlyList<Company> GetAllCompanies();
    }
}
