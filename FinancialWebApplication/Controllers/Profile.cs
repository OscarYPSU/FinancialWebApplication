using Microsoft.AspNetCore.Mvc;
using FinancialWebApplication.Data;
using Microsoft.AspNetCore.Authorization;
using FinancialWebApplication.Models;
using Microsoft.EntityFrameworkCore.Query;

namespace FinancialWebApplication.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class Profile : Controller
    {

        private readonly FinancialWebApplicationContext _context;

        public Profile(FinancialWebApplicationContext context)
        {
            _context = context;
        }

        public IActionResult Setting()
        {
            var userAccountClaim = User.FindFirst("AccountKey").Value;
            var accountDetails = _context.AccountDetails.FirstOrDefault(u => u.AccountKey == userAccountClaim);

            TempData["firstName"] = accountDetails.FirstName;
            TempData["lastName"] = accountDetails.LastName;
            TempData["defaultBudget"] = accountDetails.defaultBudget;
            TempData["ProfileImagePath"] = accountDetails.ProfileImagePath;

            return View();
        }

        // Changes user's name
        [Authorize]
        [HttpPost("changeName")]
        public async Task<IActionResult> changeName([FromQuery] string firstName, [FromQuery]string lastName)
        {
            var userAccountClaim = User.FindFirst("AccountKey").Value;
            var accountDetails = _context.AccountDetails.FirstOrDefault(u => u.AccountKey == userAccountClaim);

            if(firstName != accountDetails.FirstName)
            {
                accountDetails.FirstName = firstName;
            }
            if(lastName != accountDetails.LastName) 
            {
                accountDetails.LastName = lastName;
            }

            _context.SaveChanges();

            return Ok(accountDetails);
        }

        // Changes the default budget for each month(Does not override the existing month, only for future months)
        [Authorize]
        [HttpPost("changeBudget")]
        public async Task<IActionResult> changeBudget([FromQuery] decimal newBudget)
        {
            // loads account detail from database
            var userAccountClaim = User.FindFirst("AccountKey").Value;
            var accountDetails = _context.AccountDetails.FirstOrDefault(u => u.AccountKey == userAccountClaim);
            accountDetails.defaultBudget = newBudget;

            _context.SaveChanges();

            return Ok(accountDetails);
        }

        // Changes the budget for specifc months
        [Authorize]
        [HttpPost("ChangeBudgetForSpecificMonth")]
        public async Task<IActionResult> changeBudgetSpecifcMonth([FromQuery] decimal newBudget, [FromQuery] DateOnly userDate)
        {
            // loads account detail from database
            var userAccountClaim = User.FindFirst("AccountKey").Value;

            var specificMonth = _context.monthlyBudget.FirstOrDefault(u => u.accountKey == userAccountClaim && u.budgetMonth.Month == userDate.Month && u.budgetMonth.Year == userDate.Year);
            // if there is existing data for the month already
            if (specificMonth != null)
            {
                /// Need to change logic by adding up total amount from existing transaction of this month  
                var transactionsInMonth = _context.Transactions.Where(u => u.AccountKey == userAccountClaim && u.TransactionDate.Month == userDate.Month && u.TransactionDate.Year == userDate.Year).ToList(); // gets a list of the rows of datas from transactions of the corresponding month and year
                decimal totalAmount = 0;  // storing total amount of transaction of target month
                foreach (var transaction in transactionsInMonth)
                {
                    totalAmount += transaction.Amount;
                }

                // existing transaction from new budget amount
                var newMonthBudget = newBudget - totalAmount;
                specificMonth.AccountBudget = newMonthBudget;
            } else
            {
                specificMonth = new MonthlyBudget
                {
                    accountKey = userAccountClaim,
                    budgetMonth = userDate,
                    AccountBudget = newBudget
                };

                _context.monthlyBudget.Add(specificMonth);
            }
                _context.SaveChanges();
                return Ok(specificMonth);
        }



        // Changes the profile image
        [Authorize]
        [HttpPost("ChangeProfileImage")]
        public async Task<IActionResult> updateImage([FromForm] IFormFile profileImage)
        {
            if (profileImage == null || profileImage.Length == 0)
            {
                return BadRequest("No image is loaded");
            }
            // loads account detail from database
            var userAccountClaim = User.FindFirst("AccountKey").Value;
            var accountDetails = _context.AccountDetails.FirstOrDefault(u => u.AccountKey == userAccountClaim);

            // sets up the route to the folder where the image will be saved
            var imageFolderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images");

            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(profileImage.FileName); // Random generated url path
            var savePath = Path.Combine(imageFolderPath, fileName); // combines the random url path with the path of saving images

            // Saves the image to the disk
            using (var stream = new FileStream(savePath, FileMode.Create))
            {
                await profileImage.CopyToAsync(stream);
            }

            // Deletes the old image from the disk

            var oldImagePathTrimmed = accountDetails.ProfileImagePath.TrimStart('/'); // need to trim the starting '/' first from /images/###### to work properly
            oldImagePathTrimmed = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", oldImagePathTrimmed);
            if (System.IO.File.Exists(oldImagePathTrimmed))
            {
                System.IO.File.Delete(oldImagePathTrimmed);
            }

            // Saves the new path to user's database
            accountDetails.ProfileImagePath = "/images/" + fileName;
           
            _context.SaveChanges();

            return RedirectToAction("Profile", "Setting");
        }


        [Authorize]
        [HttpPost]
        public IActionResult changeSetting(int newMonthlyBudget)
        {

            return View();
        }
        


    }
}
