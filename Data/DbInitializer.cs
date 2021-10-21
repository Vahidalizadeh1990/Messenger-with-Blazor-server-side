
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;
namespace PrivateMessenger.Data
{
    /// <summary>
    /// We use this static class to initialize our identity with some default roles and some default users
    /// We can use this class in all of dot net core and manage our super admin when our project will be run for the first time
    /// </summary>
        public static class UserAndRoleDataInitializer
        {
            public static void SeedData(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
            {
                SeedRoles(roleManager);
                SeedUsers(userManager);
            }

        /// <summary>
        /// Seed our super admin and after that we add a role to this super admin
        /// </summary>
        /// <param name="userManager"></param>
            private static void SeedUsers(UserManager<IdentityUser> userManager)
            {
                if (userManager.FindByEmailAsync("admin@admin.com").Result == null)
                {
                    IdentityUser user = new IdentityUser();
                    user.UserName = "admin@admin.com";
                    user.Email = "admin@admin.com";
                    user.EmailConfirmed = true;
                    IdentityResult result = userManager.CreateAsync(user, "Aa*123456").Result;

                    if (result.Succeeded)
                    {
                        userManager.AddToRoleAsync(user, "Admin").Wait();
                    }
                }

            }
        /// <summary>
        /// Seed our roles (User and Admin) to our identity tables
        /// </summary>
        /// <param name="roleManager"></param>
            private static void SeedRoles(RoleManager<IdentityRole> roleManager)
            {
                if (!roleManager.RoleExistsAsync("User").Result)
                {
                    IdentityRole role = new IdentityRole();
                    role.Name = "User";
                    IdentityResult roleResult = roleManager.
                    CreateAsync(role).Result;
                }


                if (!roleManager.RoleExistsAsync("Admin").Result)
                {
                    IdentityRole role = new IdentityRole();
                    role.Name = "Admin";
                    IdentityResult roleResult = roleManager.
                    CreateAsync(role).Result;
                }
            }
        }
    }