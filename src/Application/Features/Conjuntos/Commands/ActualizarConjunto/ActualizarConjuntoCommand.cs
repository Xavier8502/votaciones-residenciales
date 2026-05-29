using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using VotacionesResidenciales.Application.Common.Models;

namespace VotacionesResidenciales.Application.Features.Conjuntos.Commands.ActualizarConjunto
{
    public record ActualizarConjuntoCommand(
    Guid Id,
    string Nombre,
    string Direccion,
    string Ciudad,
    string? Telefono,
    string? Email
) : IRequest<Result>;
}
