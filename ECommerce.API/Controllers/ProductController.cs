using ECommerce.Entities;
using ECommerce.Repositories.Context;
using ECommerce.Services.Abstract;
using ECommerce.Services.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Png;


namespace ECommerce.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductServices<Product> productServices;
        private readonly ICategoryServices<Category> categoryServices;
        private readonly ECommerceDbContext context;

        public ProductController(IProductServices<Product> _productServices, ECommerceDbContext _context, ICategoryServices<Category> _categoryServices)
        {
            productServices = _productServices;
            context = _context;
            categoryServices = _categoryServices;
        }

        [HttpGet, Route("[action]")]
        public IActionResult GetAllProducts()
        {
            var products = productServices.GetAllProducts();

            if (products is not null)
                return Ok(products);
            else
                return BadRequest();
        }

        [HttpGet, Route("[action]/{productId}")]
        public async Task<IActionResult> GetByProductById(int productId)
        {
            var product = await productServices.GetProductById(productId);

            if (product is not null)
                return Ok(product);
            else
                return NotFound();
        }

        [HttpPost, Route("[action]")]
        public IActionResult CreateProduct([FromForm] ProductCreateDTO productCreateDTO)
        {
            if (productCreateDTO == null)
            {
                return BadRequest("Product is null");
            }

            try
            {
                // Check if there is a category
                var category = context.Categories.Find(productCreateDTO.CategoryId);
                if (category == null)
                {
                    return BadRequest("Category not found");
                }

                // Save image file
                string imagePath = null;
                if (productCreateDTO.Image != null && productCreateDTO.Image.Length > 0)
                {
                    var fileName = Path.GetFileNameWithoutExtension(productCreateDTO.Image.FileName);
                    var extension = Path.GetExtension(productCreateDTO.Image.FileName);
                    var newFileName = $"{fileName}_300x300{extension}";
                    var targetFilePath = Path.Combine(Directory.GetCurrentDirectory(), "images", newFileName);

                    // Image processing and saving with ImageSharp
                    using (var image = SixLabors.ImageSharp.Image.Load(productCreateDTO.Image.OpenReadStream()))
                    {
                        image.Mutate(x => x.Resize(300, 300)); // File size

                        // Save image to 'images' directory
                        using (var fileStream = new FileStream(targetFilePath, FileMode.Create))
                        {
                            image.Save(fileStream, new PngEncoder()); // We save using PNG encoder
                        }
                    }

                    imagePath = $"/images/{newFileName}";
                }

                // Create product entity from DTO
                var product = new Product
                {
                    Name = productCreateDTO.Name,
                    Description = productCreateDTO.Description,
                    Price = productCreateDTO.Price,
                    CategoryId = productCreateDTO.CategoryId,
                    Category = category,
                    ImagePath = imagePath
                };

                // Add new product
                context.Products.Add(product);
                context.SaveChanges();

                return CreatedAtAction("GetByProductById", new { productId = product.Id }, product);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message} - {ex.InnerException?.Message}");
            }
        }

        [HttpPut, Route("[action]")]
        public IActionResult UpdateProduct([FromForm] ProductUpdateDTO productUpdateDTO)
        {
            #region Example Update API Request Body
            //{
            //    "id": 1,
            //    "name": "UpdatedProduct",
            //    "description": "This is an updated sample product description.",
            //    "price": 29.99,
            //    "CategoryId": 2,
            //    "Image": <image file>
            //}
            #endregion

            if (productUpdateDTO == null)
            {
                return BadRequest("Product is null");
            }

            try
            {
                var product = context.Products.Find(productUpdateDTO.Id);
                if (product == null)
                {
                    return NotFound("Product not found");
                }

                // Check if there is a category
                var category = context.Categories.Find(productUpdateDTO.CategoryId);
                if (category == null)
                {
                    return BadRequest("Category not found");
                }

                // Save image file
                string imagePath = product.ImagePath; // Current image path
                if (productUpdateDTO.Image != null && productUpdateDTO.Image.Length > 0)
                {
                    var fileName = Path.GetFileNameWithoutExtension(productUpdateDTO.Image.FileName);
                    var extension = Path.GetExtension(productUpdateDTO.Image.FileName);
                    var newFileName = $"{fileName}_300x300{extension}";
                    var targetFilePath = Path.Combine(Directory.GetCurrentDirectory(), "images", newFileName);

                    // Image processing and saving with ImageSharp
                    using (var image = SixLabors.ImageSharp.Image.Load(productUpdateDTO.Image.OpenReadStream()))
                    {
                        image.Mutate(x => x.Resize(300, 300)); // Resize image to 300x300

                        // Save image to 'images' directory
                        using (var fileStream = new FileStream(targetFilePath, FileMode.Create))
                        {
                            image.Save(fileStream, new PngEncoder()); // We save using PNG encoder
                        }
                    }

                    imagePath = $"/images/{newFileName}";
                }

                // Update product features
                product.Name = productUpdateDTO.Name;
                product.Description = productUpdateDTO.Description;
                product.Price = productUpdateDTO.Price;
                product.CategoryId = productUpdateDTO.CategoryId;
                product.Category = category;
                product.ImagePath = imagePath;

                context.Products.Update(product);
                context.SaveChanges();

                return Ok(product);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message} - {ex.InnerException?.Message}");
            }
        }

        [HttpDelete]
        [Route("[action]/{productId}")]
        public IActionResult DeleteProduct(int productId)
        {
            productServices.DeleteProduct(productId);
            return Ok("Product deleted...");
        }
    }
}
