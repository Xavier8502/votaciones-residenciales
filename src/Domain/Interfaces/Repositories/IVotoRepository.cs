using System;
using System.Collections.Generic;
using System.Text;
using VotacionesResidenciales.Domain.Entities;

namespace VotacionesResidenciales.Domain.Interfaces.Repositories
{
    public interface IVotoRepository : IRepository<Voto>
    {
        Task<bool> YaVotoAsync(Guid residenteId, Guid votacionId, CancellationToken ct = default);
        Task<IEnumerable<Voto>> ObtenerPorVotacionAsync(Guid votacionId, CancellationToken ct = default);
        Task<int> ContarVotosPorVotacionAsync(Guid votacionId, CancellationToken ct = default);
    }
}
