using Microsoft.VisualBasic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace FinancialWebApplication.Models
{
    public class Account
    {
        public int Id { get; set; }
        public string Username { get; set; }

        [JsonIgnore]
        public string Password { get; set; }

        [DataType(DataType.Date)]
        public DateTime AccountCreationDateTime { get; set; }

        public string MaskedPassword => new string('*', Password.Length);

        public string AccountKey { get; set; } = Guid.NewGuid().ToString();

    }
}
