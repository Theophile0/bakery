using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bakery.Domains.Data.Entities
{
    public class OpeningHours
    {
        public int Id { get; set; }

        /// <summary>
        /// 0=Monday ... 6=Sunday
        /// </summary>
        public byte DayOfWeek { get; set; }

        public bool IsClosed { get; set; } = false;

        public ICollection<OpeningHoursInterval> Intervals { get; set; } = new List<OpeningHoursInterval>();
    }
}
