using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Entities
{
    public class Size
    {
        public int Id { get; set; }
        public string SizeName { get; set; }

        // Navigation property for ShoppingCartItems
        public ICollection<ShoppingCartItem> ShoppingCartItems { get; set; }
    }
}
