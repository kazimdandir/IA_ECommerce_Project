using ECommerce.Entities;
using ECommerce.Repositories.Abstract;
using ECommerce.Repositories.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Repositories.Concrete
{
    public class SizeRepository : ISizeRepository<Sizes>
    {
        private readonly ECommerceDbContext context;

        public SizeRepository(ECommerceDbContext _context)
        {
            context = _context;
        }

        public IEnumerable<Sizes> GetAllSizes()
        {
            return context.Sizes.ToList();
        }

        public Sizes GetSizeById(int id)
        {
            return context.Sizes.Find(id); // Fetch a specific size by ID
        }
    }
}
