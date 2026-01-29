using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bakery.Domains.Data.Entities
{
    public class Product
    {
        public int Id { get; set; }
        public int CategoryId {  get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }

        public decimal Price { get; set; }
        public bool IsActive { get; set; } = true;

        public int SortOrder { get; set; } = 0;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }


        public ProductCategory Category { get; set; } = null!;
        public ICollection<ProductImage> Images { get; set; } = new List<ProductImage>();
    }
}
