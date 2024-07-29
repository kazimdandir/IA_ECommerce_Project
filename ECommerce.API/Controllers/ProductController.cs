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

        #region oldCreateProductMethod
        //[HttpPost, Route("[action]")]
        //public IActionResult CreateProduct([FromBody] Product product)
        //{

        #region Example Create API Request Body
        //{
        //    "name": "newProduct",
        //    "price": 19.99,
        //    "CategoryId": 1,
        //    "description": "This is a sample product description."
        //}
        #endregion

        //    if (product == null)
        //    {
        //        return BadRequest("Product is null");
        //    }

        //    //if (product.CategoryId <= 0)
        //    //{
        //    //    return BadRequest("Invalid CategoryId");
        //    //}

        //    try
        //    {
        //        // Ensure category exists
        //        // var category = categoryServices.GetCategoryById(product.CategoryId).Result;
        //        //if (category == null)
        //        //{
        //        //    return BadRequest("Category not found");
        //        //}

        //        var createdProduct = productServices.CreateProduct(product);
        //        return CreatedAtAction("GetProductById", new { id = createdProduct.Id }, createdProduct);
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(500, $"Internal server error: {ex.Message} - {ex.InnerException?.Message}");
        //    }
        //} 
        #endregion

        #region old2CreateProductMethod
        //[HttpPost, Route("[action]")]
        //public IActionResult CreateProduct([FromForm] ProductCreateDTO productCreateDTO)
        //{
        //    #region Example Create API Request Body
        //    //{
        //    //    "name": "newProduct",
        //    //    "price": 19.99,
        //    //    "CategoryId": 1,
        //    //    "description": "This is a sample product description."
        //    //}
        //    #endregion

        //    if (productCreateDTO == null)
        //    {
        //        return BadRequest("Product is null");
        //    }

        //    try
        //    {
        //        // Kategori var mı kontrol et
        //        var category = context.Categories.Find(productCreateDTO.CategoryId);
        //        if (category == null)
        //        {
        //            return BadRequest("Category not found");
        //        }

        //        // Resim dosyasını kaydet
        //        string imagePath = null;
        //        if (productCreateDTO.Image != null && productCreateDTO.Image.Length > 0)
        //        {
        //            var fileName = Path.GetFileName(productCreateDTO.Image.FileName);

        //            using (var stream = new FileStream(productCreateDTO.Image.FileName, FileMode.Create))
        //            {
        //                productCreateDTO.Image.CopyTo(stream);

        //            }
        //            var targetFilePath = Path.Combine(Directory.GetCurrentDirectory(), "images", productCreateDTO.Image.FileName);

        //            System.IO.File.Copy(fileName, targetFilePath, overwrite: true);

        //        }

        //        // DTO'dan ürün varlığı oluştur
        //        var product = new Product
        //        {
        //            Name = productCreateDTO.Name,
        //            Description = productCreateDTO.Description,
        //            Price = productCreateDTO.Price,
        //            CategoryId = productCreateDTO.CategoryId,
        //            Category = category,
        //            ImagePath = imagePath
        //        };

        //        // Yeni ürünü ekle
        //        context.Products.Add(product);
        //        context.SaveChanges();

        //        return CreatedAtAction("GetByProductById", new { productId = product.Id }, product);
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(500, $"Internal server error: {ex.Message} - {ex.InnerException?.Message}");
        //    }
        //}
        #endregion

        #region newCreateProductMethod
        [HttpPost, Route("[action]")]
        public IActionResult CreateProduct([FromForm] ProductCreateDTO productCreateDTO)
        {
            if (productCreateDTO == null)
            {
                return BadRequest("Product is null");
            }

            try
            {
                // Kategori var mı kontrol et
                var category = context.Categories.Find(productCreateDTO.CategoryId);
                if (category == null)
                {
                    return BadRequest("Category not found");
                }

                // Resim dosyasını kaydet
                string imagePath = null;
                if (productCreateDTO.Image != null && productCreateDTO.Image.Length > 0)
                {
                    var fileName = Path.GetFileNameWithoutExtension(productCreateDTO.Image.FileName);
                    var extension = Path.GetExtension(productCreateDTO.Image.FileName);
                    var newFileName = $"{fileName}_300x300{extension}";
                    var targetFilePath = Path.Combine(Directory.GetCurrentDirectory(), "images", newFileName);

                    // ImageSharp ile resim işleme ve kaydetme
                    using (var image = SixLabors.ImageSharp.Image.Load(productCreateDTO.Image.OpenReadStream()))
                    {
                        image.Mutate(x => x.Resize(300, 300)); // Dosya boyutu 

                        // Resmi 'images' dizinine kaydet
                        using (var fileStream = new FileStream(targetFilePath, FileMode.Create))
                        {
                            image.Save(fileStream, new PngEncoder()); // PNG encoder kullanarak kaydediyoruz
                        }
                    }

                    imagePath = $"/images/{newFileName}";
                }

                // DTO'dan ürün varlığı oluştur
                var product = new Product
                {
                    Name = productCreateDTO.Name,
                    Description = productCreateDTO.Description,
                    Price = productCreateDTO.Price,
                    CategoryId = productCreateDTO.CategoryId,
                    Category = category,
                    ImagePath = imagePath
                };

                // Yeni ürünü ekle
                context.Products.Add(product);
                context.SaveChanges();

                return CreatedAtAction("GetByProductById", new { productId = product.Id }, product);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message} - {ex.InnerException?.Message}");
            }
        }
        #endregion

        #region oldUpdateProductMethod
        //[HttpPut, Route("[action]")]
        //public IActionResult UpdateProduct([FromBody] Product product)
        //{
        //    #region Example Update API Request Body
        //    //{
        //    //    "id": 4,
        //    //    "name": "updatedProduct"
        //    //}
        //    #endregion
        //    return Ok(productServices.UpdateProduct(product));
        //} 
        #endregion

        #region old2UpdateProductMethod
        //[HttpPut, Route("[action]")]
        //public IActionResult UpdateProduct([FromBody] ProductUpdateDTO productUpdateDTO)
        //{
        //    #region Example Update API Request Body
        //    //{
        //    //    "id": 1,
        //    //    "name": "UpdatedProduct",
        //    //    "description": "This is an updated sample product description.",
        //    //    "price": 29.99,
        //    //    "CategoryId": 2
        //    //}
        //    #endregion

        //    if (productUpdateDTO == null)
        //    {
        //        return BadRequest("Product is null");
        //    }

        //    try
        //    {
        //        var product = context.Products.Find(productUpdateDTO.Id);
        //        if (product == null)
        //        {
        //            return NotFound("Product not found");
        //        }

        //        // Save image to server
        //        if (productUpdateDTO.Image != null)
        //        {
        //            var fileName = Path.GetFileName(productUpdateDTO.Image.FileName);
        //            var filePath = Path.Combine("wwwroot/images", fileName);

        //            using (var stream = new FileStream(filePath, FileMode.Create))
        //            {
        //                productUpdateDTO.Image.CopyTo(stream);
        //            }

        //            product.ImagePath = $"/images/{fileName}";
        //        }

        //        // Update product properties
        //        product.Name = productUpdateDTO.Name;
        //        product.Description = productUpdateDTO.Description;
        //        product.Price = productUpdateDTO.Price;
        //        product.CategoryId = productUpdateDTO.CategoryId;

        //        context.Products.Update(product);
        //        context.SaveChanges();

        //        return Ok(product);
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(500, $"Internal server error: {ex.Message} - {ex.InnerException?.Message}");
        //    }
        //}
        #endregion

        #region newUpdateProductMethod
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
            //    "Image": <resim dosyası>
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

                // Kategori var mı kontrol et
                var category = context.Categories.Find(productUpdateDTO.CategoryId);
                if (category == null)
                {
                    return BadRequest("Category not found");
                }

                // Resim dosyasını güncelle ve kaydet
                string imagePath = product.ImagePath; // Mevcut resim yolu
                if (productUpdateDTO.Image != null && productUpdateDTO.Image.Length > 0)
                {
                    var fileName = Path.GetFileNameWithoutExtension(productUpdateDTO.Image.FileName);
                    var extension = Path.GetExtension(productUpdateDTO.Image.FileName);
                    var newFileName = $"{fileName}_300x300{extension}";
                    var targetFilePath = Path.Combine(Directory.GetCurrentDirectory(), "images", newFileName);

                    // ImageSharp ile resim işleme ve kaydetme
                    using (var image = SixLabors.ImageSharp.Image.Load(productUpdateDTO.Image.OpenReadStream()))
                    {
                        image.Mutate(x => x.Resize(300, 300)); // Resmi 300x300 boyutuna yeniden boyutlandır

                        // Resmi 'images' dizinine kaydet
                        using (var fileStream = new FileStream(targetFilePath, FileMode.Create))
                        {
                            image.Save(fileStream, new PngEncoder()); // PNG encoder kullanarak kaydediyoruz
                        }
                    }

                    imagePath = $"/images/{newFileName}";
                }

                // Ürün özelliklerini güncelle
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

        #endregion

        [HttpDelete]
        [Route("[action]/{productId}")]
        public IActionResult DeleteProduct(int productId)
        {
            productServices.DeleteProduct(productId);
            return Ok("Product deleted...");
        }
    }
}
