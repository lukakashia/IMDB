using System.ComponentModel.DataAnnotations;

namespace IMDB.Admin
{
    public class AdminLoginRequest : AdminRequest
    {
        [Required, EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
