namespace CurrencyFetcher.Core.Models.Requests
{
    /// <summary>
    /// User model data
    /// </summary>
    public class UserModel
    {
        /// <summary>
        /// The email
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// The username
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// The password
        /// </summary>
        public string Password { get; set; }
    }
}