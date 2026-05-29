using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using VotacionesResidenciales.Application.Common.Models;

namespace VotacionesResidenciales.Application.Features.Votaciones.Commands.EmitirVoto
{
    public record EmitirVotoCommand(
    Guid VotacionId,
    Guid ResidenteId,
    List<RespuestaDto> Respuestas
) : IRequest<Result<Guid>>;

    public record RespuestaDto(
        Guid PreguntaId,
        Guid OpcionId
    );
}
