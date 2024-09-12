using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ECommerce.Repositories.Context; // Data klasöründeki ECommerceDbContext sýnýfýnýn olduðu yer
using ECommerce.Entities;
using ECommerce.Services.Abstract;
using ECommerce.Repositories.Abstract;
using ECommerce.Repositories.Concrete;
using ECommerce.Services.Concrete; // Entities klasöründeki ApplicationUser sýnýfýnýn olduðu yer

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddScoped<IProductServices<Product>, ProductManager>();
builder.Services.AddScoped<ICategoryServices<Category>, CategoryManager>();

builder.Services.AddScoped<IProductRepository<Product>, ProductRepository>();
builder.Services.AddScoped<ICategoryRepository<Category>, CategoryRepository>();

builder.Services.AddScoped<ISizeServices<Size>, SizeManager>();
builder.Services.AddScoped<ISizeRepository<Size>, SizeRepository>();

builder.Services.AddScoped<IFavoriteProductServices<FavoriteProducts>, FavoriteProductManager>();
builder.Services.AddScoped<IFavoriteProductRepository<FavoriteProducts>, FavoriteProductRepository>();

// Add DbContext
builder.Services.AddDbContext<ECommerceDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("ECommerceDb")));

// Add Identity
builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<ECommerceDbContext>()
    .AddDefaultTokenProviders();

// Configure application cookie settings
builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Account/Login";
    options.LogoutPath = "/Account/Logout";
    options.AccessDeniedPath = "/Account/AccessDenied";
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}

app.UseStaticFiles();

app.UseRouting();

// Use Authentication and Authorization
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
