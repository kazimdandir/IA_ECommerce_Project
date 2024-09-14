using ECommerce.Entities;
using ECommerce.Repositories.Abstract;
using ECommerce.Repositories.Context;
using ECommerce.Services.Abstract;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace ECommerce.UI.Controllers
{
    public class CategoryController : Controller
    {
        private readonly IProductServices<Product> _productService;
        private readonly ICategoryServices<Category> _categoryService;
        private readonly ISizeServices<Sizes> _sizeService; // Add this line
        private readonly IFavoriteProductServices<FavoriteProducts> _favoriteProductServices;
        private readonly IShoppingCartService<ShoppingCart> _shoppingCartServices;

        private readonly ECommerceDbContext _context;

        public CategoryController(IProductServices<Product> productService, ICategoryServices<Category> categoryService, ISizeServices<Sizes> sizeService, ECommerceDbContext context, IFavoriteProductServices<FavoriteProducts> favoriteProductServices, IShoppingCartService<ShoppingCart> shoppingCartServices)
        {
            _productService = productService;
            _categoryService = categoryService;
            _sizeService = sizeService;
            _context = context;
            _favoriteProductServices = favoriteProductServices;
            _shoppingCartServices = shoppingCartServices;
        }

        public IActionResult ProductsByCategory(int categoryId, int? sizeId)
        {
            var products = _productService.GetProductsByCategoryId(categoryId);

            if (sizeId.HasValue)
            {
                // Sadece seçilen beden için ürünleri filtrele
                products = products
                    .Where(p => p.ShoppingCartItems.Any(sci => sci.SizeId == sizeId.Value))
                    .ToList();
            }

            ViewBag.Sizes = _sizeService.GetAllSizes();
            ViewBag.CategoryId = categoryId; // CategoryId'yi ViewBag'e koy

            return View(products);
        }

        public IActionResult AllProducts(int? sizeId, string searchQuery)
        {
            // Fetch all products and include the related Category and ShoppingCartItems in memory
            var allProducts = _productService.GetAllProducts()
                .ToList(); // Load everything into memory first

            // Ensure that Category and ShoppingCartItems are loaded
            foreach (var product in allProducts)
            {
                _context.Entry(product).Reference(p => p.Category).Load(); // Load Category
                _context.Entry(product).Collection(p => p.ShoppingCartItems).Load(); // Load ShoppingCartItems

                foreach (var item in product.ShoppingCartItems)
                {
                    _context.Entry(item).Reference(sci => sci.Size).Load(); // Load Size for ShoppingCartItems
                }
            }

            // Filter products based on the selected size
            if (sizeId.HasValue)
            {
                allProducts = allProducts
                    .Where(p => p.ShoppingCartItems.Any(sci => sci.SizeId == sizeId.Value))
                    .ToList();
            }

            // Filter products based on the search query
            if (!string.IsNullOrWhiteSpace(searchQuery))
            {
                allProducts = allProducts
                    .Where(p => p.Name.Contains(searchQuery, StringComparison.OrdinalIgnoreCase))
                    .ToList();
            }

            ViewBag.Sizes = _sizeService.GetAllSizes();
            ViewBag.SearchQuery = searchQuery; // Pass search query to the view

            return View(allProducts);
        }

        public async Task<IActionResult> ProductDetails(int id)
        {
            // Ürünü id'ye göre bul ve gerekli ilişkileri yükle
            var product = await _productService.GetProductById(id);

            _context.Entry(product).Reference(p => p.Category).Load(); // Kategori bilgisini yükle
            _context.Entry(product).Collection(p => p.ShoppingCartItems).Load(); // Sepet öğelerini yükle

            foreach (var item in product.ShoppingCartItems)
            {
                _context.Entry(item).Reference(sci => sci.Size).Load(); // Sepet öğeleri için beden bilgisini yükle
            }

            if (product == null)
            {
                return NotFound(); // Ürün bulunamazsa 404 döndür
            }

            return View(product); // Ürün detayları ile ürünü döndür
        }

        [HttpPost]
        public async Task<IActionResult> AddToFavorites(int productId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier); // UserId'yi al

            if (string.IsNullOrEmpty(userId))
            {
                return Json(new { success = false, message = "User is not logged in." });
            }

            // Favorilere ekleme işlemi
            var result = await _favoriteProductServices.AddToFavorites(userId, productId);

            if (result)
            {
                return Json(new { success = true });
            }
            else
            {
                return Json(new { success = false, message = "Failed to add to favorites." });
            }
        }

        [HttpPost]
        public async Task<IActionResult> RemoveFromFavorites(int productId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier); // Doğru UserId'yi al
            await _favoriteProductServices.RemoveFromFavorites(userId, productId);
            return RedirectToAction("GetAllFavorites");
        }

        public async Task<IActionResult> GetAllFavorites()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier); // Kullanıcının ID'sini al

            if (string.IsNullOrEmpty(userId))
            {
                // Kullanıcı giriş yapmamışsa kayıt sayfasına yönlendir
                return RedirectToAction("Register", "Account");
            }

            // Kullanıcı ID'sine göre favori ürünleri al
            var favoriteProducts = await _context.FavoriteProducts
                .Where(f => f.UserId == userId) // Kullanıcıya ait favori ürünleri filtrele
                .Include(f => f.Product)
                .ThenInclude(p => p.Category) // Kategori dahil ediliyor
                .ToListAsync(); // Asenkron olarak listeyi al

            if (favoriteProducts == null || !favoriteProducts.Any())
            {
                ViewBag.Message = "No favorite products found."; // Favori ürün bulunamadı mesajı
                return View(new List<FavoriteProducts>()); // Boş bir liste döndürülüyor
            }

            return View(favoriteProducts); // Favori ürünleri döndür
        }

        [HttpPost]
        public async Task<IActionResult> AddToCart(int productId, int quantity = 1)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
            {
                return Json(new { success = false, message = "User is not logged in." });
            }

            var cart = await _shoppingCartServices.GetShoppingCartByUserIdAsync(userId);

            if (cart == null)
            {
                cart = new ShoppingCart { UserId = userId };
                _shoppingCartServices.CreateShoppingCart(cart);
            }

            var shoppingCartItem = cart.ShoppingCartItems.FirstOrDefault(item => item.ProductId == productId);

            if (shoppingCartItem != null)
            {
                shoppingCartItem.Quantity += quantity;
            }
            else
            {
                shoppingCartItem = new ShoppingCartItem
                {
                    ProductId = productId,
                    Quantity = quantity,
                    //SizeId = /* ilgili sizeId alınacaksa buraya ekleyin */
                    ShoppingCartId = cart.Id
                };
                cart.ShoppingCartItems.Add(shoppingCartItem);
            }

            _shoppingCartServices.UpdateShoppingCart(cart);

            return RedirectToAction("Cart");
        }

        public async Task<IActionResult> Cart()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
            {
                return RedirectToAction("Register", "Account");
            }

            var cart = await _shoppingCartServices.GetShoppingCartByUserIdAsync(userId);

            if (cart == null)
            {
                return View(new List<ShoppingCartItem>());
            }

            return View(cart.ShoppingCartItems);
        }

        [HttpPost]
        public async Task<IActionResult> RemoveFromCart(int shoppingCartItemId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
            {
                return Json(new { success = false, message = "User is not logged in." });
            }

            var cart = await _shoppingCartServices.GetShoppingCartByUserIdAsync(userId);

            if (cart == null)
            {
                return Json(new { success = false, message = "Cart not found." });
            }

            var itemToRemove = cart.ShoppingCartItems.FirstOrDefault(item => item.Id == shoppingCartItemId);

            if (itemToRemove != null)
            {
                cart.ShoppingCartItems.Remove(itemToRemove);
                _shoppingCartServices.UpdateShoppingCart(cart);
            }

            return RedirectToAction("Cart");
        }

        [HttpGet]
        public async Task<IActionResult> Checkout()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
            {
                return RedirectToAction("Login", "Account");
            }

            var cart = await _shoppingCartServices.GetShoppingCartByUserIdAsync(userId);

            if (cart != null)
            {
                // Sepeti temizle
                cart.ShoppingCartItems.Clear();
                _shoppingCartServices.UpdateShoppingCart(cart);
            }

            // Ödeme işlemi başarılı mesajı
            ViewBag.PaymentSuccess = true;

            return View("Checkout");
        }
    }
}
