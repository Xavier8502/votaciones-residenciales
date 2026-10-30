
using MediatR;
using System;
using System.Collections.Generic;
using VotacionesResidenciales.Application.Common.Models;

namespace VotacionesResidenciales.Application.Features.Auth.Commands.Login
{
    public record LoginCommand(
        string Cedula,
        string Pin,
        Guid? ConjuntoId = null
    ) : IRequest<Result<LoginResponse>>;

    public record ConjuntoAsignadoResponse(
        Guid Id,
        string Nombre
    );

    public record LoginResponse(
        string? Token,
        string NombreCompleto,
        string Rol,
        Guid ConjuntoId,
        Guid ResidenteId,
        IReadOnlyList<ConjuntoAsignadoResponse>? Conjuntos = null,
        bool RequiereSeleccionConjunto = false
    );
}
