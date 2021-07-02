using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CompaniesAPI.Models
{
    public class Stock
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public int StockId { get; set; }
        [Column(TypeName = "decimal(18,4)")]
        public decimal StockPrice { get; set; }

        public DateTime StockDateTime { get; set; } = DateTime.Now;
        public string CompanyCode { get; set; }

        public virtual Company Company { get; set; }
    }
}
