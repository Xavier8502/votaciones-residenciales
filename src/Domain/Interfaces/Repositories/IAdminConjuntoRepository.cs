
using System;
using VotacionesResidenciales.Domain.Entities;

namespace VotacionesResidenciales.Domain.Interfaces.Repositories
{
    public interface IAdminConjuntoRepository : IRepository<AdminConjunto>
    {
        Task<AdminConjunto?> ObtenerPorCedulaAsync(
            string cedula,
            CancellationToken ct = default);

        Task<AdminConjunto?> ObtenerPorConjuntoAsync(
            Guid conjuntoId,
            CancellationToken ct = default);

        Task<IReadOnlyList<AdminConjunto>> ObtenerTodosPorConjuntoAsync(
            Guid conjuntoId,
            CancellationToken ct = default);

        Task<bool> ExisteCedulaAsync(
            string cedula,
            CancellationToken ct = default);

        Task<IReadOnlyList<AdminConjunto>> ObtenerTodosConAsignacionesAsync(
            CancellationToken ct = default);

        Task<bool> ExisteAsignacionAsync(
            Guid adminId,
            Guid conjuntoId,
            CancellationToken ct = default);

        Task<Conjunto?> ObtenerConjuntoActivoAsync(
            Guid conjuntoId,
            CancellationToken ct = default);

        Task AgregarAsignacionAsync(
            AdminConjuntoAsignacion asignacion,
            CancellationToken ct = default);
    }
}
