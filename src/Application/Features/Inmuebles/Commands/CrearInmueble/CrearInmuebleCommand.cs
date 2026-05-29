using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using VotacionesResidenciales.Application.Common.Models;
using VotacionesResidenciales.Domain.Enums;

namespace VotacionesResidenciales.Application.Features.Inmuebles.Commands.CrearInmueble
{
    public record CrearInmuebleCommand(
    Guid ConjuntoId,
    string Numero,
    TipoInmueble Tipo,
    decimal Coeficiente,
    string? Torre,
    string? Piso
) : IRequest<Result<Guid>>;
}
