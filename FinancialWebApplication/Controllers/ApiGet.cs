using FinancialWebApplication.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FinancialWebApplication.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ApiGet : Controller
    {

        private readonly FinancialWebApplicationContext _context;

        public ApiGet(FinancialWebApplicationContext context)
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

        [Authorize]
        [HttpGet("getMonthBudget")]
        public async Task<IActionResult> getMonthBudget([FromQuery] int month)
        {
            var userClaim = User.FindFirst("AccountKey").Value;

            var monthBudget = _context.monthlyBudget.FirstOrDefault(u => u.accountKey == userClaim && u.budgetMonth.Month == month); // gets the budget given the parameter "month"

            // Send user default budget incase no monthly budget has been used for that month
            var accountDetail = _context.AccountDetails.FirstOrDefault(u => u.AccountKey == userClaim);

            return Ok(new { monthBudget, accountDetail });
        }
        public IActionResult Index()
        {
            return View();
        }
    }
}
