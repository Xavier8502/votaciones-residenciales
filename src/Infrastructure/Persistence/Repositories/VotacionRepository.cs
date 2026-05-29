using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using VotacionesResidenciales.Domain.Entities;
using VotacionesResidenciales.Domain.Interfaces.Repositories;

namespace VotacionesResidenciales.Infrastructure.Persistence.Repositories
{
    public class VotacionRepository : BaseRepository<Votacion>,
      IVotacionRepository
    {
        public VotacionRepository(AppDbContext context) : base(context) { }

        public async Task<IEnumerable<Votacion>> ObtenerPorConjuntoAsync(
            Guid conjuntoId, CancellationToken ct = default)
            => await _dbSet
                .Include(v => v.Preguntas)
                .Where(v => v.ConjuntoId == conjuntoId)
                .ToListAsync(ct);

        public async Task<Votacion?> ObtenerConDetallesAsync(Guid id,
            CancellationToken ct = default)
            => await _dbSet
                .Include(v => v.Preguntas)
                    .ThenInclude(p => p.Opciones)
                .Include(v => v.Votos)
                    .ThenInclude(vo => vo.Detalles)
                .FirstOrDefaultAsync(v => v.Id == id, ct);
    }
}
