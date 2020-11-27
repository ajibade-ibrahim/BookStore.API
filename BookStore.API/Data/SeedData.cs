using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace BookStore.API.Data
{
    public static class SeedData
    {
        private const string AdminEmail = "admin@bookstore.com";
        private const string Administrator = "Administrator";
        private const string Customer = "Customer";
        private const string CustomerOneEmail = "ibrahim.ajibade@hotmail.com";
        private const string CustomerTwoEmail = "kayvaldo@yahoo.co.uk";
        private const string Password = "P@ssword1";

        public static async Task Seed(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            await SeedRoles(roleManager);
            await SeedUsers(userManager);
        }

        private static async Task SeedRoles(RoleManager<IdentityRole> roleManager)
        {
            if (!await roleManager.RoleExistsAsync(Administrator))
            {
                var role = new IdentityRole
                {
                    Name = Administrator
                };
                await roleManager.CreateAsync(role);
            }

            if (!await roleManager.RoleExistsAsync(Customer))
            {
                var role = new IdentityRole
                {
                    Name = Customer
                };
                await roleManager.CreateAsync(role);
            }
        }

        private static async Task SeedUsers(UserManager<IdentityUser> userManager)
        {
            if (await userManager.FindByEmailAsync(AdminEmail) == null)
            {
                var user = new IdentityUser
                {
                    UserName = "admin",
                    Email = AdminEmail
                };
                var result = await userManager.CreateAsync(user, Password);

                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, Administrator);
                }
            }

            if (await userManager.FindByEmailAsync(CustomerOneEmail) == null)
            {
                var user = new IdentityUser
                {
                    UserName = "ibrahim",
                    Email = CustomerOneEmail
                };
                var result = await userManager.CreateAsync(user, Password);

                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, Customer);
                }
            }

            if (await userManager.FindByEmailAsync(CustomerTwoEmail) == null)
            {
                var user = new IdentityUser
                {
                    UserName = "kayvaldo",
                    Email = CustomerTwoEmail
                };
                var result = await userManager.CreateAsync(user, Password);

                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, Customer);
                }
            }
        }
    }
}