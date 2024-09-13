using ECommerce.Entities;
using ECommerce.Services.Abstract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.UI.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly ICategoryServices<Category> _categoryService;
        private readonly IProductServices<Product> _productService;

        public AdminController(ICategoryServices<Category> categoryService, IProductServices<Product> productService)
        {
            _categoryService = categoryService;
            _productService = productService;
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
                var products = _productService.GetAllProducts();
                return View(products);
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
        public async Task<IActionResult> CreateProduct([Bind("Name,Price,CategoryId,ImagePath,DetailsCare")] Product product)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await Task.Run(() => _productService.CreateProduct(product)); // Simulate async call if necessary
                    return RedirectToAction(nameof(Products));
                }
                catch (Exception ex)
                {
                    ViewBag.ErrorMessage = ex.Message;
                }
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
        public async Task<IActionResult> EditProduct(int id, [Bind("Id,Name,Price,CategoryId,ImagePath,DetailsCare")] Product product)
        {
            if (id != product.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await Task.Run(() => _productService.UpdateProduct(product)); // Simulate async call if necessary
                    return RedirectToAction(nameof(Products));
                }
                catch (Exception ex)
                {
                    ViewBag.ErrorMessage = ex.Message;
                }
            }
            ViewData["Categories"] = _categoryService.GetAllCategories();
            return View(product);
        }

        // GET: Admin/Products/Delete/5
        public async Task<IActionResult> DeleteProduct(int id)
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
                return View(product);
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = ex.Message;
                return View(new Product());
            }
        }

        // POST: Admin/Products/Delete/5
        [HttpPost, ActionName("DeleteProduct")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteProductConfirmed(int id)
        {
            try
            {
                await Task.Run(() => _productService.DeleteProduct(id)); // Simulate async call if necessary
                return RedirectToAction(nameof(Products));
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = ex.Message;
                return RedirectToAction(nameof(Products)); // Redirect back with an error message
            }
        }

        #endregion

        
    }
}
