using Microsoft.AspNetCore.Identity;

namespace ExpenseLayeredApi.Entities.Identity
{
    public class AppUserRole : IdentityUserRole<int>
    {
        public User User { get; set; }
        public AppRole Role { get; set; }
    }
}
