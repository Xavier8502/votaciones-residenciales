using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using VotacionesResidenciales.Domain.Entities;
using VotacionesResidenciales.Domain.Interfaces.Repositories;

namespace VotacionesResidenciales.Infrastructure.Persistence.Repositories
{
    public class AdminConjuntoRepository
    : BaseRepository<AdminConjunto>, IAdminConjuntoRepository
    {
        public AdminConjuntoRepository(AppDbContext context)
            : base(context) { }

        public async Task<AdminConjunto?> ObtenerPorCedulaAsync(
            string cedula, CancellationToken ct = default)
            => await _dbSet
                .Include(a => a.Conjunto)
                .FirstOrDefaultAsync(a => a.Cedula == cedula, ct);

        public async Task<AdminConjunto?> ObtenerPorConjuntoAsync(
            Guid conjuntoId, CancellationToken ct = default)
            => await _dbSet
                .FirstOrDefaultAsync(a => a.ConjuntoId == conjuntoId, ct);

        public async Task<bool> ExisteCedulaAsync(
            string cedula, CancellationToken ct = default)
            => await _dbSet
                .AnyAsync(a => a.Cedula == cedula, ct);
    }
}
