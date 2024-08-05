using ECommerce.Entities;
using ECommerce.Repositories.Abstract;
using ECommerce.Repositories.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Repositories.Concrete
{
    public class ShoppingCartRepository : IShoppingCartRepository<ShoppingCart>
    {
        private readonly ECommerceDbContext _context;

        public ShoppingCartRepository(ECommerceDbContext context)
        {
            _context = context;
        }

        public async Task<ShoppingCart> GetShoppingCartByUserIdAsync(string userId)
        {
            return await _context.ShoppingCarts
                .Include(sc => sc.ShoppingCartItems)
                .ThenInclude(sci => sci.Product)
                .FirstOrDefaultAsync(sc => sc.UserId == userId);
        }

        public ShoppingCart CreateShoppingCart(ShoppingCart shoppingCart)
        {
            _context.ShoppingCarts.Add(shoppingCart);
            _context.SaveChanges();
            return shoppingCart;
        }

        public void UpdateShoppingCart(ShoppingCart shoppingCart)
        {
            _context.ShoppingCarts.Update(shoppingCart);
            _context.SaveChanges();
        }

        public void DeleteShoppingCart(int shoppingCartId)
        {
            var shoppingCart = _context.ShoppingCarts.Find(shoppingCartId);
            if (shoppingCart != null)
            {
                _context.ShoppingCarts.Remove(shoppingCart);
                _context.SaveChanges();
            }
        }

        public IEnumerable<ShoppingCart> GetAllShoppingCarts()
        {
            return _context.ShoppingCarts
                .Include(sc => sc.ShoppingCartItems)
                .ThenInclude(sci => sci.Product)
                .ToList();
        }
    }
}
