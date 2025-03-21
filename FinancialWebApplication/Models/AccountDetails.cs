using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace FinancialWebApplication.Models
{
    public class AccountDetails
    {
        [Key]
        public string AccountKey { get; set; }

        [Required(ErrorMessage = "First name is required.")]
        public string? FirstName { get; set; }

        [Required(ErrorMessage = "Last name is required.")]
        public string? LastName { get; set; }

        [Precision(20, 2)]
        public decimal AccountBudget { get; set; } = 250; // Default budget

        public ICollection<Transactions>? Transactions { get; set; }

        public DateOnly LastUpdated { get; set; } = DateOnly.FromDateTime(DateTime.Now); // Default to today

    }
}
