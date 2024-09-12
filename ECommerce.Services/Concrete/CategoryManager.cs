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
    public class CategoryManager : ICategoryServices<Category>
    {
        public readonly ICategoryRepository<Category> categoryRepository;

        public CategoryManager(ICategoryRepository<Category> _categoryRepository)
        {
            categoryRepository = _categoryRepository;
        }

        public Category CreateCategory(Category category)
        {
            if (category is not null)
            {
                return categoryRepository.CreateCategory(category);
            }
            else
            {
                throw new Exception("Favorite cannot be empty!");
            }
        }

        public void DeleteCategory(int categoryId)
        {
            if (categoryId != 0)
            {
                categoryRepository.DeleteCategory(categoryId);
            }
            else
            {
                throw new Exception("ID must not be NULL during the deletion process!");
            }
        }

        public IEnumerable<Category> GetAllCategories()
        {
            if (categoryRepository.GetAllCategories() is not null)
            {
                return categoryRepository.GetAllCategories();
            }
            else
            {
                throw new Exception("Category list is empty!");
            }
        }

        public async Task<Category> GetCategoryById(int categoryId)
        {
            if (categoryId > 0)
            {
                return await categoryRepository.GetCategoryById(categoryId);
            }
            else
            {
                throw new Exception("ID parameter cannot be less than 1!");
            }
        }

        public Category UpdateCategory(Category category)
        {
            if (category is not null)
            {
                return categoryRepository.UpdateCategory(category);
            }
            else
            {
                throw new Exception("Category cannot be empty!");
            }
        }
    }
}
