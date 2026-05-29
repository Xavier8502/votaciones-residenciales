using System;
using System.Collections.Generic;
using System.Text;

namespace VotacionesResidenciales.Domain.Interfaces.Repositories
{
    public interface IRepository<T> where T : class
    {
        Task<T?> ObtenerPorIdAsync(Guid id, CancellationToken ct = default);
        Task<IEnumerable<T>> ObtenerTodosAsync(CancellationToken ct = default);
        Task AgregarAsync(T entidad, CancellationToken ct = default);
        Task ActualizarAsync(T entidad, CancellationToken ct = default);
        Task EliminarAsync(Guid id, CancellationToken ct = default);
    }
}
