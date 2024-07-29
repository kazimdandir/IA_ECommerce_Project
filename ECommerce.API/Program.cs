using ECommerce.Entities;
using ECommerce.Repositories.Abstract;
using ECommerce.Repositories.Concrete;
using ECommerce.Repositories.Context;
using ECommerce.Services.Abstract;
using ECommerce.Services.Concrete;
using System;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<ICategoryRepository<Category>, CategoryRepository>();
builder.Services.AddSingleton<ICategoryServices<Category>, CategoryManager>();
builder.Services.AddSingleton<IProductRepository<Product>, ProductRepository>();
builder.Services.AddSingleton<IProductServices<Product>, ProductManager>();
builder.Services.AddSingleton<ECommerceDbContext>();
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve;
        options.JsonSerializerOptions.MaxDepth = 64; // Or whatever depth you require
        options.JsonSerializerOptions.WriteIndented = true; // Optional, makes JSON output more readable
    });

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseStaticFiles(); // Statik dosyalarý etkinleþtir
app.UseAuthorization();
app.MapControllers();

app.Run();