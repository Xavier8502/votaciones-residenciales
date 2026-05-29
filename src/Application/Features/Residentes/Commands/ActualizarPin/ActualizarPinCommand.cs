using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using VotacionesResidenciales.Application.Common.Models;

namespace VotacionesResidenciales.Application.Features.Residentes.Commands.ActualizarPin
{
    public record ActualizarPinCommand(
    Guid ResidenteId,
    string PinActual,
    string NuevoPin
) : IRequest<Result>;
}
