using Microsoft.Extensions.Diagnostics.HealthChecks;
using System.ComponentModel.DataAnnotations;

namespace IMDB.AuthorizationFields
{
    public class RegisterModel
    {
        public int Id { get; set; }


        [Required, EmailAddress]
        public string Email { get; set; }

        [Required, MinLength(6, ErrorMessage = "Please enter at least 6 characters!")]
        public string Password { get; set; }

        [Required, Compare("Password", ErrorMessage = "Password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }
}
