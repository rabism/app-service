using MediatR;
using StocksAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MongoDB.Driver;

namespace StocksAPI.Features.StockFeatures.Commands
{
    public class AddStockCommand : IRequest<Stock>
    {
        public Stock Stock { get; set; }

        
    }
    public class AddStockCommandHandler : IRequestHandler<AddStockCommand,Stock>
        {
            private readonly IStockDbContext _context;
            public AddStockCommandHandler(IStockDbContext context)
            {
                _context = context;
            }
            public async Task<Stock> Handle(AddStockCommand command, CancellationToken cancellationToken)
            {
               // int id = 101;
                //var stockList = await _context.Stocks.FindAsync(x => x.CompanyCode.Equals(command.Stock.CompanyCode)).Result.ToListAsync();
                //if (stockList.Count > 0)
                  //  id = stockList.Max(d => d.StockId) + 1;
               // command.Stock.StockId = id;
               await  _context.Stocks.InsertOneAsync(command.Stock);
               return command.Stock;
                
            }
        }
}
