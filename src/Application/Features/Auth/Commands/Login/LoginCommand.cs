using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using VotacionesResidenciales.Application.Common.Models;

namespace VotacionesResidenciales.Application.Features.Auth.Commands.Login
{
    public record LoginCommand(string Cedula, string Pin)
    : IRequest<Result<LoginResponse>>;

    public record LoginResponse(
        string Token,
        string NombreCompleto,
        string Rol,
        Guid ConjuntoId,
        Guid ResidenteId
    );
}
