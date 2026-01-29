using Bakery.Domains.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bakery.Repositories.Interfaces
{
    public interface ISpecialDayRepository
    {
        Task<IReadOnlyList<SpecialDay>> GetRangeAsync(DateOnly from, DateOnly to, CancellationToken ct = default); // includes intervals
        Task<SpecialDay?> GetByDateAsync(DateOnly date, CancellationToken ct = default); // includes intervals

        Task AddAsync(SpecialDay day, CancellationToken ct = default);
        Task UpdateAsync(SpecialDay day, CancellationToken ct = default);
        Task DeleteAsync(SpecialDay day, CancellationToken ct = default);
        Task DeleteIntervalsAsync(int specialDayId, CancellationToken ct = default);
    }
}
