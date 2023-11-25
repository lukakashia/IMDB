using System.ComponentModel.DataAnnotations;

namespace IMDB.AuthorizationFields
{
    public class ResetPasswordRequest
    {
        [Required]
        public string Token { get; set; }

        [Required, MinLength(6, ErrorMessage = "Please enter at least 6 characters!")]
        public string Password { get; set; }

        [Required, Compare("Password", ErrorMessage = "Password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }
}
