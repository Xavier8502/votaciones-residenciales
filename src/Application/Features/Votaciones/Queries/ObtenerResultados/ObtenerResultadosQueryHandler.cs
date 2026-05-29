using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using VotacionesResidenciales.Application.Common.Exceptions;
using VotacionesResidenciales.Application.Common.Models;
using VotacionesResidenciales.Domain.Entities;
using VotacionesResidenciales.Domain.Enums;
using VotacionesResidenciales.Domain.Interfaces.Repositories;

namespace VotacionesResidenciales.Application.Features.Votaciones.Queries.ObtenerResultados
{
    public class ObtenerResultadosQueryHandler
    : IRequestHandler<ObtenerResultadosQuery, Result<ResultadosVotacionDto>>
    {
        private readonly IVotacionRepository _votacionRepo;
        private readonly IVotoRepository _votoRepo;
        private readonly IInmuebleRepository _inmuebleRepo;

        public ObtenerResultadosQueryHandler(
            IVotacionRepository votacionRepo,
            IVotoRepository votoRepo,
            IInmuebleRepository inmuebleRepo)
        {
            _votacionRepo = votacionRepo;
            _votoRepo = votoRepo;
            _inmuebleRepo = inmuebleRepo;
        }

        public async Task<Result<ResultadosVotacionDto>> Handle(
            ObtenerResultadosQuery request, CancellationToken ct)
        {
            var votacion = await _votacionRepo
                .ObtenerConDetallesAsync(request.VotacionId, ct);

            if (votacion is null)
                throw new NotFoundException(nameof(Votacion), request.VotacionId);

            // Si la votación no muestra parciales y está abierta
            if (!votacion.MostrarResultadosParciales
                && votacion.Estado == EstadoVotacion.Abierta)
                return Result<ResultadosVotacionDto>.Fallo(
                    "Los resultados no están disponibles hasta cerrar la votación.");

            var votos = await _votoRepo
                .ObtenerPorVotacionAsync(request.VotacionId, ct);

            var totalVotos = votos.Count();
            var totalCoeficientes = await _inmuebleRepo
                .SumarCoeficientesAsync(votacion.ConjuntoId, ct);

            var pesoTotal = votos.Sum(v => v.PesoAplicado);

            var porcentaje = votacion.TipoPeso == TipoPesoVoto.PorCoeficiente
                ? totalCoeficientes > 0
                    ? (pesoTotal / totalCoeficientes) * 100
                    : 0
                : 0;

            var quorumAlcanzado = porcentaje >= votacion.QuorumRequerido;

            var preguntas = votacion.Preguntas.Select(p =>
            {
                var detallesPregunta = votos
                    .SelectMany(v => v.Detalles)
                    .Where(d => d.PreguntaId == p.Id)
                    .ToList();

                var opciones = p.Opciones.Select(o =>
                {
                    var votosPorOpcion = detallesPregunta
                        .Where(d => d.OpcionId == o.Id).Count();

                    var pesoPorOpcion = votos
                        .Where(v => v.Detalles
                            .Any(d => d.OpcionId == o.Id
                                && d.PreguntaId == p.Id))
                        .Sum(v => v.PesoAplicado);

                    var pct = detallesPregunta.Count > 0
                        ? (decimal)votosPorOpcion / detallesPregunta.Count * 100
                        : 0;

                    return new ResultadoOpcionDto(
                        o.Id, o.Texto,
                        votosPorOpcion,
                        Math.Round(pct, 2),
                        Math.Round(pesoPorOpcion, 4));
                }).ToList();

                return new ResultadoPreguntaDto(p.Id, p.Texto, opciones);
            }).ToList();

            return Result<ResultadosVotacionDto>.Exito(new ResultadosVotacionDto(
                votacion.Id,
                votacion.Titulo,
                votacion.Estado.ToString(),
                totalVotos,
                Math.Round(porcentaje, 2),
                quorumAlcanzado,
                preguntas));
        }
    }
}
