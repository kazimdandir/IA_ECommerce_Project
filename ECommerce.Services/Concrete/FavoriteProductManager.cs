using ECommerce.Entities;
using ECommerce.Repositories.Abstract;
using ECommerce.Repositories.Concrete;
using ECommerce.Services.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Services.Concrete
{
    public class FavoriteProductManager : IFavoriteProductServices<FavoriteProducts>
    {
        private readonly IFavoriteProductRepository<FavoriteProducts> _repository;

        public FavoriteProductManager(IFavoriteProductRepository<FavoriteProducts> repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<FavoriteProducts>> GetAllFavoritesByUserId(string userId)
        {
            return await _repository.GetByUserIdAsync(userId);
        }

        public async Task<bool> AddToFavorites(string userId, int productId)
        {
            // Kullanıcı ve ürün kombinasyonu için favori var mı kontrol et
            var existingFavorite = await _repository.GetByUserAndProductAsync(userId, productId);

            if (existingFavorite == null)
            {
                // Favori bulunamadı, yeni favori ekle
                var favoriteProduct = new FavoriteProducts
                {
                    UserId = userId,
                    ProductId = productId
                };

                try
                {
                    await _repository.AddAsync(favoriteProduct);
                    return true; // İşlem başarılı
                }
                catch
                {
                    return false; // Hata durumunda
                }
            }

            return false; // Zaten favorilerde, tekrar eklemedik
        }

        public async Task RemoveFromFavorites(string userId, int productId)
        {
            var favoriteProduct = await _repository.GetByUserAndProductAsync(userId, productId);
            if (favoriteProduct != null)
            {
                await _repository.RemoveAsync(favoriteProduct);
            }
        }
    }
}
