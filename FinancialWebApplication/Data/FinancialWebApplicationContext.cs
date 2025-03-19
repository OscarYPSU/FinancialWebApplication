using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using FinancialWebApplication.Models;

namespace FinancialWebApplication.Data
{
    public class FinancialWebApplicationContext : DbContext
    {
        public FinancialWebApplicationContext (DbContextOptions<FinancialWebApplicationContext> options)
            : base(options)
        {
        }

        public DbSet<FinancialWebApplication.Models.Account> Account { get; set; } = default!;
        
        public DbSet<FinancialWebApplication.Models.AccountDetails> AccountDetails { get; set; } = default!;

        public DbSet<FinancialWebApplication.Models.Transactions> Transactions { get; set; } = default!;
    }
}
