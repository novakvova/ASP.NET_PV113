using Bogus;
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
                if (!context.Areas.Any())
                {
                    novaPoshta.GetAreas();
                }

                if (!context.Settlements.Any())
                {
                    novaPoshta.GetSettlements();
                }

                if (!context.Warehouses.Any())
                {
                    novaPoshta.GetWarehouses();
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

                #region Додавання категорій

                if (context.Categories.Count() < 10)
                {
                    var fakeCategory = new Faker<CategoryEntity>("uk")
                                        .RuleFor(o => o.IsDeleted, f => false)
                                        .RuleFor(o => o.DateCreated, f => DateTime.UtcNow)
                                        .RuleFor(o => o.Name, f => f.Commerce.Categories(1)[0]);
                    var list = fakeCategory.Generate(10);

                    foreach(var category in list)
                    {
                        category.Image = ImageWorker.SaveImageFromUrlAsync("https://loremflickr.com/800/600").Result;
                        context.Categories.Add(category);
                        context.SaveChanges();
                    }

                }

                #endregion


                #region Додавання товарів

                if (context.Products.Count() < 200)
                {
                    Faker faker = new Faker();

                    var categoriesId = context.Categories.Where(c => !c.IsDeleted).Select(c => c.Id).ToList();

                    var fakeProduct = new Faker<ProductEntity>("uk")
                         .RuleFor(o => o.IsDeleted, f => false)
                         .RuleFor(o => o.DateCreated, f => DateTime.UtcNow)
                         .RuleFor(o => o.Name, f => f.Commerce.ProductName())
                         .RuleFor(o => o.Price, f => Math.Round(f.Random.Decimal(1, 1000), 2))
                         .RuleFor(o => o.Description, f => f.Lorem.Paragraph())
                         .RuleFor(o => o.Quantity, f => f.Random.Number(0, 1000))
                         .RuleFor(o => o.CategoryId, f => f.PickRandom(categoriesId));

                    var fakeProducts = fakeProduct.Generate(100);

                    context.Products.AddRange(fakeProducts);
                    context.SaveChanges();

                    var photos = new List<ProductImageEntity>();

                    foreach (var product in fakeProducts)
                    {
                        var numberOfPhotos = faker.Random.Number(1, 3);

                        for (int i = 0; i < numberOfPhotos; i++)
                        {
                            var fakeImage = ImageWorker.SaveImageFromUrlAsync("https://loremflickr.com/800/600").Result;
                            photos.Add(new ProductImageEntity
                            {
                                Name = fakeImage,
                                ProductId = product.Id,
                                Priority = (byte)(i + 1)
                            });
                        }
                    }
                    context.AddRange(photos);
                    context.SaveChanges();
                }
            }

            #endregion
        }

    }
}
