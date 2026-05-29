using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using VotacionesResidenciales.Application.Common.Exceptions;
using VotacionesResidenciales.Application.Common.Models;
using VotacionesResidenciales.Domain.Entities;
using VotacionesResidenciales.Domain.Interfaces.Repositories;

namespace VotacionesResidenciales.Application.Features.Votaciones.Queries.ObtenerVotacion
{
    public class ObtenerVotacionQueryHandler
    : IRequestHandler<ObtenerVotacionQuery, Result<VotacionDetalleDto>>
    {
        private readonly IVotacionRepository _votacionRepo;

        public ObtenerVotacionQueryHandler(IVotacionRepository votacionRepo)
        {
            _votacionRepo = votacionRepo;
        }

        public async Task<Result<VotacionDetalleDto>> Handle(
            ObtenerVotacionQuery request, CancellationToken ct)
        {
            var votacion = await _votacionRepo
                .ObtenerConDetallesAsync(request.VotacionId, ct);

            if (votacion is null)
                throw new NotFoundException(nameof(Votacion), request.VotacionId);

            return Result<VotacionDetalleDto>.Exito(MapearDto(votacion));
        }

        private static VotacionDetalleDto MapearDto(Votacion votacion) =>
            new(
                votacion.Id,
                votacion.ConjuntoId,
                votacion.Titulo,
                votacion.Descripcion,
                votacion.Estado.ToString(),
                votacion.TipoPeso.ToString(),
                votacion.QuorumRequerido,
                votacion.FechaInicio,
                votacion.FechaFin,
                votacion.MostrarResultadosParciales,
                votacion.ActaUrl,
                votacion.CreadoEn,
                votacion.Preguntas
                    .OrderBy(p => p.Orden)
                    .Select(p => new PreguntaDetalleDto(
                        p.Id,
                        p.Texto,
                        p.Orden,
                        p.Opciones
                            .OrderBy(o => o.Orden)
                            .Select(o => new OpcionDetalleDto(
                                o.Id,
                                o.Texto,
                                o.Orden))
                            .ToList()))
                    .ToList());
    }
}
