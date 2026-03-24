using Microsoft.AspNetCore.Identity;

namespace PharmaFlow.Models
{
    public class User : IdentityUser
    {
        // Custom fields (extend later if needed)
        public string Role { get; set; } = "User";

        // Optional (future-proofing 🔥)
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}