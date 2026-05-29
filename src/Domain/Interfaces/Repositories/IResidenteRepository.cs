using System;
using System.Collections.Generic;
using System.Text;
using VotacionesResidenciales.Domain.Entities;

namespace VotacionesResidenciales.Domain.Interfaces.Repositories
{
    public interface IResidenteRepository : IRepository<Residente>
    {
        Task<Residente?> ObtenerPorCedulaAsync(string cedula, CancellationToken ct = default);
        Task<IEnumerable<Residente>> ObtenerPorConjuntoAsync(Guid conjuntoId, CancellationToken ct = default);
    }
}
