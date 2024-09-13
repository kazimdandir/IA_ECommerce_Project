using ECommerce.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Repositories.Context
{
    public class ECommerceDbContext : IdentityDbContext<ApplicationUser>
    {
        public ECommerceDbContext()
        {

        }

        public ECommerceDbContext(DbContextOptions<ECommerceDbContext> options) : base(options)
        {

        }

        // Open only when creating the first migration, comment after migration
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Server=KAZIM\\SQLExpress;Database=ECommerce_Project_Db;Trusted_Connection=True;")
                              .EnableSensitiveDataLogging();
            }
        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ShoppingCart> ShoppingCarts { get; set; }
        public DbSet<ShoppingCartItem> ShoppingCartItems { get; set; }
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<Size> Sizes { get; set; }
        public DbSet<FavoriteProducts> FavoriteProducts { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder); // Adds identity tables

            // We use PasswordHasher to hash passwords
            var hasher = new PasswordHasher<ApplicationUser>();

            modelBuilder.Entity<ApplicationUser>().HasData(
                new ApplicationUser
                {
                    Id = "1", // Given as String because IdentityUser's Id is usually a string
                    FullName = "John Doe",
                    UserName = "johndoe",
                    NormalizedUserName = "JOHNDOE",
                    Email = "johndoe@example.com",
                    NormalizedEmail = "JOHNDOE@EXAMPLE.COM",
                    EmailConfirmed = true,
                    PasswordHash = hasher.HashPassword(null, "password") // We hash the password
                },
                new ApplicationUser
                {
                    Id = "2",
                    FullName = "Jane Smith",
                    UserName = "janesmith",
                    NormalizedUserName = "JANESMITH",
                    Email = "janesmith@example.com",
                    NormalizedEmail = "JANESMITH@EXAMPLE.COM",
                    EmailConfirmed = true,
                    PasswordHash = hasher.HashPassword(null, "password")
                },
                new ApplicationUser
                {
                    Id = "3", // Given as String because IdentityUser's Id is usually a string
                    FullName = "Kazım İkbal Dandır",
                    UserName = "kazimdandir",
                    NormalizedUserName = "KAZIMIKBALDANDIR",
                    Email = "kikbal.dandir@gmail.com",
                    NormalizedEmail = "KAZIMIKBALDANDIR@GMAIL.COM",
                    EmailConfirmed = true,
                    PasswordHash = "Kazim.123"
                }
            );

            modelBuilder.Entity<Size>().HasData(
                new Size { Id = 1, SizeName = "28" },
                new Size { Id = 2, SizeName = "32" },
                new Size { Id = 3, SizeName = "34" },
                new Size { Id = 4, SizeName = "36" },
                new Size { Id = 5, SizeName = "38" },
                new Size { Id = 6, SizeName = "40" },
                new Size { Id = 7, SizeName = "42" },
                new Size { Id = 8, SizeName = "44" },
                new Size { Id = 9, SizeName = "46" },
                new Size { Id = 10, SizeName = "XS" },
                new Size { Id = 11, SizeName = "S" },
                new Size { Id = 12, SizeName = "M" },
                new Size { Id = 13, SizeName = "L" },
                new Size { Id = 14, SizeName = "XL" },
                new Size { Id = 15, SizeName = "XXL" },
                new Size { Id = 16, SizeName = "XXXL" },
                new Size { Id = 17, SizeName = "One Size" }
            );

            modelBuilder.Entity<Category>().HasData(
                new Category { Id = 1, Name = "Footwear" },
                new Category { Id = 2, Name = "Jewellery" },
                new Category { Id = 3, Name = "Hats & Caps" },
                new Category { Id = 4, Name = "Sunglasses" },
                new Category { Id = 5, Name = "T-Shirts" },
                new Category { Id = 6, Name = "Tracksuits" },
                new Category { Id = 7, Name = "Jeans" },
                new Category { Id = 8, Name = "Hoodies & Sweatshirts" },
                new Category { Id = 9, Name = "Coats & Jackets" },
                new Category { Id = 10, Name = "Trousers" },
                new Category { Id = 11, Name = "Shorts" },
                new Category { Id = 12, Name = "Shirts" },
                new Category { Id = 13, Name = "Joggers" },
                new Category { Id = 14, Name = "Cargos" }
            );

            modelBuilder.Entity<Product>().HasData(
                new Product { Id = 1, Name = "Oversized Bird Graphic T-shirt", Price = 14, CategoryId = 5, ImagePath = "/img/Oversized Bird Graphic T-shirt.png", DetailsCare = "100% cotton. Model is 6\"1 and wears a size 3XL" },
                new Product { Id = 2, Name = "Oversized Deadpool Cereal License Print T-shirt", Price = 20, CategoryId = 5, ImagePath = "/img/Oversized Deadpool Cereal License Print T-shirt.png", DetailsCare = "100% Baumwolle. Das Model ist 185cm groß und trägt Größe M" },
                new Product { Id = 3, Name = "Chocolate Oversized Extended Neck ABODE T-shirt", Price = 14, CategoryId = 5, ImagePath = "/img/Chocolate Oversized Extended Neck ABODE T-shirt.png", DetailsCare = "100% Baumwolle" },
                new Product { Id = 4, Name = "Baggy Rigid Jean", Price = 25, CategoryId = 7, ImagePath = "/img/Baggy Rigid Jean.png", DetailsCare = "100% COTTON. MODEL IS 6'1 AND WEARS SIZE 32." },
                new Product { Id = 5, Name = "Chocolate Relaxed Rigid Flare Patchwork Jeans", Price = 42, CategoryId = 7, ImagePath = "/img/Chocolate Relaxed Rigid Flare Patchwork Jeans.png", DetailsCare = "100% COTTON. MODEL IS 6'1 AND WEARS SIZE 32." },
                new Product { Id = 6, Name = "Grey Slim Flared All Over Ripped Jeans With Let Down Hem", Price = 35, CategoryId = 7, ImagePath = "/img/Grey Slim Flared All Over Ripped Jeans With Let Down Hem.png", DetailsCare = "100% Baumwolle. Das Model ist 185cm groß und trägt Größe 32." },
                new Product { Id = 7, Name = "Short Sleeve Linen Shirt", Price = 50, CategoryId = 12, ImagePath = "/img/Short Sleeve Linen Shirt.png", DetailsCare = "50% Viskose, 40% Baumwolle, 10% Leinen. Das Model ist 185cm groß und trägt Größe M" },
                new Product { Id = 8, Name = "Green Satin Oversized Revere Statue Border Shirt", Price = 28, CategoryId = 12, ImagePath = "/img/Green Satin Oversized Revere Statue Border Shirt.png", DetailsCare = "100% Polyester. Model ist 185cm geoß und trägt Größe M." },
                new Product { Id = 9, Name = "Tonal Chunky Trainers In Blue", Price = 35, CategoryId = 1, ImagePath = "/img/Tonal Chunky Trainers In Blue.png", DetailsCare = "Upper: Polyurethane (PU) Lining: Textile Insole: Textile Outsole: Thermoplastic Rubber (TPR)" },
                new Product { Id = 10, Name = "Stone Track Sole Loafer", Price = 35, CategoryId = 1, ImagePath = "/img/Stone Track Sole Loafer.png", DetailsCare = "UPPER/LINING : 100% PU SYNTHETIC, SOLE: 100% THERMOPLASTIC RUBBER" },
                new Product { Id = 11, Name = "Red Tapestry Buckle Detail Mule", Price = 32, CategoryId = 1, ImagePath = "/img/Red Tapestry Buckle Detail Mule.png", DetailsCare = "UPPER : 100% TEXTILE LINING : 100%PU SYNTH , SOCKS: 100%PU SYNTH, MID SOLE : 100% PU SOLE : 100% EVA" },
                new Product { Id = 12, Name = "Silver Cuban Chain Jean Chain", Price = 10, CategoryId = 2, ImagePath = "/img/Silver Cuban Chain Jean Chain.png", DetailsCare = "zinc alloy+glass" },
                new Product { Id = 13, Name = "Silver 3 Pack Mixed Bead Rings", Price = 8, CategoryId = 2, ImagePath = "/img/Silver 3 Pack Mixed Bead Rings.png", DetailsCare = "90%zinc alloy+8%glass+2%epoxy" },
                new Product { Id = 14, Name = "BM Flames Cap In Black", Price = 8, CategoryId = 3, ImagePath = "/img/BM Flames Cap In Black.png", DetailsCare = "80% Polyester, 20% Kunststoff" },
                new Product { Id = 15, Name = "Black Gothic Logo Jacquard Beanie", Price = 12, CategoryId = 3, ImagePath = "/img/Black Gothic Logo Jacquard Beanie.png", DetailsCare = "100% acrylic" },
                new Product { Id = 16, Name = "Star Rimless Sunglasses In Red", Price = 8, CategoryId = 4, ImagePath = "/img/Star Rimless Sunglasses In Red.png", DetailsCare = "90% Polycarbonate 10% Copper" },
                new Product { Id = 17, Name = "Brown Aviator Matte Sunglasses", Price = 6, CategoryId = 4, ImagePath = "/img/Brown Aviator Matte Sunglasses.png", DetailsCare = "90% Metal, 10% Plastic." },
                new Product { Id = 18, Name = "Black Oversized Boxy Over The Seams Eagle Graphic Tracksuit", Price = 45, CategoryId = 6, ImagePath = "/img/Black Oversized Boxy Over The Seams Eagle Graphic Tracksuit.png", DetailsCare = "60% Cotton 40% Polyester. Model is 6'1 and wears size M." },
                new Product { Id = 19, Name = "Burgundy Oversized Boxy Cross Applique Zip Through Hoodie And Relaxed Jogger Tracksuit", Price = 60, CategoryId = 6, ImagePath = "/img/Burgundy Oversized Boxy Cross Applique Zip Through Hoodie And Relaxed Jogger Tracksuit.png", DetailsCare = "60% Baumwolle 40% Polyester" },
                new Product { Id = 20, Name = "Sage Oversized Boxy ABODE Hoodie", Price = 25, CategoryId = 8, ImagePath = "/img/Sage Oversized Boxy ABODE Hoodie.png", DetailsCare = "50% Baumwolle, 50% Polyester" },
                new Product { Id = 21, Name = "Sand Monaco Back Print Sweatshirt", Price = 30, CategoryId = 8, ImagePath = "/img/Sand Monaco Back Print Sweatshirt.png", DetailsCare = "50% Baumwolle und 50% Polyester, das Model ist 185cm groß und trägt Größe M." },
                new Product { Id = 22, Name = "Yellow Oversized PU Badge Moto Jacket", Price = 70, CategoryId = 9, ImagePath = "/img/Yellow Oversized PU Badge Moto Jacket.png", DetailsCare = "100% Polyurethane. Model is 6'1 and wears size M." },
                new Product { Id = 23, Name = "Washed black Oversized Dirty Wash Carpenter Denim Biker Jacket", Price = 40, CategoryId = 9, ImagePath = "/img/Washed black Oversized Dirty Wash Carpenter Denim Biker Jacket.png", DetailsCare = "100% Baumwolle. Model ist 185cm groß und trägt Größe M" },
                new Product { Id = 24, Name = "Stone Fixed Waist Relaxed Applique Print Trouser", Price = 35, CategoryId = 10, ImagePath = "/img/Stone Fixed Waist Relaxed Applique Print Trouser.png", DetailsCare = "100% Cotton. Model is 6'1 and wears size M/ 32." },
                new Product { Id = 25, Name = "Slate Elasticated Waist Relaxed Fit Buckle Cargo Trouser", Price = 35, CategoryId = 10, ImagePath = "/img/Slate Elasticated Waist Relaxed Fit Buckle Cargo Trouser.png", DetailsCare = "100% Cotton. Model is 6'1 and wears size M." },
                new Product { Id = 26, Name = "Grey Slim Fit Elasticated Waist Cargo Shorts", Price = 22, CategoryId = 11, ImagePath = "/img/Grey Slim Fit Elasticated Waist Cargo Shorts.png", DetailsCare = "98% Baumwolle, 2% Elasthan. Model ist 185cm groß und trägt Größe M." },
                new Product { Id = 27, Name = "Charcoal Oversized Drop Crotch Rib Hem Loopback Short", Price = 20, CategoryId = 11, ImagePath = "/img/Charcoal Oversized Drop Crotch Rib Hem Loopback Short.png", DetailsCare = "52% Polyester, 48% Baumwolle. Model ist 185cm groß und trägt Größe M" },
                new Product { Id = 28, Name = "Blue Relaxed Fit Split Hem Jacquard Joggers", Price = 30, CategoryId = 13, ImagePath = "/img/Blue Relaxed Fit Split Hem Jacquard Joggers.png", DetailsCare = "60% Baumwolle, 40% Polyester. Model ist 185cm groß und trägt EU Größe M." },
                new Product { Id = 29, Name = "Mint Plus Oversized Heavy Washed Applique Jogger", Price = 30, CategoryId = 13, ImagePath = "/img/Mint Plus Oversized Heavy Washed Applique Jogger.png", DetailsCare = "80% cotton 20% polyester. Model is 6\\'1 and wears a size 3XL" },
                new Product { Id = 30, Name = "Olive Fixed Waist Slim Stacked Flare Strap Cargo Trouser", Price = 40, CategoryId = 14, ImagePath = "/img/Olive Fixed Waist Slim Stacked Flare Strap Cargo Trouser.png", DetailsCare = "98% Baumwolle, 2% Elastan. Das Model ist 185cm groß und trägt Größe 32" },
                new Product { Id = 31, Name = "Grey Plus Fixed Waist Straight Fit Cargo Trousers", Price = 30, CategoryId = 14, ImagePath = "/img/Grey Plus Fixed Waist Straight Fit Cargo Trousers.png", DetailsCare = "100% cotton. This model is 6\"1 and wears a size 3XL." }
            );

            modelBuilder.Entity<ShoppingCart>().HasData(
                new ShoppingCart { Id = 1, UserId = "1" },
                new ShoppingCart { Id = 2, UserId = "2" }
            );

            modelBuilder.Entity<ShoppingCartItem>().HasData(
                new ShoppingCartItem { Id = 1, ShoppingCartId = 1, ProductId = 1, Quantity = 1, SizeId = 12 },
                new ShoppingCartItem { Id = 2, ShoppingCartId = 1, ProductId = 2, Quantity = 2, SizeId = 11 },
                new ShoppingCartItem { Id = 3, ShoppingCartId = 2, ProductId = 3, Quantity = 1, SizeId = 13 },
                new ShoppingCartItem { Id = 4, ShoppingCartId = 2, ProductId = 4, Quantity = 3, SizeId = 3 },
                new ShoppingCartItem { Id = 5, ShoppingCartId = 1, ProductId = 5, Quantity = 1, SizeId = 4 },
                new ShoppingCartItem { Id = 6, ShoppingCartId = 2, ProductId = 6, Quantity = 2, SizeId = 5 },
                new ShoppingCartItem { Id = 7, ShoppingCartId = 1, ProductId = 7, Quantity = 5, SizeId = 10 },
                new ShoppingCartItem { Id = 8, ShoppingCartId = 2, ProductId = 8, Quantity = 1, SizeId = 14 },
                new ShoppingCartItem { Id = 9, ShoppingCartId = 2, ProductId = 9, Quantity = 2, SizeId = 6 },
                new ShoppingCartItem { Id = 10, ShoppingCartId = 1, ProductId = 10, Quantity = 1, SizeId = 7 },
                new ShoppingCartItem { Id = 11, ShoppingCartId = 1, ProductId = 11, Quantity = 6, SizeId = 8 },
                new ShoppingCartItem { Id = 12, ShoppingCartId = 1, ProductId = 12, Quantity = 1, SizeId = 17 },
                new ShoppingCartItem { Id = 13, ShoppingCartId = 2, ProductId = 13, Quantity = 2, SizeId = 17 },
                new ShoppingCartItem { Id = 14, ShoppingCartId = 2, ProductId = 14, Quantity = 5, SizeId = 11 },
                new ShoppingCartItem { Id = 15, ShoppingCartId = 1, ProductId = 15, Quantity = 3, SizeId = 12 },
                new ShoppingCartItem { Id = 16, ShoppingCartId = 1, ProductId = 16, Quantity = 2, SizeId = 17 },
                new ShoppingCartItem { Id = 17, ShoppingCartId = 2, ProductId = 17, Quantity = 1, SizeId = 17 },
                new ShoppingCartItem { Id = 18, ShoppingCartId = 1, ProductId = 18, Quantity = 2, SizeId = 4 },
                new ShoppingCartItem { Id = 19, ShoppingCartId = 2, ProductId = 19, Quantity = 4, SizeId = 2 },
                new ShoppingCartItem { Id = 20, ShoppingCartId = 1, ProductId = 20, Quantity = 2, SizeId = 14 },
                new ShoppingCartItem { Id = 21, ShoppingCartId = 2, ProductId = 21, Quantity = 1, SizeId = 16 },
                new ShoppingCartItem { Id = 22, ShoppingCartId = 1, ProductId = 22, Quantity = 2, SizeId = 12 },
                new ShoppingCartItem { Id = 23, ShoppingCartId = 2, ProductId = 23, Quantity = 1, SizeId = 13 },
                new ShoppingCartItem { Id = 24, ShoppingCartId = 2, ProductId = 24, Quantity = 2, SizeId = 9 },
                new ShoppingCartItem { Id = 25, ShoppingCartId = 1, ProductId = 25, Quantity = 4, SizeId = 6 },
                new ShoppingCartItem { Id = 26, ShoppingCartId = 1, ProductId = 26, Quantity = 2, SizeId = 12 },
                new ShoppingCartItem { Id = 27, ShoppingCartId = 2, ProductId = 27, Quantity = 5, SizeId = 10 },
                new ShoppingCartItem { Id = 28, ShoppingCartId = 1, ProductId = 28, Quantity = 2, SizeId = 1 },
                new ShoppingCartItem { Id = 29, ShoppingCartId = 2, ProductId = 29, Quantity = 1, SizeId = 2 },
                new ShoppingCartItem { Id = 30, ShoppingCartId = 1, ProductId = 30, Quantity = 1, SizeId = 3 },
                new ShoppingCartItem { Id = 31, ShoppingCartId = 1, ProductId = 31, Quantity = 3, SizeId = 14 }
            );


            modelBuilder.Entity<ShoppingCartItem>()
                .Property(s => s.SizeId)
                .IsRequired(false); // Nullable olarak işaretleme

            // Relationships
            modelBuilder.Entity<Product>()
                .HasOne(p => p.Category)
                .WithMany(c => c.Products)
                .HasForeignKey(p => p.CategoryId);

            modelBuilder.Entity<ShoppingCartItem>()
                .HasOne(sci => sci.Product)
                .WithMany(p => p.ShoppingCartItems)
                .HasForeignKey(sci => sci.ProductId);

            modelBuilder.Entity<ShoppingCartItem>()
                .HasOne(sci => sci.ShoppingCart)
                .WithMany(sc => sc.ShoppingCartItems)
                .HasForeignKey(sci => sci.ShoppingCartId);

            modelBuilder.Entity<ShoppingCart>()
                .HasOne(sc => sc.User)
                .WithMany(au => au.ShoppingCarts)
                .HasForeignKey(sc => sc.UserId);

            modelBuilder.Entity<ShoppingCartItem>()
                .HasOne(sci => sci.Size)
                .WithMany(s => s.ShoppingCartItems)
                .HasForeignKey(sci => sci.SizeId);

            modelBuilder.Entity<FavoriteProducts>()
                .HasOne(fp => fp.Product)
                .WithMany()
                .HasForeignKey(fp => fp.ProductId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<FavoriteProducts>()
                .HasOne(fp => fp.User)
                .WithMany() // Bu kısmı ihtiyaçlarınıza göre ayarlayın
                .HasForeignKey(fp => fp.UserId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}