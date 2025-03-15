using FinancialWebApplication.Data;
using FinancialWebApplication.Models;
using Microsoft.AspNetCore.Mvc;


namespace FinancialWebApplication.Controllers
{
    public class SignUp : Controller
    {
        private readonly FinancialWebApplicationContext _context;
        public SignUp(FinancialWebApplicationContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Index([Bind("Id,Username,ConfirmPassword,Password")] AccountsSignUp account)
        {

            // Here you would typically save the new account to the database
            if (ModelState.IsValid)
            {
                // If the account is valid, create a new account object
                var SignUpAccount = new Account
                {
                    Username = account.Username,
                    Password = account.Password,
                    AccountCreationDateTime = DateTime.Now
                };

                _context.Account.Add(SignUpAccount);// adds the account to the database
                _context.SaveChanges();
                
                // redirects to home page, ideally to a login page after implementation
                return RedirectToAction("AccountDetails", "SignUp", new {AccountKey = SignUpAccount.AccountKey});
            }

            // 
            return View();
        }

        public IActionResult AccountDetails(string AccountKey)
        {
            var AccountDetail = new AccountDetails
            {
                AccountKey = AccountKey
            };
            return View(AccountDetail);
        }

        [HttpPost]
        public IActionResult AccountDetails(AccountDetails model)
        {
            // Here you would typically save the new account to the database
            if (ModelState.IsValid)
            {
                // If the account is valid, create a new account object
                var AccountDetail = new AccountDetails
                {
                    AccountKey = model.AccountKey,
                    FirstName = model.FirstName,
                    LastName = model.LastName
                };

                _context.AccountDetails.Add(AccountDetail);// adds the account to the database
                _context.SaveChanges();

                // redirects to home page, ideally to a login page after implementation
                return RedirectToAction("Index", "Home");
            }

            return View();
        }
    }
}
