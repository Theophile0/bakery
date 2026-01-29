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
    public sealed class SpecialDayRepository : ISpecialDayRepository
    {
        private readonly ApplicationDbContext _db;

        public SpecialDayRepository(ApplicationDbContext db) => _db = db;

        public async Task<IReadOnlyList<SpecialDay>> GetRangeAsync(DateOnly from, DateOnly to, CancellationToken ct = default)
            => await _db.SpecialDays
                .Include(s => s.Intervals)
                .AsNoTracking()
                .Where(s => s.Date >= from && s.Date <= to)
                .OrderBy(s => s.Date)
                .ToListAsync(ct);

        public Task<SpecialDay?> GetByDateAsync(DateOnly date, CancellationToken ct = default)
            => _db.SpecialDays
                .Include(s => s.Intervals)
                .FirstOrDefaultAsync(s => s.Date == date, ct);

        public async Task AddAsync(SpecialDay day, CancellationToken ct = default)
        {
            _db.SpecialDays.Add(day);
            await _db.SaveChangesAsync(ct);
        }

        public async Task UpdateAsync(SpecialDay day, CancellationToken ct = default)
        {
            _db.SpecialDays.Update(day);
            await _db.SaveChangesAsync(ct);
        }

        public async Task DeleteAsync(SpecialDay day, CancellationToken ct = default)
        {
            _db.SpecialDays.Remove(day);
            await _db.SaveChangesAsync(ct);
        }

        public async Task DeleteIntervalsAsync(int specialDayId, CancellationToken ct = default)
        {
            var intervals = await _db.SpecialDayIntervals
                .Where(i => i.SpecialDayId == specialDayId)
                .ToListAsync(ct);

            _db.SpecialDayIntervals.RemoveRange(intervals);
            await _db.SaveChangesAsync(ct);
        }
    }
}
