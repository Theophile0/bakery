using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bakery.Domains.Data.Entities
{
    public class ProductImage
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public string ImageUrl { get; set; } = null!;
        public string? AltText { get; set; }
        public bool IsPrimary { get; set; } = false;
        public int SortOrder { get; set; } = 0;

        public Product Product { get; set; } = null!;
    }
}
