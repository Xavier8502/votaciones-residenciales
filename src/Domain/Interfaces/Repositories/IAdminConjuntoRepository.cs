using System;
using System.Collections.Generic;
using System.Text;
using VotacionesResidenciales.Domain.Entities;

namespace VotacionesResidenciales.Domain.Interfaces.Repositories
{
    public interface IAdminConjuntoRepository : IRepository<AdminConjunto>
    {
        Task<AdminConjunto?> ObtenerPorCedulaAsync(
            string cedula, CancellationToken ct = default);

        Task<AdminConjunto?> ObtenerPorConjuntoAsync(
            Guid conjuntoId, CancellationToken ct = default);

        Task<bool> ExisteCedulaAsync(
            string cedula, CancellationToken ct = default);
    }
}
