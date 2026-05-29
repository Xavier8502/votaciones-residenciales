using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using VotacionesResidenciales.Application.Common.Models;
using VotacionesResidenciales.Domain.Interfaces.Repositories;

namespace VotacionesResidenciales.Application.Features.Conjuntos.Queries.ObtenerConjuntos
{
    public class ObtenerConjuntosQueryHandler
    : IRequestHandler<ObtenerConjuntosQuery, Result<List<ConjuntoResumenDto>>>
    {
        private readonly IConjuntoRepository _conjuntoRepo;

        public ObtenerConjuntosQueryHandler(IConjuntoRepository conjuntoRepo)
        {
            _conjuntoRepo = conjuntoRepo;
        }

        public async Task<Result<List<ConjuntoResumenDto>>> Handle(
            ObtenerConjuntosQuery request, CancellationToken ct)
        {
            var conjuntos = await _conjuntoRepo.ObtenerTodosAsync(ct);

            var resultado = conjuntos
                .Where(c => c.Activo)
                .OrderBy(c => c.Nombre)
                .Select(c => new ConjuntoResumenDto(
                    c.Id,
                    c.Nombre,
                    c.Nit,
                    c.Direccion,
                    c.Ciudad,
                    c.Telefono,
                    c.Email,
                    c.CreadoEn))
                .ToList();

            return Result<List<ConjuntoResumenDto>>.Exito(resultado);
        }
    }
}
