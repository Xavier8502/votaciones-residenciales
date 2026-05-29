using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using VotacionesResidenciales.Domain.Entities;
using VotacionesResidenciales.Domain.Enums;
using VotacionesResidenciales.Domain.Interfaces.Repositories;

namespace VotacionesResidenciales.Infrastructure.Persistence.Repositories
{
    public class InmuebleRepository : BaseRepository<Inmueble>,
    IInmuebleRepository
    {
        public InmuebleRepository(AppDbContext context) : base(context) { }

        public async Task<IEnumerable<Inmueble>> ObtenerPorConjuntoAsync(
            Guid conjuntoId, CancellationToken ct = default)
            => await _dbSet
                .Where(i => i.ConjuntoId == conjuntoId)
                .ToListAsync(ct);

        public async Task<Inmueble?> ObtenerConResidentesAsync(Guid id,
            CancellationToken ct = default)
            => await _dbSet
                .Include(i => i.Residentes)
                .FirstOrDefaultAsync(i => i.Id == id, ct);

        public async Task<IEnumerable<Inmueble>> ObtenerPorTipoAsync(
            Guid conjuntoId, TipoInmueble tipo,
            CancellationToken ct = default)
            => await _dbSet
                .Where(i => i.ConjuntoId == conjuntoId && i.Tipo == tipo)
                .ToListAsync(ct);

        public async Task<bool> ExisteNumeroEnConjuntoAsync(
            Guid conjuntoId, string numero,
            CancellationToken ct = default)
            => await _dbSet
                .AnyAsync(i => i.ConjuntoId == conjuntoId
                    && i.Numero == numero, ct);

        public async Task<decimal> SumarCoeficientesAsync(
            Guid conjuntoId, CancellationToken ct = default)
            => await _dbSet
                .Where(i => i.ConjuntoId == conjuntoId && i.Activo)
                .SumAsync(i => i.Coeficiente, ct);
    }
}
