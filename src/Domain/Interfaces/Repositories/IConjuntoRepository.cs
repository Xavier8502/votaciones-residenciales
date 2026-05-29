using System;
using System.Collections.Generic;
using System.Text;
using VotacionesResidenciales.Domain.Entities;

namespace VotacionesResidenciales.Domain.Interfaces.Repositories
{
    public interface IConjuntoRepository : IRepository<Conjunto>
    {
        Task<Conjunto?> ObtenerPorNitAsync(string nit, CancellationToken ct = default);
        Task<Conjunto?> ObtenerConInmueblesAsync(Guid id, CancellationToken ct = default);
        Task<bool> ExisteNitAsync(string nit, CancellationToken ct = default);
    }
}
