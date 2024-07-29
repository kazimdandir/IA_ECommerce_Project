using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Repositories.Abstract
{
    public interface IProductRepository<T> where T : class
    {
        T CreateProduct(T product);
        T UpdateProduct(T product);
        void DeleteProduct(int productId);
        Task<T> GetProductById(int productId);
        IEnumerable<T> GetAllProducts();
    }
}
