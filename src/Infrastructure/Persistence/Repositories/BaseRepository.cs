using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using VotacionesResidenciales.Domain.Interfaces.Repositories;

namespace VotacionesResidenciales.Infrastructure.Persistence.Repositories
{
    public abstract class BaseRepository<T> : IRepository<T>
    where T : class
    {
        protected readonly AppDbContext _context;
        protected readonly DbSet<T> _dbSet;

        protected BaseRepository(AppDbContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }

        public async Task<T?> ObtenerPorIdAsync(Guid id,
            CancellationToken ct = default)
            => await _dbSet.FindAsync([id], ct);

        public async Task<IEnumerable<T>> ObtenerTodosAsync(
            CancellationToken ct = default)
            => await _dbSet.ToListAsync(ct);

        public async Task AgregarAsync(T entidad,
            CancellationToken ct = default)
            => await _dbSet.AddAsync(entidad, ct);

        public Task ActualizarAsync(T entidad,
            CancellationToken ct = default)
        {
            _dbSet.Update(entidad);
            return Task.CompletedTask;
        }

        public async Task EliminarAsync(Guid id,
            CancellationToken ct = default)
        {
            var entidad = await ObtenerPorIdAsync(id, ct);
            if (entidad is not null)
                _dbSet.Remove(entidad);
        }
    }
}
