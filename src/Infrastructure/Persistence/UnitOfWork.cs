
using Microsoft.EntityFrameworkCore;
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

        public async Task<int> GuardarCambiosAsync(
            CancellationToken ct = default)
        {
            try
            {
                return await _context.SaveChangesAsync(ct);
            }
            catch (DbUpdateConcurrencyException ex)
            {
                Console.Error.WriteLine(
                    "========== ERROR DE CONCURRENCIA EF CORE ==========");

                foreach (var entry in ex.Entries)
                {
                    Console.Error.WriteLine(
                        $"Entidad: {entry.Metadata.ClrType.Name}");
                    Console.Error.WriteLine(
                        $"Tabla: {entry.Metadata.GetTableName()}");
                    Console.Error.WriteLine(
                        $"Estado: {entry.State}");

                    foreach (var property in entry.Properties)
                    {
                        Console.Error.WriteLine(
                            $"  {property.Metadata.Name}: " +
                            $"Original={property.OriginalValue}, " +
                            $"Actual={property.CurrentValue}");
                    }
                }

                Console.Error.WriteLine(
                    "===================================================");

                throw;
            }
        }

        public void Dispose() => _context.Dispose();
    }
}
