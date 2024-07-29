using ECommerce.Entities;
using ECommerce.Repositories.Context;
using ECommerce.Services.Abstract;
using ECommerce.Services.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryServices<Category> categoryServices;
        private readonly ECommerceDbContext context;

        public CategoryController(ICategoryServices<Category> _categoryServices, ECommerceDbContext _context)
        {
            categoryServices = _categoryServices;
            context = _context;
        }

        [HttpGet, Route("[action]")]
        public IActionResult GetAllCategories()
        {
            var categories = categoryServices.GetAllCategories();

            if (categories is not null)
                return Ok(categories);
            else
                return BadRequest();
        }

        [HttpGet, Route("[action]/{categoryId}")]
        public async Task<IActionResult> GetCategoryById(int categoryId)
        {
            var category = await categoryServices.GetCategoryById(categoryId);

            if (category is not null)
                return Ok(category);
            else
                return NotFound();
        }

        #region oldCreateCategoryMethod
        //[HttpPost, Route("[action]")]
        //public IActionResult CreateCategory([FromBody] Category category)
        //{
        //    #region Example Request Body
        //    //{
        //    //    "name": "newCategory"
        //    //}
        //    #endregion

        //    var createdCategory = categoryServices.CreateCategory(category);
        //    try
        //    {
        //        CreatedAtAction("GetCategoryById", new { id = createdCategory.Id }, createdCategory);
        //        return Ok();
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //} 
        #endregion

        #region newCreateCategoryMethod
        [HttpPost, Route("[action]")]
        public IActionResult CreateCategory([FromBody] CategoryCreateDTO categoryCreateDTO)
        {
            #region Example Create API Request Body
            //{
            //    "name": "newCreatedCategory"
            //}
            #endregion

            if (categoryCreateDTO == null)
            {
                return BadRequest("Category is null");
            }

            try
            {
                // Create category entity from DTO
                var category = new Category
                {
                    Name = categoryCreateDTO.Name
                };

                // Add new category
                context.Categories.Add(category);
                context.SaveChanges();

                return CreatedAtAction("GetCategoryById", new { categoryId = category.Id }, category);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message} - {ex.InnerException?.Message}");
            }
        }
        #endregion

        #region oldUpdateCategoryMethod
        //[HttpPut, Route("[action]")]
        //public IActionResult UpdateCategory([FromBody] Category category)
        //{
        //    #region Example Request Body
        //    //{
        //    //    "id": 4,
        //    //    "name": "updatedCategory"
        //    //}
        //    #endregion
        //    return Ok(categoryServices.UpdateCategory(category));
        //} 
        #endregion

        #region newUpdateCategoryMethod
        [HttpPut, Route("[action]")]
        public IActionResult UpdateCategory([FromBody] CategoryUpdateDTO categoryUpdateDTO)
        {
            #region Example Update API Request Body
            //{
            //    "name": "newUpdateCategoryTest"
            //} 
            #endregion

            if (categoryUpdateDTO == null)
            {
                return BadRequest("Category is null");
            }

            try
            {
                // Find existing category
                var category = context.Categories.Find(categoryUpdateDTO.Id);
                if (category == null)
                {
                    return NotFound("Category not found");
                }

                // Update category entity from DTO
                category.Name = categoryUpdateDTO.Name;

                // Update category
                context.Categories.Update(category);
                context.SaveChanges();

                return Ok(category);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message} - {ex.InnerException?.Message}");
            }
        }
        #endregion

        [HttpDelete]
        [Route("[action]/{categoryId}")]
        public IActionResult DeleteCategory(int categoryId)
        {
            categoryServices.DeleteCategory(categoryId);
            return Ok("Category deleted...");
        }
    }
}