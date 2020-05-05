using System.ComponentModel.DataAnnotations;

namespace CurrencyFetcher.Core.Models.Requests
{
    /// <summary>
    /// Data for authenticates user
    /// </summary>
    public class TokenAuthRequest
    {
        /// <summary>
        /// The username
        /// </summary>
        [Required]
        public string Username { get; set; }

        /// <summary>
        /// The password
        /// </summary>
        [Required]
        public string Password { get; set; }

        /// <summary>
        /// Generate string from class properties
        /// </summary>
        /// <returns>The string from class properties</returns>
        public override string ToString()
        {
            return $"Username => {Username}, Password => ******";
        }
    }
}
