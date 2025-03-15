using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using FinancialWebApplication.Models;
using Microsoft.AspNetCore.Authorization;

namespace FinancialWebApplication.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
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
}
