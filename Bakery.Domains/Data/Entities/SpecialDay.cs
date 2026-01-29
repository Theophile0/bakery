using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bakery.Domains.Data.Entities
{
    public class SpecialDay
    {
        public int Id { get; set; }

        public DateOnly Date { get; set; }

        public bool IsClosedAllDay { get; set; } = false;

        /// <summary>
        /// Optional note like "Vacation" / "Public holiday"
        /// </summary>
        public string? Note { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }

        // Nav
        public ICollection<SpecialDayInterval> Intervals { get; set; } = new List<SpecialDayInterval>();
    }
}
