using CompaniesAPI.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CompaniesAPI.DBContexts
{

    public class CompanyDBContext : DbContext
    {
        public CompanyDBContext(DbContextOptions<CompanyDBContext> options) : base(options)
        {
            Database.EnsureCreated();
        }
        public DbSet<Company> Companies { get; set; }
        public DbSet<Stock> Stocks { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Stock>()
                .HasOne<Company>(s => s.Company)
                .WithMany(g => g.Stocks)
                .HasForeignKey(s => s.CompanyCode)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Company>().HasData(new Company {
                CompanyCode = "Code1",
                CompanyName = "ABC",
                CompanyCEO = "XYZ",
                CompanyTurnOver = 100,
                Website = "website1"
            });
            modelBuilder.Entity<Stock>().HasData(
                new Stock {
                    StockId = 101,
                    StockDateTime = DateTime.Now,
                    StockPrice = 100.89m,
                    CompanyCode = "Code1"
                });
        }

    }
}
