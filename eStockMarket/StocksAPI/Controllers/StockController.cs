using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using StocksAPI.DTO;
using StocksAPI.Exceptions;
using StocksAPI.Models;
using StocksAPI.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StocksAPI.Controllers
{
    [ApiController]
    [Route("/api/v1.0/market/[controller]")]
    public class StockController : ControllerBase
    {

        private readonly ILogger<StockController> _logger;
        private readonly IStockService _stockService;
        readonly IMessageProducerService messageProducer;
        public StockController(ILogger<StockController> logger, IStockService stockService, IMessageProducerService producerService)
        {
            _logger = logger;
            _stockService = stockService;
            messageProducer = producerService;
        }
        [Authorize(AuthenticationSchemes =
JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost]
        [Route("/add/{companycode}")]
        public async Task<IActionResult> Post([FromBody] StockDto stock,string companycode)
        {
            try
            {
                Stock _stock = MapToStock(stock);
                _stock.CompanyCode = companycode;
                await _stockService.AddStockAsync(_stock);
                _logger.LogInformation($"Sending stock info to Kafka for {companycode}");
                messageProducer.WriteMessage("StockInfo", _stock);
                return CreatedAtAction(nameof(Post), companycode);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in Adding the stock: {ex.Message}");
                return StatusCode(500);
            }

        }

        private Stock MapToStock(StockDto stock)
        {
            Stock stock1 = new Stock
            {
                StockPrice = stock.StockPrice,
            };
            return stock1;
        }

        [HttpGet]
        [Route("/get/{companycode}/{startdate}/{enddate}")]
        public async Task<IActionResult> Get(string companycode, DateTime startdate, DateTime enddate)
        {
            try
            {
                _logger.LogInformation($"Getting stock of {companycode} for the time-period {startdate} - {enddate}");
                var stocks = await _stockService.GetAsync(companycode, startdate, enddate);
                return Ok(stocks);
            }
            catch (StockNotFoundException pnf)
            {
                _logger.LogInformation($"Required stock does not exist");
                return NotFound(pnf.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in fetching data: {ex.Message}");
                return StatusCode(500);
            }
        }
    }
}
