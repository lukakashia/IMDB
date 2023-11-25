using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace IMDB.AuthorizationFields
{
    public class Userr
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public byte[] PasswordHash { get; set; } = new byte[32];
        public byte[] PasswordSalt { get; set; } = new byte[32];
        public string? VerificationToken { get; set; }
        public DateTime VerifiedAt { get; set; }
        public string? PasswordResetToken { get; set; }
        public DateTime? ResetTokenExpires { get; set; }
        public Role Role { get; set; }
    }
}
