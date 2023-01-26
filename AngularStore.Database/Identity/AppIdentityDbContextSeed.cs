using AngularStore.Core.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AngularStore.Database.Identity
{
    public class AppIdentityDbContextSeed
    {
        public static async Task SeedUserAsync(UserManager<AppUser> userManager)
        {
            if (!userManager.Users.Any())
            {
                var user = new AppUser
                {
                    DisplayName = "Viktor",
                    Email = "viktar.varabei@mail.ru",
                    UserName = "viktar.varabei@mail.ru",
                    Address = new Address
                    {
                        FirstName = "Viktor",
                        LastName = "Vorobei",
                        Street = "Sharipova street, 33",
                        City = "Grodno",
                        State = "Grodno",
                        ZipCode = "230028"
                    }
                };

                await userManager.CreateAsync(user,"1919101976$aAa$");
            }
        }
    }
}
