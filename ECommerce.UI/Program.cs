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

// Admin kullanýcýyý seed etme iþlemi burada tetiklenecek
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
        // Hata yönetimi
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
    // Admin kullanýcý var mý kontrol et
    var adminUser = await userManager.FindByEmailAsync("kikbal.dandir@gmail.com");

    if (adminUser == null)
    {
        // Eðer kullanýcý yoksa admin kullanýcýyý oluþtur
        var seedUser = new ApplicationUser
        {
            FullName = "Kazým Ýkbal Dandýr",
            UserName = "kazimdandir",
            Email = "kikbal.dandir@gmail.com",
            Role = UserRole.Admin // Admin rolü atanýyor
        };

        var seedResult = await userManager.CreateAsync(seedUser, "Kazim.123");



        if (seedResult.Succeeded)
        {
            // Admin rolü mevcut deðilse oluþtur
            if (!await roleManager.RoleExistsAsync("Admin"))
            {
                await roleManager.CreateAsync(new IdentityRole("Admin"));
            }

            // Admin rolü kullanýcýya atanýyor
            await userManager.AddToRoleAsync(seedUser, "Admin");
        }
    }
}