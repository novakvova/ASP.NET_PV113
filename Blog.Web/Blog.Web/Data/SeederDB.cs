using Blog.Web.Constants;
using Blog.Web.Data.Entities;
using Blog.Web.Data.Entities.Identity;
using Blog.Web.Helpers;
using Bogus;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Blog.Web.Data
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

                if (!context.Users.Any())
                {
                    var user = new UserEntity
                    {
                        FirstName = "Павло",
                        LastName = "Марко",
                        Email = "admin@gmail.com",
                        UserName = "admin@gmail.com"
                    };
                    var result = userManager.CreateAsync(user, "123456").Result;
                    if (result.Succeeded)
                    {
                        result = userManager.AddToRoleAsync(user, Roles.Admin).Result;
                    }
                }

                #endregion

                #region Додаємо категорії

                if (!context.Categories.Any())
                {
                    var fakeCategory = new Faker<CategoryEntity>()
                        .RuleFor(c => c.Name, f => f.Commerce.Categories(1)[0])
                        .RuleFor(c => c.UrlSlug, (f, c) => UrlSlugMaker.GenerateSlug(c.Name))
                        .RuleFor(c => c.Description, f => f.Lorem.Sentence());

                    List<CategoryEntity> fakeCategoryData = fakeCategory.Generate(10);

                    fakeCategoryData = fakeCategoryData
                        .GroupBy(category => category.Name)
                        .Select(group => group.First())
                        .ToList();

                    context.Categories.AddRange(fakeCategoryData);
                    context.SaveChanges();
                }

                #endregion
            }
        }
    }
}
