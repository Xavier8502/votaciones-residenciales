using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using VotacionesResidenciales.Application.Common.Exceptions;
using VotacionesResidenciales.Application.Common.Models;
using VotacionesResidenciales.Domain.Entities;
using VotacionesResidenciales.Domain.Interfaces.Repositories;

namespace VotacionesResidenciales.Application.Features.Votaciones.Queries.ObtenerVotaciones
{
    public class ObtenerVotacionesQueryHandler
    : IRequestHandler<ObtenerVotacionesQuery, Result<List<VotacionResumenDto>>>
    {
        private readonly IVotacionRepository _votacionRepo;
        private readonly IConjuntoRepository _conjuntoRepo;

        public ObtenerVotacionesQueryHandler(
            IVotacionRepository votacionRepo,
            IConjuntoRepository conjuntoRepo)
        {
            _votacionRepo = votacionRepo;
            _conjuntoRepo = conjuntoRepo;
        }

        public async Task<Result<List<VotacionResumenDto>>> Handle(
            ObtenerVotacionesQuery request, CancellationToken ct)
        {
            var conjunto = await _conjuntoRepo
                .ObtenerPorIdAsync(request.ConjuntoId, ct);

            if (conjunto is null)
                throw new NotFoundException(nameof(Conjunto), request.ConjuntoId);

            var votaciones = await _votacionRepo
                .ObtenerPorConjuntoAsync(request.ConjuntoId, ct);

            // Filtrar por estado si se especificó
            if (request.Estado.HasValue)
                votaciones = votaciones
                    .Where(v => v.Estado == request.Estado.Value);

            var resultado = votaciones
                .OrderByDescending(v => v.CreadoEn)
                .Select(MapearDto)
                .ToList();

            return Result<List<VotacionResumenDto>>.Exito(resultado);
        }

        private static VotacionResumenDto MapearDto(Votacion votacion) =>
            new(
                votacion.Id,
                votacion.Titulo,
                votacion.Descripcion,
                votacion.Estado.ToString(),
                votacion.TipoPeso.ToString(),
                votacion.QuorumRequerido,
                votacion.FechaInicio,
                votacion.FechaFin,
                votacion.MostrarResultadosParciales,
                votacion.Preguntas.Count,
                votacion.CreadoEn);
    }
}
