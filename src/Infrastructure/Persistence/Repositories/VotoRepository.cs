using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using VotacionesResidenciales.Domain.Entities;
using VotacionesResidenciales.Domain.Interfaces.Repositories;

namespace VotacionesResidenciales.Infrastructure.Persistence.Repositories
{
    public class VotoRepository : BaseRepository<Voto>, IVotoRepository
    {
        public VotoRepository(AppDbContext context) : base(context) { }

        public async Task<bool> YaVotoAsync(Guid residenteId,
            Guid votacionId, CancellationToken ct = default)
            => await _dbSet
                .AnyAsync(v => v.ResidenteId == residenteId
                    && v.VotacionId == votacionId, ct);

        public async Task<IEnumerable<Voto>> ObtenerPorVotacionAsync(
            Guid votacionId, CancellationToken ct = default)
            => await _dbSet
                .Include(v => v.Detalles)
                .Where(v => v.VotacionId == votacionId)
                .ToListAsync(ct);

        public async Task<int> ContarVotosPorVotacionAsync(
            Guid votacionId, CancellationToken ct = default)
            => await _dbSet
                .CountAsync(v => v.VotacionId == votacionId, ct);
    }
}
