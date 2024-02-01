using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WebRozetka.Constants;
using WebRozetka.Data.Entities;
using WebRozetka.Data.Entities.Identity;
using WebRozetka.Helpers;
using WebRozetka.Interfaces;

namespace WebRozetka.Data
{
    public static class SeederDB
    {
        public static void SeedData(this IApplicationBuilder app)
        {
            using (var scope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                var service = scope.ServiceProvider;
                //Отримую посилання на наш контекст
                var context = service.GetRequiredService<AppEFContext>();
                context.Database.Migrate();

                var userManager = scope.ServiceProvider
                    .GetRequiredService<UserManager<UserEntity>>();

                var roleManager = scope.ServiceProvider
                    .GetRequiredService<RoleManager<RoleEntity>>();

                var novaPoshta = scope.ServiceProvider.GetRequiredService<INovaPoshtaService>();
                if(!context.Areas.Any())
                {
                    novaPoshta.GetAreas();
                }
                

                #region Додавання користувачів та ролей

                if (!context.Roles.Any())
                {
                    foreach (var role in Roles.All)
                    {
                        var result = roleManager.CreateAsync(new RoleEntity
                        {
                            Name = role
                        }).Result;
                    }
                }

                if(!context.Users.Any())
                {
                    var user = new UserEntity
                    {
                        FirstName = "Павло",
                        LastName = "Марко",
                        Email = "admin@gmail.com",
                        UserName = "admin@gmail.com"
                    };
                    var result = userManager.CreateAsync(user,"123456").Result;
                    if(result.Succeeded)
                    {
                        result = userManager.AddToRoleAsync(user, Roles.Admin).Result;
                    }
                }

                #endregion
            }
        }
    }
}
