using System;
using System.Collections.Generic;
using System.Text;
using VotacionesResidenciales.Domain.Entities;

namespace VotacionesResidenciales.Domain.Interfaces.Repositories
{
    public interface IVotacionRepository : IRepository<Votacion>
    {
        Task<IEnumerable<Votacion>> ObtenerPorConjuntoAsync(Guid conjuntoId, CancellationToken ct = default);
        Task<Votacion?> ObtenerConDetallesAsync(Guid id, CancellationToken ct = default);
    }
}
