using System;
using System.Collections.Generic;
using System.Text;
using VotacionesResidenciales.Domain.Interfaces;

namespace VotacionesResidenciales.Infrastructure.Persistence
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;

        public UnitOfWork(AppDbContext context)
        {
            _context = context;
        }

        public async Task<int> GuardarCambiosAsync(CancellationToken ct = default)
            => await _context.SaveChangesAsync(ct);

        public void Dispose() => _context.Dispose();
    }
}
