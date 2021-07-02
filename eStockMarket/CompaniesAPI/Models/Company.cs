using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CompaniesAPI.Models
{
    public class Company
    {
        [Key]
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
        //public int StockId { get; set; }


        public virtual ICollection<Stock> Stocks { get; set; }

    }
}
