using CompaniesAPI.DTO;
using CompaniesAPI.Exceptions;
using CompaniesAPI.Models;
using CompaniesAPI.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;

namespace CompaniesAPI.Controllers
{
    [ApiController]
    [Route("/api/v1.0/market/[controller]")]
    public class CompanyController : ControllerBase
    {
        readonly ICompanyService service;
        readonly ILogger<CompanyController> _logger;

        public CompanyController(ICompanyService companyService, ILogger<CompanyController> logger)
        {
            service = companyService;
            _logger = logger;
        }
        [Authorize(AuthenticationSchemes =
JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost]
        [Route("/register")]
        public IActionResult Post([FromBody] CompanyDTO company)
        {
            try
            {
                _logger.LogInformation($"Adding a new Company {company}");
                Company _company = MapToCompany(company);
                service.Register(_company);
                return Created("", company);
            }
            catch (CompanyAlreadyExistsException pae)
            {
                _logger.LogInformation($"This company already exists {company.CompanyCode}");
                return Conflict(pae.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in Adding the company: {ex.Message}");
                return StatusCode(500);
            }
        }
        [Authorize(AuthenticationSchemes =
JwtBearerDefaults.AuthenticationScheme)]
        [HttpDelete]
        [Route("/delete/{companycode}")]
        public IActionResult Delete(string companycode)
        {
            try
            {
                _logger.LogInformation($"Removing the company {companycode}");
                service.Delete(companycode);
                return NoContent();
            }
            catch (CompanyNotFoundException pnf)
            {
                _logger.LogInformation("Company trying to remove does not exist");
                return NotFound(pnf.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in deleting the company: {ex.Message}");
                return StatusCode(500);
            }
        }
        [HttpGet]
        [Route("/info/{companycode}")]
        public IActionResult Get(string companycode)
        {
            try
            {
                _logger.LogInformation($"Getting Company Details for {companycode}");
                return Ok(MapToCompanyDTO(service.GetCompany(companycode)));
            }
            catch (CompanyNotFoundException pnf)
            {
                _logger.LogInformation($"Required CompanyCode does not exist");
                return NotFound(pnf.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in fetching data: {ex.Message}");
                return StatusCode(500);
            }
        }
        [HttpGet]
        [Route("/getall")]
        public IActionResult Get()
        {
            try
            {
                _logger.LogInformation($"Getting All Companies");
                var comps = service.GetAllCompanies();
                var companies = new List<CompanyDTO>();
                if (comps != null && comps.Count > 0)
                {
                    foreach (var comp in comps)
                    {
                        companies.Add(MapToCompanyDTO(comp));
                    }
                    
                }
                return Ok(companies);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in fetching data: {ex.Message}");
                return StatusCode(500);
            }
        }
        Company MapToCompany(CompanyDTO companyDto)
        {
            Company comp = new Company
            {
                CompanyCEO = companyDto.CompanyCEO,
                CompanyCode = companyDto.CompanyCode,
                CompanyName = companyDto.CompanyName,
                CompanyTurnOver = companyDto.CompanyTurnOver,
                Website = companyDto.Website,
                Stocks = new List<Stock>
                 {
                     new Stock{
                     StockPrice = companyDto.StockPrice
                     }
                 }
            };
            return comp;
        }

        CompanyDTO MapToCompanyDTO(Company company)
        {
            CompanyDTO comp = new CompanyDTO
            {
                CompanyCEO = company.CompanyCEO,
                CompanyCode = company.CompanyCode,
                CompanyName = company.CompanyName,
                CompanyTurnOver = company.CompanyTurnOver,
                Website = company.Website,
                StockPrice = company.Stocks.OrderByDescending(x => x.StockDateTime).Select(x => x.StockPrice).FirstOrDefault()
            };
            return comp;
        }
    }

}
