using ECommerce.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Services.Abstract
{
    public interface ISizeServices<T> where T : class
    {
        IEnumerable<Sizes> GetAllSizes();
        Sizes GetSizeById(int id);
    }
}
