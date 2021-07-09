using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace StocksAPI.DTO
{
    public class StockDto
    {
        [RegularExpression(@"(^[0-9]+[.][0-9]{2,}$)",
 ErrorMessage = "Stock price must be a fractional value.")]
        public decimal StockPrice { get; set; }

    }
}
