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
                    new Claim(ClaimTypes.Name, UserDetail.FirstName),
                    new Claim("lastName", UserDetail.LastName),
                    new Claim("AccountBudget", UserDetail.AccountBudget.ToString()),
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

                // Everytime user logs in, check if their account budget needs to be monthly resetted
                if (UserDetail.LastUpdated.Month != DateTime.Now.Month)
                {
                    UserDetail.AccountBudget = 250; // Reset budget to default
                    UserDetail.LastUpdated = DateOnly.FromDateTime(DateTime.Now); // Update last updated date
                    _context.Update(UserDetail);
                    await _context.SaveChangesAsync();
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
