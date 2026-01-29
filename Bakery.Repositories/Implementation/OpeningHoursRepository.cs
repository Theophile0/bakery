using Bakery.Data;
using Bakery.Domains.Data.Entities;
using Bakery.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bakery.Repositories.Implementation
{
    public sealed class OpeningHoursRepository : IOpeningHoursRepository
    {
        private readonly ApplicationDbContext _db;

        public OpeningHoursRepository(ApplicationDbContext db) => _db = db;

        public async Task<IReadOnlyList<OpeningHours>> GetWeeklyAsync(CancellationToken ct = default)
            => await _db.OpeningHours
                .Include(o => o.Intervals)
                .AsNoTracking()
                .OrderBy(o => o.DayOfWeek)
                .ToListAsync(ct);

        public Task<OpeningHours?> GetByDayAsync(byte dayOfWeek, CancellationToken ct = default)
            => _db.OpeningHours
                .Include(o => o.Intervals)
                .FirstOrDefaultAsync(o => o.DayOfWeek == dayOfWeek, ct);

        public async Task AddAsync(OpeningHours hours, CancellationToken ct = default)
        {
            _db.OpeningHours.Add(hours);
            await _db.SaveChangesAsync(ct);
        }

        public async Task UpdateAsync(OpeningHours hours, CancellationToken ct = default)
        {
            _db.OpeningHours.Update(hours);
            await _db.SaveChangesAsync(ct);
        }

        public async Task DeleteIntervalsForDayAsync(int openingHoursId, CancellationToken ct = default)
        {
            var intervals = await _db.OpeningHoursIntervals
                .Where(i => i.OpeningHoursId == openingHoursId)
                .ToListAsync(ct);

            _db.OpeningHoursIntervals.RemoveRange(intervals);
            await _db.SaveChangesAsync(ct);
        }
    }
}
