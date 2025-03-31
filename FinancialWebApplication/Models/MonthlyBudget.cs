using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace FinancialWebApplication.Models
{
    public class MonthlyBudget
    {
        [Key]
        public int Id { get; set; }
        
        public string accountKey { get; set; }

        public DateTime budgetMonth { get; set; }

        [Precision(20, 2)]
        public decimal AccountBudget { get; set; } = 250; // Default budget
    }
}
