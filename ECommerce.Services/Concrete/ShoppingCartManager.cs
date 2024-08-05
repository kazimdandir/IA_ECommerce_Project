using ECommerce.Entities;
using ECommerce.Repositories.Abstract;
using ECommerce.Services.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Services.Concrete
{
    public class ShoppingCartManager : IShoppingCartService<ShoppingCart>
    {
        private readonly IShoppingCartRepository<ShoppingCart> _shoppingCartRepository;

        public ShoppingCartManager(IShoppingCartRepository<ShoppingCart> shoppingCartRepository)
        {
            _shoppingCartRepository = shoppingCartRepository;
        }

        public async Task<ShoppingCart> GetShoppingCartByUserIdAsync(string userId)
        {
            return await _shoppingCartRepository.GetShoppingCartByUserIdAsync(userId);
        }

        public ShoppingCart CreateShoppingCart(ShoppingCart shoppingCart)
        {
            return _shoppingCartRepository.CreateShoppingCart(shoppingCart);
        }

        public void UpdateShoppingCart(ShoppingCart shoppingCart)
        {
            _shoppingCartRepository.UpdateShoppingCart(shoppingCart);
        }

        public void DeleteShoppingCart(int shoppingCartId)
        {
            _shoppingCartRepository.DeleteShoppingCart(shoppingCartId);
        }

        public IEnumerable<ShoppingCart> GetAllShoppingCarts()
        {
            return _shoppingCartRepository.GetAllShoppingCarts();
        }

        public decimal CalculateTotalAmount(ShoppingCart shoppingCart)
        {
            return shoppingCart.ShoppingCartItems.Sum(item => item.Product.Price * item.Quantity);
        }
    }
}
