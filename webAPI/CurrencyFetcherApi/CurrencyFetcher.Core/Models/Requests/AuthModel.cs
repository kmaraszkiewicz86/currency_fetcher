using System.ComponentModel.DataAnnotations;

namespace CurrencyFetcher.Core.Models.Requests
{
    public class AuthModel
    {
        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
