using System;
using System.Collections.Generic;
using System.Text;
using VotacionesResidenciales.Domain.Entities;
using VotacionesResidenciales.Domain.Enums;

namespace VotacionesResidenciales.Domain.Interfaces.Repositories
{
    public interface IInmuebleRepository : IRepository<Inmueble>
    {
        Task<IEnumerable<Inmueble>> ObtenerPorConjuntoAsync(Guid conjuntoId, CancellationToken ct = default);
        Task<Inmueble?> ObtenerConResidentesAsync(Guid id, CancellationToken ct = default);
        Task<IEnumerable<Inmueble>> ObtenerPorTipoAsync(Guid conjuntoId, TipoInmueble tipo, CancellationToken ct = default);
        Task<bool> ExisteNumeroEnConjuntoAsync(Guid conjuntoId, string numero, CancellationToken ct = default);
        Task<decimal> SumarCoeficientesAsync(Guid conjuntoId, CancellationToken ct = default);
    }
}
