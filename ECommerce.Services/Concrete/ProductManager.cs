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
        public readonly IProductRepository<Product> productRepository;
        public readonly ECommerceDbContext _dbContext;

        public ProductManager(IProductRepository<Product> _productRepository, ECommerceDbContext dbContext)
        {
            productRepository = _productRepository;
            _dbContext = dbContext;
        }

        public Product CreateProduct(Product product)
        {
            #region oldCode
            //if (product is not null)
            //{
            //    return productRepository.CreateProduct(product);
            //}
            //else
            //{
            //    throw new Exception("Product cannot be empty!");
            //} 
            #endregion

            #region newCode
            if (product is not null)
            {
                // Ensure category exists
                var existingCategory = _dbContext.Categories.Find(product.CategoryId);
                if (existingCategory == null)
                {
                    throw new Exception("Category not found");
                }

                product.Category = existingCategory; // İlişkili kategoriyi atayın
                product.Id = 0; // Id alanını sıfırlayın

                _dbContext.Products.Add(product);
                _dbContext.SaveChanges();

                return product;
            }
            else
            {
                throw new Exception("Product cannot be empty!");
            }
            #endregion
        }


        public void DeleteProduct(int productId)
        {
            if (productId != 0)
            {
                productRepository.DeleteProduct(productId);
            }
            else
            {
                throw new Exception("ID must not be NULL during the deletion process!");
            }
        }

        public IEnumerable<Product> GetAllProducts()
        {
            if (productRepository.GetAllProducts() is not null)
            {
                return productRepository.GetAllProducts();
            }
            else
            {
                throw new Exception("Product list is empty!");
            }
        }

        public async Task<Product> GetProductById(int productId)
        {
            if (productId > 0)
            {
                return await productRepository.GetProductById(productId);
            }
            else
            {
                throw new Exception("ID parameter cannot be less than 1!");
            }
        }

        public Product UpdateProduct(Product product)
        {
            if (product is not null)
            {
                return productRepository.UpdateProduct(product);
            }
            else
            {
                throw new Exception("Product cannot be empty!");
            }
        }
    }
}
