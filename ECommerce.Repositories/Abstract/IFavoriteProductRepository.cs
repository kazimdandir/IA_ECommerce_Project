using ECommerce.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Repositories.Abstract
{
    public interface IFavoriteProductRepository<T> where T : class
    {
        Task AddAsync(T entity);
        Task RemoveAsync(T entity);
        Task<T> GetByUserAndProductAsync(string userId, int productId);
        Task<IEnumerable<T>> GetByUserIdAsync(string userId);
    }
}
