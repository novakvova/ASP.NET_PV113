using Microsoft.EntityFrameworkCore;
using WebBomba.Data.Entities;

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
                context.Database.Migrate();
                //Якщо категорій в БД немає, то ми їх створимо по default
                if(!context.Categories.Any())
                {
                    var c1 = new CategoryEntity
                    {
                        Name = "Одяг",
                        Description = "Для усіх людей на планеті",
                        Image = "https://kasta.ua/imgw/loc/0x0/s3/9/75/29/10986537/32202394/32202394_original.jpeg"
                    };

                    var c2 = new CategoryEntity
                    {
                        Name = "Взуття",
                        Description = "Для дівчат",
                        Image = "https://kasta.ua/image/345/s3/supplier_provided_link/feed/9b4/cde/5be/40f/a3f/c20/2c5/ab4/e46.jpeg"
                    };
                    context.Categories.Add(c1);
                    context.Categories.Add(c2);
                    context.SaveChanges();
                }
            }
        }
    }
}
