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
    public class FavoriteProductRepository : IFavoriteProductRepository<FavoriteProducts>
    {
        private readonly ECommerceDbContext _context;

        public FavoriteProductRepository(ECommerceDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(FavoriteProducts entity)
        {
            await _context.FavoriteProducts.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task RemoveAsync(FavoriteProducts entity)
        {
            _context.FavoriteProducts.Remove(entity);
            await _context.SaveChangesAsync();
        }

        public async Task<FavoriteProducts> GetByUserAndProductAsync(string userId, int productId)
        {
            return await _context.FavoriteProducts
                .FirstOrDefaultAsync(fp => fp.UserId == userId && fp.ProductId == productId);
        }

        public async Task<IEnumerable<FavoriteProducts>> GetByUserIdAsync(string userId)
        {
            return await _context.FavoriteProducts
                .Where(fp => fp.UserId == userId)
                .ToListAsync();
        }
    }
}
