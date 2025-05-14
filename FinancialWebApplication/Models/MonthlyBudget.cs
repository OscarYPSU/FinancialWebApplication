using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace FinancialWebApplication.Models
{
    public class MonthlyBudget
    {
        [Key]
        public int Id { get; set; }
        
        public string accountKey { get; set; }

        public DateOnly budgetMonth { get; set; }

        [Precision(20, 2)]
        public decimal AccountBudget { get; set; } // Default budget
    }
}
