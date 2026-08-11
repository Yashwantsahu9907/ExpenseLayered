using Microsoft.AspNetCore.Identity;

namespace ExpenseLayeredApi.Entities.Identity
{
    public class AppRole : IdentityRole<int>
    {
        public ICollection<AppUserRole> UserRoles { get; set; }  // ek role ke sath multiple userRole Ho sakte hai 
    }
}
