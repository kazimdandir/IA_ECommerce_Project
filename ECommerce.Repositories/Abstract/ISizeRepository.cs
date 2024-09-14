using ECommerce.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Repositories.Abstract
{
    public interface ISizeRepository<T> where T : class
    {
        IEnumerable<Sizes> GetAllSizes();
        Sizes GetSizeById(int id);
    }
}
