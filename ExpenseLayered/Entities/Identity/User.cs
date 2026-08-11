using Microsoft.AspNetCore.Identity;

namespace ExpenseLayeredApi.Entities.Identity
{
    public class User : IdentityUser<int>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        public string Gender { get; set; }
        public ICollection<AppUserRole> UserRoles { get; set; }  // one user has multiple role
    }
}