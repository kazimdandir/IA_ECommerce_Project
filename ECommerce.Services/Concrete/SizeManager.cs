using ECommerce.Entities;
using ECommerce.Repositories.Abstract;
using ECommerce.Repositories.Concrete;
using ECommerce.Repositories.Context;
using ECommerce.Services.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Services.Concrete
{
    public class SizeManager : ISizeServices<Sizes>
    {
        public readonly ISizeRepository<Sizes> sizeRepository;

        public SizeManager(ISizeRepository<Sizes> _sizeRepository)
        {
            sizeRepository = _sizeRepository;
        }

        public IEnumerable<Sizes> GetAllSizes()
        {
            if (sizeRepository.GetAllSizes() is not null)
            {
                return sizeRepository.GetAllSizes();
            }
            else
            {
                throw new Exception("Size list is empty!");
            }
        }

        public Sizes GetSizeById(int id)
        {
            if (id > 0)
            {
                return sizeRepository.GetSizeById(id); // Get a specific size by ID using the repository
            }
            else
            {
                throw new Exception("ID parameter cannot be less than 1!");
            }
        }
    }
}
