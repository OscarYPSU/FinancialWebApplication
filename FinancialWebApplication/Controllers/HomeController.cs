using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using FinancialWebApplication.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using FinancialWebApplication.Data;
using FinancialWebApplication.Migrations;

namespace FinancialWebApplication.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly FinancialWebApplicationContext _context;
    public HomeController(FinancialWebApplicationContext context, ILogger<HomeController> logger)
    {
        _context = context;
        _logger = logger;
    }

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

    [Authorize]
    public IActionResult LoggedHome()
    {
        var FirstName = User.Identity.Name;
        var LastName = User.FindFirst("lastName").Value;
        var AccountBudget = User.FindFirst("AccountBudget").Value;

        ViewData["FirstName"] = FirstName;
        ViewData["LastName"] = LastName;
        ViewData["AccountBudget"] = AccountBudget;

        return View();
    }

    [Authorize]
    public IActionResult transactions()
    {
        return View();
    }

    [Authorize]
    [HttpPost]
    public IActionResult transactions(Transactions Model)
    {
        var accountKeyClaim = User.FindFirst("AccountKey").Value;
        var AccountDetails = _context.AccountDetails.FirstOrDefault(a => a.AccountKey == accountKeyClaim);


        var NewTransaction = new Transactions
        {
            AccountKey = accountKeyClaim,
            AccountDetails = AccountDetails, // link to the account details
            TransactionType = Model.TransactionType,
            Amount = Model.Amount,
            TransactionDate = DateTime.Now,
            Description = Model.Description
        };

        _context.Transactions.Add(NewTransaction); // adds the transaction to the database
        _context.SaveChanges();

        return RedirectToAction("Transactions", "Home"); // redirects to the home page after transaction
    }



    [Authorize]
    public async Task<IActionResult> LogOut()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

        return RedirectToAction("Index", "Login");
    }
}
