using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using VotacionesResidenciales.Application.Common.Models;
using VotacionesResidenciales.Domain.Enums;

namespace VotacionesResidenciales.Application.Features.Inmuebles.Commands.ActualizarInmueble
{
    public record ActualizarInmuebleCommand(
    Guid Id,
    decimal Coeficiente,
    TipoInmueble Tipo,
    string? Torre,
    string? Piso
) : IRequest<Result>;
}
