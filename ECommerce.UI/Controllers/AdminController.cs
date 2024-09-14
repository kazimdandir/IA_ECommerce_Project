using ECommerce.Entities;
using ECommerce.Repositories.Context;
using ECommerce.Services.Abstract;
using ECommerce.UI.Models.Account;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Png;

namespace ECommerce.UI.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly ICategoryServices<Category> _categoryService;
        private readonly IProductServices<Product> _productService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ECommerceDbContext _context;
        private readonly ISizeServices<Sizes> _sizeService; // Add this line
        private readonly IShoppingCartService<ShoppingCart> _shoppingCartServices;

        public AdminController(ICategoryServices<Category> categoryService, IProductServices<Product> productService, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, ECommerceDbContext context, ISizeServices<Sizes> sizeService, IShoppingCartService<ShoppingCart> shoppingCartServices)
        {
            _categoryService = categoryService;
            _productService = productService;
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
            _shoppingCartServices = shoppingCartServices;
            _sizeService = sizeService;
        }

        #region Categories

        // GET: Admin/Categories
        public async Task<IActionResult> Categories()
        {
            try
            {
                var categories = _categoryService.GetAllCategories();
                return View(categories);
            }
            catch (Exception ex)
            {
                // Handle exceptions as needed, possibly log them
                ViewBag.ErrorMessage = ex.Message;
                return View(new List<Category>());
            }
        }

        // GET: Admin/Categories/Create
        public IActionResult CreateCategory()
        {
            return View();
        }

        // POST: Admin/Categories/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateCategory([Bind("Name")] Category category)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await Task.Run(() => _categoryService.CreateCategory(category)); // Simulate async call if necessary
                    return RedirectToAction(nameof(Categories));
                }
                catch (Exception ex)
                {
                    ViewBag.ErrorMessage = ex.Message;
                }
            }
            return View(category);
        }

        // GET: Admin/Categories/Edit/5
        public async Task<IActionResult> EditCategory(int id)
        {
            if (id == 0)
            {
                return NotFound();
            }

            try
            {
                var category = await _categoryService.GetCategoryById(id);
                if (category == null)
                {
                    return NotFound();
                }
                return View(category);
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = ex.Message;
                return View(new Category());
            }
        }

        // POST: Admin/Categories/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditCategory(int id, [Bind("Id,Name")] Category category)
        {
            if (id != category.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await Task.Run(() => _categoryService.UpdateCategory(category)); // Simulate async call if necessary
                    return RedirectToAction(nameof(Categories));
                }
                catch (Exception ex)
                {
                    ViewBag.ErrorMessage = ex.Message;
                }
            }
            return View(category);
        }

        // GET: Admin/Categories/Delete/5
        public async Task<IActionResult> DeleteCategory(int id)
        {
            if (id == 0)
            {
                return NotFound();
            }

            try
            {
                var category = await _categoryService.GetCategoryById(id);
                if (category == null)
                {
                    return NotFound();
                }
                return View(category);
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = ex.Message;
                return View(new Category());
            }
        }

        // POST: Admin/Categories/Delete/5
        [HttpPost, ActionName("DeleteCategory")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteCategoryConfirmed(int id)
        {
            try
            {
                await Task.Run(() => _categoryService.DeleteCategory(id)); // Simulate async call if necessary
                return RedirectToAction(nameof(Categories));
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = ex.Message;
                return RedirectToAction(nameof(Categories)); // Redirect back with an error message
            }
        }

        #endregion

        #region Products

        // GET: Admin/Products
        public async Task<IActionResult> Products()
        {
            try
            {
                // Fetch all products and include related Category and ShoppingCartItems
                var allProducts = _productService.GetAllProducts(); // This should fetch products including related Category and ShoppingCartItems

                foreach (var product in allProducts)
                {
                    _context.Entry(product).Reference(p => p.Category).Load(); // Load Category
                    _context.Entry(product).Collection(p => p.ShoppingCartItems).Load(); // Load ShoppingCartItems

                    foreach (var item in product.ShoppingCartItems)
                    {
                        _context.Entry(item).Reference(sci => sci.Size).Load(); // Load Size for ShoppingCartItems
                    }
                }

                // If your service method does not include related data, manually load it here
                foreach (var product in allProducts)
                {
                    // Load related Category
                    _context.Entry(product).Reference(p => p.Category).Load();

                    // Load related ShoppingCartItems
                    _context.Entry(product).Collection(p => p.ShoppingCartItems).Load();

                    // Load Size for ShoppingCartItems if needed
                    foreach (var item in product.ShoppingCartItems)
                    {
                        _context.Entry(item).Reference(sci => sci.Size).Load();
                    }
                }

                ViewBag.Sizes = _sizeService.GetAllSizes();

                return View(allProducts);
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = ex.Message;
                return View(new List<Product>());
            }
        }

        // GET: Admin/Products/Create
        public async Task<IActionResult> CreateProduct()
        {
            try
            {
                ViewData["Categories"] = _categoryService.GetAllCategories();
                return View();
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = ex.Message;
                return View(new Product());
            }
        }

        // POST: Admin/Products/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateProduct(Product product, IFormFile imageFile)
        {
            try
            {
                string imagePath = null;

                // Handle file upload with ImageSharp resizing
                if (imageFile != null && imageFile.Length > 0)
                {
                    var fileNameWithoutExtension = Path.GetFileNameWithoutExtension(imageFile.FileName);
                    var extension = Path.GetExtension(imageFile.FileName);
                    var newFileName = $"{fileNameWithoutExtension}_300x300{extension}";
                    var folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images");

                    // Ensure the 'images' directory exists
                    if (!Directory.Exists(folderPath))
                    {
                        Directory.CreateDirectory(folderPath); // Create the directory if it doesn't exist
                    }

                    var targetFilePath = Path.Combine(folderPath, newFileName);

                    // Image processing and saving with ImageSharp
                    using (var image = SixLabors.ImageSharp.Image.Load(imageFile.OpenReadStream()))
                    {
                        image.Mutate(x => x.Resize(300, 300)); // Resize the image to 300x300

                        // Save the resized image to the 'images' directory
                        await using (var fileStream = new FileStream(targetFilePath, FileMode.Create))
                        {
                            image.Save(fileStream, new SixLabors.ImageSharp.Formats.Png.PngEncoder()); // Save as PNG
                        }
                    }

                    imagePath = $"/images/{newFileName}"; // Public URL for the image
                }

                // Set the ImagePath for the product
                product.ImagePath = imagePath;

                // Save the product
                _productService.CreateProduct(product);
                return RedirectToAction(nameof(Products));
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = ex.Message;
            }

            ViewData["Categories"] = _categoryService.GetAllCategories();
            return View(product);
        }

        // GET: Admin/Products/Edit/5
        public async Task<IActionResult> EditProduct(int id)
        {
            if (id == 0)
            {
                return NotFound();
            }

            try
            {
                var product = await _productService.GetProductById(id);
                if (product == null)
                {
                    return NotFound();
                }
                ViewData["Categories"] = _categoryService.GetAllCategories();
                return View(product);
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = ex.Message;
                return View(new Product());
            }
        }

        // POST: Admin/Products/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditProduct(int id, Product product, IFormFile imageFile)
        {
            if (id != product.Id)
            {
                return NotFound();
            }

            try
            {
                // Handle file upload with ImageSharp resizing
                if (imageFile != null && imageFile.Length > 0)
                {
                    var fileNameWithoutExtension = Path.GetFileNameWithoutExtension(imageFile.FileName);
                    var extension = Path.GetExtension(imageFile.FileName);
                    var newFileName = $"{fileNameWithoutExtension}_300x300{extension}";
                    var folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images");

                    // Ensure the 'images' directory exists
                    if (!Directory.Exists(folderPath))
                    {
                        Directory.CreateDirectory(folderPath); // Create the directory if it doesn't exist
                    }

                    var targetFilePath = Path.Combine(folderPath, newFileName);

                    // Image processing and saving with ImageSharp
                    using (var image = SixLabors.ImageSharp.Image.Load(imageFile.OpenReadStream()))
                    {
                        image.Mutate(x => x.Resize(300, 300)); // Resize the image to 300x300

                        // Save the resized image to the 'images' directory
                        await using (var fileStream = new FileStream(targetFilePath, FileMode.Create))
                        {
                            image.Save(fileStream, new SixLabors.ImageSharp.Formats.Png.PngEncoder()); // Save as PNG
                        }
                    }

                    product.ImagePath = $"/images/{newFileName}"; // Public URL for the image
                }

                // Update the product
                _productService.UpdateProduct(product);
                return RedirectToAction(nameof(Products));
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = ex.Message;
            }

            ViewData["Categories"] = _categoryService.GetAllCategories();
            return View(product);
        }

        // GET: Admin/Categories/Delete/5
        public async Task<IActionResult> DeleteProduct(int id)
        {
            if (id == 0)
            {
                return NotFound();
            }

            try
            {
                var category = await _productService.GetProductById(id);
                if (category == null)
                {
                    return NotFound();
                }
                return View(category);
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = ex.Message;
                return View(new Category());
            }
        }

        // POST: Admin/Categories/Delete/5
        [HttpPost, ActionName("DeleteCategory")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteProductConfirmed(int id)
        {
            try
            {
                await Task.Run(() => _productService.GetProductById(id)); // Simulate async call if necessary
                return RedirectToAction(nameof(Products));
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = ex.Message;
                return RedirectToAction(nameof(Products)); // Redirect back with an error message
            }
        }

        #endregion

        #region User

        public IActionResult Home()
        {
            return View();
        }

        #endregion
    }
}
