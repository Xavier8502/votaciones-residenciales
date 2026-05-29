using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using VotacionesResidenciales.Application.Common.Models;

namespace VotacionesResidenciales.Application.Features.Votaciones.Queries.ObtenerVotacion
{
    public record ObtenerVotacionQuery(Guid VotacionId)
    : IRequest<Result<VotacionDetalleDto>>;

    public record VotacionDetalleDto(
        Guid Id,
        Guid ConjuntoId,
        string Titulo,
        string? Descripcion,
        string Estado,
        string TipoPeso,
        decimal QuorumRequerido,
        DateTime FechaInicio,
        DateTime FechaFin,
        bool MostrarResultadosParciales,
        string? ActaUrl,
        DateTime CreadoEn,
        List<PreguntaDetalleDto> Preguntas
    );

    public record PreguntaDetalleDto(
        Guid Id,
        string Texto,
        int Orden,
        List<OpcionDetalleDto> Opciones
    );

    public record OpcionDetalleDto(
        Guid Id,
        string Texto,
        int Orden
    );
}
