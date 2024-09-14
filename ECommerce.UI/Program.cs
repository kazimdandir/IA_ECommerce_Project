using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ECommerce.Repositories.Context; // Data klas�r�ndeki ECommerceDbContext s�n�f�n�n oldu�u yer
using ECommerce.Entities;
using ECommerce.Services.Abstract;
using ECommerce.Repositories.Abstract;
using ECommerce.Repositories.Concrete;
using ECommerce.Services.Concrete; // Entities klas�r�ndeki ApplicationUser s�n�f�n�n oldu�u yer

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddScoped<IProductServices<Product>, ProductManager>();
builder.Services.AddScoped<ICategoryServices<Category>, CategoryManager>();

builder.Services.AddScoped<IProductRepository<Product>, ProductRepository>();
builder.Services.AddScoped<ICategoryRepository<Category>, CategoryRepository>();

builder.Services.AddScoped<ISizeServices<Sizes>, SizeManager>();
builder.Services.AddScoped<ISizeRepository<Sizes>, SizeRepository>();

builder.Services.AddScoped<IFavoriteProductServices<FavoriteProducts>, FavoriteProductManager>();
builder.Services.AddScoped<IFavoriteProductRepository<FavoriteProducts>, FavoriteProductRepository>();

builder.Services.AddScoped<IShoppingCartService<ShoppingCart>, ShoppingCartManager>();
builder.Services.AddScoped<IShoppingCartRepository<ShoppingCart>, ShoppingCartRepository>();

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

// Admin kullan�c�y� seed etme i�lemi burada tetiklenecek
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
        var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();

        // Seed admin user
        await SeedAdminUser(userManager, roleManager);
    }
    catch (Exception ex)
    {
        // Hata y�netimi
        Console.WriteLine($"An error occurred during seeding: {ex.Message}");
    }
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

// SeedAdminUser method
async Task SeedAdminUser(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
{
    // Admin kullan�c� var m� kontrol et
    var adminUser = await userManager.FindByEmailAsync("kikbal.dandir@gmail.com");

    if (adminUser == null)
    {
        // E�er kullan�c� yoksa admin kullan�c�y� olu�tur
        var seedUser = new ApplicationUser
        {
            FullName = "Kaz�m �kbal Dand�r",
            UserName = "kazimdandir",
            Email = "kikbal.dandir@gmail.com",
            Role = UserRole.Admin // Admin rol� atan�yor
        };

        var seedResult = await userManager.CreateAsync(seedUser, "Kazim.123");



        if (seedResult.Succeeded)
        {
            // Admin rol� mevcut de�ilse olu�tur
            if (!await roleManager.RoleExistsAsync("Admin"))
            {
                await roleManager.CreateAsync(new IdentityRole("Admin"));
            }

            // Admin rol� kullan�c�ya atan�yor
            await userManager.AddToRoleAsync(seedUser, "Admin");
        }
    }
}