using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bakery.Domains.Data.Entities
{
    public class ProductCategory
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int SortOrder { get; set; } = 0;
        public bool IsActive { get; set; }

        public ICollection<Product> Products { get; set; } = new List<Product>();
    }
}
