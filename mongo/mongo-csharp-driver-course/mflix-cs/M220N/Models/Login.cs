using System.ComponentModel.DataAnnotations;

namespace M220N.Models
{
    public class Login
    {
        [EmailAddress]
        private string Email { get; set; }

        [MinLength(8)]
        private string Password { get; set; }
    }
}
