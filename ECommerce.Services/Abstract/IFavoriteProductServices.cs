using ECommerce.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Services.Abstract
{
    public interface IFavoriteProductServices<T> where T : class
    {
        Task<bool> AddToFavorites(string userId, int productId);
        Task RemoveFromFavorites(string userId, int productId);
        Task<IEnumerable<FavoriteProducts>> GetAllFavoritesByUserId(string userId);
    }
}
