using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bakery.Domains.Data.Entities
{
    public class SpecialDayInterval
    {
        public int Id { get; set; }

        public int SpecialDayId { get; set; }

        public TimeOnly OpenTime { get; set; }
        public TimeOnly CloseTime { get; set; }

        // Nav
        public SpecialDay SpecialDay { get; set; } = null!;
    }
}
