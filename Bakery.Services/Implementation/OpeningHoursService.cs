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
    public sealed class OpeningHoursService : IOpeningHoursService
    {
        private readonly IOpeningHoursRepository _openingHoursRepository;

        public OpeningHoursService(IOpeningHoursRepository openingHoursService) => _openingHoursRepository = openingHoursService;

        public Task<IReadOnlyList<OpeningHours>> GetWeeklyAsync(CancellationToken ct = default)
            => _openingHoursRepository.GetWeeklyAsync(ct);

    }
}
