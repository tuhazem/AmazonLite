using Microsoft.AspNetCore.Identity;

namespace Amazon.Domain.Entities
{
    public class ApplicationUser : IdentityUser
    {
        public string? FullName { get; set; }
    }
}
