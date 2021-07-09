using MediatR;
using StocksAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MongoDB.Driver;

namespace StocksAPI.Features.StockFeatures.Queries
{
    public class GetStockByCompanyCodeQuery : IRequest<IReadOnlyList<Stock>>
    {
        public string CompanyCode { get; set; }
        
    }

    public class GetStockByCompanyCodeQueryHandler : IRequestHandler<GetStockByCompanyCodeQuery, IReadOnlyList<Stock>>
        {
            private readonly IStockDbContext _context;
            public GetStockByCompanyCodeQueryHandler(IStockDbContext context)
            {
                _context = context;
            }
            public async Task<IReadOnlyList<Stock>> Handle(GetStockByCompanyCodeQuery query, CancellationToken cancellationToken)
            {
                return await _context.Stocks.FindAsync(x => x.CompanyCode.Equals(query.CompanyCode)).Result.ToListAsync();
            }
        }
}
