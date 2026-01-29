using Bakery.Domains.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bakery.Services.Interfaces
{
    public interface IOpeningHoursService
    {
        Task<IReadOnlyList<OpeningHours>> GetWeeklyAsync(CancellationToken ct = default);
    }
}
