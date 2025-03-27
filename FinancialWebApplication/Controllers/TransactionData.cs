using FinancialWebApplication.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace FinancialWebApplication.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TransactionData : Controller
    {
        private readonly FinancialWebApplicationContext _context;

        public TransactionData(FinancialWebApplicationContext context)
        {
            _context = context;
        }

        [Authorize]
        [HttpGet("TransactionsData")]
        public async Task<IActionResult> GetTransaction([FromQuery] int Month, [FromQuery] int Year)
        {
            var user = User.FindFirst("AccountKey").Value;

            // Can improve query by using range of data so its index friendly
            var transactionData = _context.Transactions.Where(s => s.AccountKey == user && s.TransactionDate.Year == Year && s.TransactionDate.Month == Month);

            return Ok(transactionData);
        }

    }
}
