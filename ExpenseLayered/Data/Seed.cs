using ExpenseLayeredApi.Constant;
using ExpenseLayeredApi.Entities.Identity;
using Microsoft.AspNetCore.Identity;

namespace ExpenseLayeredApi.Data
{
    public class Seed
    {
        public static async Task SeedDataAsync(UserManager<User> userManager, RoleManager<AppRole> roleManager, 
            IConfiguration configuration)
        {
            var roles = new List<string>
            {
                RoleConstant.SuperAdmin,
                RoleConstant.Admin,
                RoleConstant.User
            };

            foreach(var role in roles)
            {
                if(!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new AppRole
                    {
                        Name = role
                    });
                }
            }
            string superAdminEmail = configuration["SuperAdmin:Email"];
            string superAdminPassword = configuration["SuperAdmin:Password"];
            string firstName = configuration["SuperAdmin:FirstName"];
            string lastName = configuration["SuperAdmin:LastName"];
            string gender = configuration["SuperAdmin:Gender"];
            if(string.IsNullOrWhiteSpace(superAdminEmail) ||
                string.IsNullOrWhiteSpace(superAdminPassword))
            {
                return;
            }
            var existingSuperAdmin = await userManager.FindByEmailAsync(superAdminEmail);
            if(existingSuperAdmin != null)
            {
                return;
            }
            var superAdmin = new User
            {
                FirstName = firstName ?? "Yash",
                LastName = lastName ?? "Sahu",
                Email = superAdminEmail,
                UserName = superAdminEmail,
                Gender = gender ?? "Male",
                EmailConfirmed = true,
            };
            var result = await userManager.CreateAsync(superAdmin, superAdminPassword);
            if(result.Succeeded)
            {
                await userManager.AddToRoleAsync(superAdmin, RoleConstant.SuperAdmin);
            }
        }
    }
}
