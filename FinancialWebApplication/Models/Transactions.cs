﻿using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FinancialWebApplication.Models
{
    [Index(nameof(AccountKey), Name = "IndexAccountKey")] // Index for faster lookups by AccountKey
    public class Transactions
    {
        [Key]
        public int TransactionId { get; set; }

        [Required]
        public string AccountKey { get; set; }

        [ForeignKey("AccountKey")]
        public AccountDetails AccountDetails { get; set; }

        [Required]
        public string TransactionType { get; set; } // e.g., "Deposit", "Withdrawal"

        [Required]
        [Precision(20, 2)]
        public decimal Amount { get; set; } // Amount of the transaction

        [Required]
        public DateTime TransactionDate { get; set; } = DateTime.Now; // Default to current date and time

        public string Description { get; set; } // Optional description of the transaction

    }
}
