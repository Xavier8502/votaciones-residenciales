using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using VotacionesResidenciales.Application.Common.Models;

namespace VotacionesResidenciales.Application.Features.Residentes.Commands.CrearResidente
{
    public record CrearResidenteCommand(
    Guid InmuebleId,
    string Cedula,
    string Nombre,
    string Apellido,
    string Pin,
    bool EsPropietario,
    string? Telefono,
    string? Email
) : IRequest<Result<Guid>>;
}
