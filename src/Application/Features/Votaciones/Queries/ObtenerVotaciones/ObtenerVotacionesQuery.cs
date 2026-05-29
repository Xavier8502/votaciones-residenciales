using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using VotacionesResidenciales.Application.Common.Models;
using VotacionesResidenciales.Domain.Enums;

namespace VotacionesResidenciales.Application.Features.Votaciones.Queries.ObtenerVotaciones
{
    public record ObtenerVotacionesQuery(
    Guid ConjuntoId,
    EstadoVotacion? Estado = null
) : IRequest<Result<List<VotacionResumenDto>>>;

    public record VotacionResumenDto(
        Guid Id,
        string Titulo,
        string? Descripcion,
        string Estado,
        string TipoPeso,
        decimal QuorumRequerido,
        DateTime FechaInicio,
        DateTime FechaFin,
        bool MostrarResultadosParciales,
        int TotalPreguntas,
        DateTime CreadoEn
    );
}
