using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bakery.Domains.Data.Entities
{
    public class OpeningHoursInterval
    {
        public int Id { get; set; }

        public int OpeningHoursId { get; set; }

        public TimeOnly OpenTime { get; set; }
        public TimeOnly CloseTime { get; set; }

        public OpeningHours OpeningHours { get; set; } = null!;
    }
}
