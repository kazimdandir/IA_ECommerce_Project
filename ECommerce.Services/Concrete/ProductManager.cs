using ECommerce.Entities;
using ECommerce.Repositories.Abstract;
using ECommerce.Repositories.Concrete;
using ECommerce.Repositories.Context;
using ECommerce.Services.Abstract;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Services.Concrete
{
    public class ProductManager : IProductServices<Product>
    {
        private readonly IProductRepository<Product> _productRepository;
        private readonly ECommerceDbContext _dbContext;

        public ProductManager(IProductRepository<Product> productRepository, ECommerceDbContext dbContext)
        {
            _productRepository = productRepository;
            _dbContext = dbContext;
        }

        public Product CreateProduct(Product product)
        {
            if (product is not null)
            {
                // Ensure category exists
                var existingCategory = _dbContext.Categories.Find(product.CategoryId);
                if (existingCategory == null)
                {
                    throw new Exception("Category not found");
                }

                product.Category = existingCategory; // Discards associated category
                product.Id = 0; // Resets the id field

                _dbContext.Products.Add(product);
                _dbContext.SaveChanges();

                return product;
            }
            else
            {
                throw new Exception("Product cannot be empty!");
            }
        }

        public void DeleteProduct(int productId)
        {
            _productRepository.DeleteProduct(productId);
        }

        public IEnumerable<Product> GetAllProducts()
        {
            return _productRepository.GetAllProducts();
        }

        public async Task<Product> GetProductById(int productId)
        {
            return await _productRepository.GetProductById(productId);
        }

        public Product UpdateProduct(Product product)
        {
            return _productRepository.UpdateProduct(product);
        }

        public IEnumerable<Product> GetProductsByCategoryId(int categoryId)
        {
            return _dbContext.Products
                .Include(p => p.Category) // Include related Category
                .Include(p => p.ShoppingCartItems) // Include related   ShoppingCartItems
                    .ThenInclude(sci => sci.Size) // Include related Size through           ShoppingCartItems
                .Where(p => p.CategoryId == categoryId)
                .ToList();
        }
    }
}
