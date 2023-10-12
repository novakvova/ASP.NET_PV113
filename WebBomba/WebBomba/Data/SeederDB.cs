using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WebBomba.Constants;
using WebBomba.Data.Entities;
using WebBomba.Data.Entities.Identity;
using WebBomba.Interfaces;

namespace WebBomba.Data
{
    public static class SeederDB
    {
        public static void SeedData(this IApplicationBuilder app)
        {
            using(var scope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                var service = scope.ServiceProvider;
                //Отримую посилання на наш контекст
                var context = service.GetRequiredService<DataEFContext>();
                var userManager = service.GetRequiredService<UserManager<UserEntity>>();
                var roleManager = service.GetRequiredService<RoleManager<RoleEntity>>();
                var imageWorker = service.GetRequiredService<IImageWorker>();
                context.Database.Migrate();
                //Якщо категорій в БД немає, то ми їх створимо по default
                if(!context.Categories.Any())
                {
                    
                    var c1 = new CategoryEntity
                    {
                        Name = "Одяг",
                        Description = "Для усіх людей на планеті",
                        Image = imageWorker.ImageSave("https://kasta.ua/imgw/loc/0x0/s3/9/75/29/10986537/32202394/32202394_original.jpeg")
                    };

                    var c2 = new CategoryEntity
                    {
                        Name = "Взуття",
                        Description = "Для дівчат",
                        Image = imageWorker.ImageSave("https://kasta.ua/image/345/s3/supplier_provided_link/feed/9b4/cde/5be/40f/a3f/c20/2c5/ab4/e46.jpeg")
                    };
                    context.Categories.Add(c1);
                    context.Categories.Add(c2);
                    context.SaveChanges();
                }

                if (!context.Roles.Any())
                {
                    var admin = new RoleEntity
                    {
                        Name = Roles.Admin
                    };
                    var roleResult = roleManager.CreateAsync(admin).Result;
                    if(!roleResult.Succeeded)
                    {
                        Console.WriteLine("-----Problem create role {0}-----", Roles.Admin);
                    }

                    var user = new RoleEntity { Name = Roles.User };
                    roleResult = roleManager.CreateAsync(user).Result;
                    if (!roleResult.Succeeded)
                    {
                        Console.WriteLine("-----Problem create role {0}-----", Roles.User);
                    }
                }

                if(!context.Users.Any())
                {
                    UserEntity user = new UserEntity
                    {
                        FirstName = "Валерій",
                        LastName = "Підкаблучник",
                        Email = "admin@gmail.com",
                        UserName = "admin@gmail.com",
                        Image = "admin.webp"
                    };
                    var result = userManager.CreateAsync(user, "123456").Result; 
                    if (!result.Succeeded) 
                    {
                        Console.WriteLine("------Propblem create user {0}-----", user.Email);
                    }
                    else
                    {
                        result = userManager.AddToRoleAsync(user, Roles.Admin).Result;
                        if(!result.Succeeded)
                        {
                            Console.WriteLine("-------Propblem add user {0} role {1}--------", user.Email, Roles.Admin);
                        }
                    }

                }
            }
        }
    }
}
