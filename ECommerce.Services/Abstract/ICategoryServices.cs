using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Services.Abstract
{
    public interface ICategoryServices<T> where T : class 
    {
        T CreateCategory(T category);
        T UpdateCategory(T category);
        void DeleteCategory(int categoryId);
        Task<T> GetCategoryById(int categoryId);
        IEnumerable<T> GetAllCategories();
    }
}
