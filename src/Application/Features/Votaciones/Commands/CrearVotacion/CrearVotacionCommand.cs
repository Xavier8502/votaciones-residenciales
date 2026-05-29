using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using VotacionesResidenciales.Application.Common.Models;
using VotacionesResidenciales.Domain.Enums;

namespace VotacionesResidenciales.Application.Features.Votaciones.Commands.CrearVotacion
{
    public record CrearVotacionCommand(
    Guid ConjuntoId,
    string Titulo,
    string? Descripcion,
    TipoPesoVoto TipoPeso,
    decimal QuorumRequerido,
    DateTime FechaInicio,
    DateTime FechaFin,
    bool MostrarResultadosParciales,
    List<CrearPreguntaDto> Preguntas
) : IRequest<Result<Guid>>;

    public record CrearPreguntaDto(
        string Texto,
        int Orden,
        List<CrearOpcionDto> Opciones
    );

    public record CrearOpcionDto(
        string Texto,
        int Orden
    );
}
