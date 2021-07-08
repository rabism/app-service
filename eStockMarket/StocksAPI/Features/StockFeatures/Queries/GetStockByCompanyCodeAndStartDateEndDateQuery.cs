using MediatR;
using MongoDB.Driver;
using StocksAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace StocksAPI.Features.StockFeatures.Queries
{
    public class GetStockByCompanyCodeAndStartDateEndDateQuery : IRequest<IReadOnlyList<Stock>>
    {
        public string CompanyCode { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public class GetStockByCompanyCodeAndStartDateEndDateQueryHandler : IRequestHandler<GetStockByCompanyCodeAndStartDateEndDateQuery, IReadOnlyList<Stock>>
        {
            readonly IStockDbContext _context;
            public GetStockByCompanyCodeAndStartDateEndDateQueryHandler(IStockDbContext context)
            {
                _context = context;
            }
            public async Task<IReadOnlyList<Stock>> Handle(GetStockByCompanyCodeAndStartDateEndDateQuery query, CancellationToken cancellationToken)
            {
                return await _context.Stocks.FindAsync(x => x.CompanyCode.Equals(query.CompanyCode) && x.StockDateTime > query.StartDate && x.StockDateTime < query.EndDate).Result.ToListAsync();
            }
        }
    }
}
