using ECommerce.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Repositories.Abstract
{
    public interface IShoppingCartRepository<T> where T : class
    {
        Task<T> GetShoppingCartByUserIdAsync(string userId);
        T CreateShoppingCart(T shoppingCart);
        void UpdateShoppingCart(T shoppingCart);
        void DeleteShoppingCart(int shoppingCartId);
        IEnumerable<T> GetAllShoppingCarts();
    }
}
