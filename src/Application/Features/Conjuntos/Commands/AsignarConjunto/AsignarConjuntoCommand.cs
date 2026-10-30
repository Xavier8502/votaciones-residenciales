using MediatR;
using VotacionesResidenciales.Application.Common.Models;

namespace VotacionesResidenciales.Application.Features.AdminConjuntos.Commands.AsignarConjunto;

public record AsignarConjuntoCommand(
    Guid AdminConjuntoId,
    Guid ConjuntoId
) : IRequest<Result<bool>>;