using Microsoft.AspNetCore.Mvc;
using FinancialWebApplication.Data;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using FinancialWebApplication.Models;
using Microsoft.AspNetCore.Authorization;

namespace FinancialWebApplication.Controllers
{
    public class Login : Controller
    {
        // context for database
        private readonly FinancialWebApplicationContext _context;

        public Login(FinancialWebApplicationContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        // POST: Login
        [HttpPost]
        public async Task<IActionResult> Index(Account Model)
        {
            if (!ModelState.IsValid)
            {
                return View(Model);
            }

            // Queues the query and find matching user in the database
            var user = _context.Account.FirstOrDefault(u => u.Username == Model.Username && u.Password == Model.Password);

            if (user != null)
            {
                var UserDetail = _context.AccountDetails.FirstOrDefault(u => u.AccountKey == user.AccountKey);

                // Sets up the claims for the authenticated user
                var claims = new List<Claim>
                {
                    new Claim("AccountKey", user.AccountKey),
                    // find out how to utilize roles
                    // new Claim(ClaimTypes.Role, "Administrator"),
                };

                var ClaimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var AuthProperties = new AuthenticationProperties
                {
                    IsPersistent = true, // The cookie will persist across browser sessions.
                    ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(30)
                };
                await HttpContext.SignInAsync(
                   CookieAuthenticationDefaults.AuthenticationScheme,
                   new ClaimsPrincipal(ClaimsIdentity),
                   AuthProperties);

                // Everytime user logs in, check if there is a current monthly budget, else add a new one to the table of MonthlyBudget
                var monthlyBudget = _context.monthlyBudget.FirstOrDefault(u => u.accountKey == user.AccountKey && DateTime.Now.Month == u.budgetMonth.Month);

                if (monthlyBudget == null)
                {
                    var newMonthlyBudget = new MonthlyBudget
                    {
                        accountKey = user.AccountKey,
                        budgetMonth = DateOnly.FromDateTime(DateTime.Now.Date),
                        AccountBudget = UserDetail.defaultBudget
                    };

                    _context.monthlyBudget.Add(newMonthlyBudget);
                    _context.SaveChanges();
                }

                return RedirectToAction("LoggedHome", "Home");
            } else
            {
                // Wrong password and username combination
                ModelState.AddModelError(string.Empty, "Wrong User/Password combination please try again!");
                return View(Model);

            }
        }
    }
}
