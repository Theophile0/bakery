using Bakery.Domains.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bakery.Services.Interfaces
{
    public interface ISpecialDayService
    {
        Task<IReadOnlyList<SpecialDay>> GetRangeAsync(DateOnly from, DateOnly to, CancellationToken ct = default);
        Task<SpecialDay?> GetByDateAsync(DateOnly date, CancellationToken ct = default);

        /// <summary>
        /// Creates or updates an exception day. If closedAllDay=true, intervals are cleared.
        /// If closedAllDay=false, intervals replace existing intervals (can be empty).
        /// </summary>
        Task UpsertAsync(DateOnly date, bool closedAllDay, string? note, IReadOnlyList<(TimeOnly open, TimeOnly close)> intervals, CancellationToken ct = default);

        Task DeleteAsync(DateOnly date, CancellationToken ct = default);
    }
}
