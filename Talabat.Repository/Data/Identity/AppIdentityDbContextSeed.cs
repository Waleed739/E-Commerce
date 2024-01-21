using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities.Identity;

namespace Talabat.Repository.Identity.Data.Identity
{
    public class AppIdentityDbContextSeed
    {
        public static async Task SeedAsync(UserManager<AppUser> userManager)
        {
            if (!userManager.Users.Any())
            {
                var user = new AppUser()
                {
                    DisplayName = "Waleed",
                    Email = "Waleed@gmail.com",
                    UserName = "Waleed",
                    PhoneNumber = "0128493302"

                };
                await userManager.CreateAsync(user,"A@#s12341234");
            }
        }
    }
}
