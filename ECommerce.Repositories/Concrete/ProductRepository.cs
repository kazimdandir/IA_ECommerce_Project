using ECommerce.Entities;
using ECommerce.Repositories.Abstract;
using ECommerce.Repositories.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Repositories.Concrete
{
    public class ProductRepository : IProductRepository<Product>
    {
        private readonly ECommerceDbContext context;

        public ProductRepository(ECommerceDbContext _context)
        {
            context = _context;
        }

        public Product CreateProduct(Product product)
        {
            context.Products.Add(product);
            context.SaveChanges();
            return product;
        }

        public void DeleteProduct(int productId)
        {
            var product = context.Products.FirstOrDefault(c => c.Id == productId);
            context.Products.Remove(product);
            context.SaveChanges();
        }

        public IEnumerable<Product> GetAllProducts()
        {
            return context.Products.ToList();
        }

        public async Task<Product> GetProductById(int productId)
        {
            return await context.Products.FindAsync(productId);
        }

        public Product UpdateProduct(Product product)
        {
            context.Products.Update(product);
            context.SaveChanges();
            return product;
        }
    }
}
