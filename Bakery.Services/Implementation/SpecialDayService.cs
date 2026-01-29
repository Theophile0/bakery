using Bakery.Domains.Data.Entities;
using Bakery.Repositories.Interfaces;
using Bakery.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bakery.Services.Implementation
{
    public sealed class SpecialDayService : ISpecialDayService
    {
        private readonly ISpecialDayRepository _repo;

        public SpecialDayService(ISpecialDayRepository repo) => _repo = repo;

        public Task<IReadOnlyList<SpecialDay>> GetRangeAsync(DateOnly from, DateOnly to, CancellationToken ct = default)
            => _repo.GetRangeAsync(from, to, ct);

        public Task<SpecialDay?> GetByDateAsync(DateOnly date, CancellationToken ct = default)
            => _repo.GetByDateAsync(date, ct);

        public async Task UpsertAsync(
            DateOnly date,
            bool closedAllDay,
            string? note,
            IReadOnlyList<(TimeOnly open, TimeOnly close)> intervals,
            CancellationToken ct = default)
        {
            if (!closedAllDay)
            {
                // if open override, require at least one interval
                if (intervals.Count == 0) throw new ArgumentException("Provide at least one interval when not closed all day.");
                foreach (var (open, close) in intervals)
                    if (close <= open) throw new ArgumentException("CloseTime must be after OpenTime.");
            }

            var existing = await _repo.GetByDateAsync(date, ct);

            if (existing is null)
            {
                existing = new SpecialDay
                {
                    Date = date,
                    IsClosedAllDay = closedAllDay,
                    Note = note,
                    CreatedAt = DateTime.UtcNow
                };
                await _repo.AddAsync(existing, ct);
            }
            else
            {
                existing.IsClosedAllDay = closedAllDay;
                existing.Note = note;
                existing.UpdatedAt = DateTime.UtcNow;
                await _repo.UpdateAsync(existing, ct);
            }

            // Replace intervals
            await _repo.DeleteIntervalsAsync(existing.Id, ct);

            if (!closedAllDay)
            {
                existing.Intervals = intervals.Select(x => new SpecialDayInterval
                {
                    SpecialDayId = existing.Id,
                    OpenTime = x.open,
                    CloseTime = x.close
                }).ToList();

                await _repo.UpdateAsync(existing, ct);
            }
        }

        public async Task DeleteAsync(DateOnly date, CancellationToken ct = default)
        {
            var existing = await _repo.GetByDateAsync(date, ct);
            if (existing is null) return;

            await _repo.DeleteAsync(existing, ct);
        }
    }
}
