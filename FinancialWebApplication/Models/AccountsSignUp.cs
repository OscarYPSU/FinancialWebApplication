using System.ComponentModel.DataAnnotations;

namespace FinancialWebApplication.Models
{
    public class AccountsSignUp
    {
        public int Id { get; set; }
        public string Username { get; set; }

        public string Password { get; set; }

        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        [DataType(DataType.Date)]
        public DateTime AccountCreationDateTime { get; set; }

        public string MaskedPassword => new string('*', Password.Length);
    }
}
