using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using VotacionesResidenciales.Application.Common.Models;

namespace VotacionesResidenciales.Application.Features.Votaciones.Queries.ObtenerResultados
{
    public record ObtenerResultadosQuery(Guid VotacionId)
    : IRequest<Result<ResultadosVotacionDto>>;

    public record ResultadosVotacionDto(
        Guid VotacionId,
        string Titulo,
        string Estado,
        int TotalVotos,
        decimal PorcentajeParticipacion,
        bool QuorumAlcanzado,
        List<ResultadoPreguntaDto> Preguntas
    );

    public record ResultadoPreguntaDto(
        Guid PreguntaId,
        string Texto,
        List<ResultadoOpcionDto> Opciones
    );

    public record ResultadoOpcionDto(
        Guid OpcionId,
        string Texto,
        int TotalVotos,
        decimal PorcentajeVotos,
        decimal PesoTotal
    );
}
