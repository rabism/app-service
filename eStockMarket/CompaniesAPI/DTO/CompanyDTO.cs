using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CompaniesAPI.DTO
{
    public class CompanyDTO
    {
        [Required]
        public string CompanyCode { get; set; }
        [Required]
        public string CompanyName { get; set; }
        [Required]
        public string CompanyCEO { get; set; }
        [Required]
        [Range(10, int.MaxValue, ErrorMessage = "Company Turnover must be greater than 10Cr.")]
        public int CompanyTurnOver { get; set; }
        [Required]
        public string Website { get; set; }
        [Required]
        [RegularExpression(@"(^[0-9]+[.][0-9]{2,}$)",
         ErrorMessage = "Stock price must be a fractional value.")]
        public decimal StockPrice { get; set; }

    }
}
