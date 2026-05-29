using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using VotacionesResidenciales.Domain.Entities;
using VotacionesResidenciales.Domain.Interfaces.Repositories;

namespace VotacionesResidenciales.Infrastructure.Persistence.Repositories
{
    public class ConjuntoRepository : BaseRepository<Conjunto>,
    IConjuntoRepository
    {
        public ConjuntoRepository(AppDbContext context) : base(context) { }

        public async Task<Conjunto?> ObtenerPorNitAsync(string nit,
            CancellationToken ct = default)
            => await _dbSet
                .FirstOrDefaultAsync(c => c.Nit == nit, ct);

        public async Task<Conjunto?> ObtenerConInmueblesAsync(Guid id,
            CancellationToken ct = default)
            => await _dbSet
                .Include(c => c.Inmuebles)
                .FirstOrDefaultAsync(c => c.Id == id, ct);

        public async Task<bool> ExisteNitAsync(string nit,
            CancellationToken ct = default)
            => await _dbSet
                .AnyAsync(c => c.Nit == nit, ct);
    }
}
