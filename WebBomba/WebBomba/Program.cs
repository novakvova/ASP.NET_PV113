using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using WebBomba.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<DataEFContext>(opt =>
    opt.UseSqlServer(builder.Configuration.GetConnectionString("MSSQLConnection")));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

var dir = Path.Combine(Directory.GetCurrentDirectory(), "images");
if(!Directory.Exists(dir))
{ 
    Directory.CreateDirectory(dir); 
}
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(dir),
    RequestPath = "/images"
});



app.UseRouting();

app.UseAuthorization();

//Налаштуваня маршрутизації - якою адресою в url - ми отрмуємо доступ до методів контролера
//http://localhost:5890 - корінь сайту - головна сторінка - /

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Category}/{action=Index}/{id?}");

//Початкова ініціалізація Бази даних
app.SeedData();

app.Run();
