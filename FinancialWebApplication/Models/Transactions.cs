using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema; 

namespace FinancialWebApplication.Models
{
    [Index(nameof(AccountKey), Name = "IndexAccountKey")] // Index for faster lookups by AccountKey
    public class Transactions
    {
        [Key]
        public int TransactionId { get; set; }

        public string? AccountKey { get; set; } 

        [ForeignKey("AccountKey")]
        public AccountDetails? AccountDetails { get; set; }

        public string? TransactionType { get; set; } // e.g., "Deposit", "Withdrawal"

        [Required]
        [Precision(20, 2)]
        public decimal Amount { get; set; } // Amount of the transaction

        public DateTime TransactionDate { get; set; } // Date and Time of the transactions

        public string Description { get; set; } // Optional description of the transaction

    }
}
