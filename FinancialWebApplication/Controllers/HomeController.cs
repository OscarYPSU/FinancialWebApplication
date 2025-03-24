using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using FinancialWebApplication.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using FinancialWebApplication.Data;
using FinancialWebApplication.Migrations;
using System.Transactions;

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
        var AccountKey = User.FindFirst("AccountKey").Value;

        var user = _context.AccountDetails.Where(s => s.AccountKey == AccountKey).FirstOrDefault();

        TempData["FirstName"] = user.FirstName;
        TempData["LastName"] = user.LastName;
        TempData["AccountBudget"] = user.AccountBudget;

        var datas = _context.Transactions.Where(s => s.AccountKey == User.FindFirst("AccountKey").Value).ToList();
        return View(datas);
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
        var Account = _context.AccountDetails.FirstOrDefault(a => a.AccountKey == accountKeyClaim);


        var NewTransaction = new Transactions
        {
            AccountKey = accountKeyClaim,
            AccountDetails = Account, // link to the account details
            TransactionType = Model.TransactionType,
            Amount = Model.Amount,
            TransactionDate = DateTime.Now,
            Description = Model.Description
        };

        Account.AccountBudget -= Model.Amount; // updates the account budget after transaction
        _context.Transactions.Add(NewTransaction); // adds the transaction to the database
        _context.SaveChanges();

        TempData["AccountBudget"] = Account.AccountBudget; // updates the view data with the new budget
        TempData["FirstName"] = Account.FirstName;
        TempData["LastName"] = Account.LastName;

        var datas = _context.Transactions.Where(s => s.AccountKey == User.FindFirst("AccountKey").Value && s.TransactionDate.Month == DateTime.Now.Month).ToList();

        return View("LoggedHome", datas); // redirects to the home page after transaction
    }

    [Authorize]
    public IActionResult Delete(int TransactionId)
    {
        var transaction = _context.Transactions.FirstOrDefault(t => t.TransactionId == TransactionId);
        if (transaction != null)
        {
            var AccountKey = User.FindFirst("AccountKey").Value;

            var Account = _context.AccountDetails.FirstOrDefault(a => a.AccountKey == AccountKey);
            
            Account.AccountBudget += transaction.Amount; // updates the account budget before deleting the transaction

            _context.Transactions.Remove(transaction); // removes the transaction from the database

            _context.SaveChanges();
        }      
        return RedirectToAction("LoggedHome");
    }

    [Authorize]
    public IActionResult Edit(int TransactionId)
    {
        var Transaction = _context.Transactions.FirstOrDefault(Transaction => Transaction.TransactionId == TransactionId);

        if (Transaction == null)
        {
            return NotFound();
        }

        return View(Transaction);
    }

    [Authorize]
    [HttpPost]
    public IActionResult Edit(int TransactionId, int TransactionAmount, string Description)
    {
        var transaction = _context.Transactions.FirstOrDefault(t => t.TransactionId == TransactionId);
        if (transaction == null)
        {
            return NotFound();
        } else
        {
            var AccountKey = User.FindFirst("AccountKey").Value;
            var Account = _context.AccountDetails.FirstOrDefault(a => a.AccountKey == AccountKey);

            Account.AccountBudget += transaction.Amount; // updates the account budget before editing the transaction
            Account.AccountBudget -= TransactionAmount;

            transaction.Amount = TransactionAmount; // updates the transaction amount
            transaction.Description = Description; // updates the transaction description
            _context.SaveChanges();
        }
            return RedirectToAction("LoggedHome"); // Redirect to list after saving
    }


    [Authorize]
    public async Task<IActionResult> LogOut()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

        return RedirectToAction("Index", "Login");
    }

}
