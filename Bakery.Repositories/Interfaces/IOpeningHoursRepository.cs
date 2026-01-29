using Bakery.Domains.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bakery.Repositories.Interfaces
{
    public interface IOpeningHoursRepository
    {
        Task<IReadOnlyList<OpeningHours>> GetWeeklyAsync(CancellationToken ct = default); // includes intervals
        Task<OpeningHours?> GetByDayAsync(byte dayOfWeek, CancellationToken ct = default); // includes intervals

        Task AddAsync(OpeningHours hours, CancellationToken ct = default);
        Task UpdateAsync(OpeningHours hours, CancellationToken ct = default);
        Task DeleteIntervalsForDayAsync(int openingHoursId, CancellationToken ct = default);
    }
}
