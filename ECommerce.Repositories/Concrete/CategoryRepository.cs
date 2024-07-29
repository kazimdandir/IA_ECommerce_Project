using ECommerce.Entities;
using ECommerce.Repositories.Abstract;
using ECommerce.Repositories.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Repositories.Concrete
{
    public class CategoryRepository : ICategoryRepository<Category>
    {
        private readonly ECommerceDbContext context;

        public CategoryRepository(ECommerceDbContext _context)
        {
            context = _context;
        }

        public Category CreateCategory(Category category)
        {
            context.Categories.Add(category);
            context.SaveChanges();
            return category;
        }

        public void DeleteCategory(int categoryId)
        {
            var category = context.Categories.FirstOrDefault(c => c.Id == categoryId);
            context.Categories.Remove(category);
            context.SaveChanges();
        }

        public IEnumerable<Category> GetAllCategories()
        {
            return context.Categories.ToList();
        }

        public async Task<Category> GetCategoryById(int categoryId)
        {
            return await context.Categories.FindAsync(categoryId);
        }

        public Category UpdateCategory(Category category)
        {
            context.Categories.Update(category);
            context.SaveChanges();
            return category;
        }
    }
}
