using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using System;
using WebBomba.Data;
using WebBomba.Data.Entities.Identity;
using WebBomba.Interfaces;
using WebBomba.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddScoped<IImageWorker, ImageWorker>();

builder.Services.AddDbContext<DataEFContext>(opt =>
    opt.UseSqlServer(builder.Configuration.GetConnectionString("MSSQLConnection")));

builder.Services.Configure<CookiePolicyOptions>(options =>
{
    options.CheckConsentNeeded = context => true;
    options.MinimumSameSitePolicy = SameSiteMode.None;
});

builder.Services.AddIdentity<UserEntity, RoleEntity>(options =>
{
    options.Stores.MaxLengthForKeys = 128;
    options.Password.RequireDigit = false;
    options.Password.RequiredLength = 5;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireLowercase = false;
})
                .AddEntityFrameworkStores<DataEFContext>()
                .AddDefaultTokenProviders();

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

app.UseCookiePolicy();
app.UseAuthentication();
app.UseAuthorization();

//Налаштуваня маршрутизації - якою адресою в url - ми отрмуємо доступ до методів контролера
//http://localhost:5890 - корінь сайту - головна сторінка - /

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Category}/{action=Index}/{id?}");

//Початкова ініціалізація Бази даних
app.SeedData();

app.Run();
