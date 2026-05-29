using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using VotacionesResidenciales.Application.Common.Models;

namespace VotacionesResidenciales.Application.Features.Conjuntos.Commands.CrearConjunto
{
    public record CrearConjuntoCommand(
    string Nombre,
    string Nit,
    string Direccion,
    string Ciudad,
    string? Telefono,
    string? Email
) : IRequest<Result<Guid>>;
}
