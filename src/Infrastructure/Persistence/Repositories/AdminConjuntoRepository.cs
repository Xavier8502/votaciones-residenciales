
using Microsoft.EntityFrameworkCore;
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
            string cedula,
            CancellationToken ct = default)
        {
            return await _dbSet
                .Include(a => a.Asignaciones)
                    .ThenInclude(asignacion => asignacion.Conjunto)
                .FirstOrDefaultAsync(a => a.Cedula == cedula, ct);
        }

        public async Task<AdminConjunto?> ObtenerPorConjuntoAsync(
            Guid conjuntoId,
            CancellationToken ct = default)
        {
            return await _dbSet
                .Include(a => a.Asignaciones)
                .FirstOrDefaultAsync(
                    a => a.Asignaciones.Any(
                        asignacion => asignacion.ConjuntoId == conjuntoId),
                    ct);
        }

        public async Task<IReadOnlyList<AdminConjunto>>
            ObtenerTodosPorConjuntoAsync(
                Guid conjuntoId,
                CancellationToken ct = default)
        {
            return await _dbSet
                .Where(a => a.Asignaciones.Any(
                    asignacion => asignacion.ConjuntoId == conjuntoId))
                .OrderBy(a => a.Apellido)
                .ThenBy(a => a.Nombre)
                .ToListAsync(ct);
        }

        public async Task<bool> ExisteCedulaAsync(
            string cedula,
            CancellationToken ct = default)
        {
            return await _dbSet
                .AnyAsync(a => a.Cedula == cedula, ct);
        }

        public async Task<IReadOnlyList<AdminConjunto>> ObtenerTodosConAsignacionesAsync(
            CancellationToken ct = default)
        {
            return await _dbSet
                .Include(a => a.Asignaciones)
                    .ThenInclude(a => a.Conjunto)
                .OrderBy(a => a.Apellido)
                .ThenBy(a => a.Nombre)
                .ToListAsync(ct);
        }

        public async Task<bool> ExisteAsignacionAsync(
            Guid adminId,
            Guid conjuntoId,
            CancellationToken ct = default)
        {
            return await _context.AdminConjuntoAsignaciones
                .AnyAsync(a =>
                    a.AdminConjuntoId == adminId &&
                    a.ConjuntoId == conjuntoId,
                    ct);
        }

        public async Task<Conjunto?> ObtenerConjuntoActivoAsync(
            Guid conjuntoId,
            CancellationToken ct = default)
        {
            return await _context.Conjuntos
                .FirstOrDefaultAsync(
                    c => c.Id == conjuntoId && c.Activo,
                    ct);
        }

        public async Task AgregarAsignacionAsync(
            AdminConjuntoAsignacion asignacion,
            CancellationToken ct = default)
        {
            await _context.AdminConjuntoAsignaciones.AddAsync(
                asignacion, ct);
        }
    }
}
