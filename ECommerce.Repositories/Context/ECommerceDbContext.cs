using ECommerce.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Repositories.Context
{
    public class ECommerceDbContext : DbContext
    {
        public ECommerceDbContext()
        {

        }

        public ECommerceDbContext(DbContextOptions<ECommerceDbContext> options) : base(options)
        {

        }

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    optionsBuilder.UseSqlServer("Server=KAZIM\\SQLExpress;Database=ECommerce_Project_Db;Trusted_Connection=True;").EnableSensitiveDataLogging();
        //}

        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ShoppingCart> ShoppingCarts { get; set; }
        public DbSet<ShoppingCartItem> ShoppingCartItems { get; set; }
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Category>().HasData(
                new Category { Id = 1, Name = "Electronics" },
                new Category { Id = 2, Name = "Clothing" },
                new Category { Id = 3, Name = "Books" }
            );

            modelBuilder.Entity<Product>().HasData(
                new Product { Id = 1, Name = "Laptop", Price = 1000, CategoryId = 1 },
                new Product { Id = 2, Name = "Smartphone", Price = 500, CategoryId = 1 },
                new Product { Id = 3, Name = "T-Shirt", Price = 20, CategoryId = 2 },
                new Product { Id = 4, Name = "Novel", Price = 15, CategoryId = 3 }
            );

            modelBuilder.Entity<ApplicationUser>().HasData(
                new ApplicationUser { Id = "1", FullName = "John Doe", UserName = "johndoe", PasswordHash = "password" },
                new ApplicationUser { Id = "2", FullName = "Jane Smith", UserName = "janesmith", PasswordHash = "password" }
            );

            modelBuilder.Entity<ShoppingCart>().HasData(
                new ShoppingCart { Id = 1, UserId = "1" },
                new ShoppingCart { Id = 2, UserId = "2" }
            );

            modelBuilder.Entity<ShoppingCartItem>().HasData(
                new ShoppingCartItem { Id = 1, ShoppingCartId = 1, ProductId = 1, Quantity = 1 },
                new ShoppingCartItem { Id = 2, ShoppingCartId = 1, ProductId = 3, Quantity = 2 },
                new ShoppingCartItem { Id = 3, ShoppingCartId = 2, ProductId = 2, Quantity = 1 },
                new ShoppingCartItem { Id = 4, ShoppingCartId = 2, ProductId = 4, Quantity = 3 }
            );

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
        }
    }
}