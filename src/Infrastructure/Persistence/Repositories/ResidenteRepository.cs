using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using VotacionesResidenciales.Domain.Entities;
using VotacionesResidenciales.Domain.Interfaces.Repositories;

namespace VotacionesResidenciales.Infrastructure.Persistence.Repositories
{
    public class ResidenteRepository : BaseRepository<Residente>,
    IResidenteRepository
    {
        public ResidenteRepository(AppDbContext context) : base(context) { }

        public async Task<Residente?> ObtenerPorCedulaAsync(string cedula,
            CancellationToken ct = default)
            => await _dbSet
                .Include(r => r.Inmueble)
                .FirstOrDefaultAsync(r => r.Cedula == cedula, ct);

        public async Task<IEnumerable<Residente>> ObtenerPorConjuntoAsync(
            Guid conjuntoId, CancellationToken ct = default)
            => await _dbSet
                .Include(r => r.Inmueble)
                .Where(r => r.Inmueble != null
                    && r.Inmueble.ConjuntoId == conjuntoId)
                .ToListAsync(ct);
    }
}
