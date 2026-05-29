using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using VotacionesResidenciales.Application.Common.Models;

namespace VotacionesResidenciales.Application.Features.Conjuntos.Queries.ObtenerConjuntos
{
    public record ObtenerConjuntosQuery()
    : IRequest<Result<List<ConjuntoResumenDto>>>;

    public record ConjuntoResumenDto(
        Guid Id,
        string Nombre,
        string Nit,
        string Direccion,
        string Ciudad,
        string? Telefono,
        string? Email,
        DateTime CreadoEn
    );
}
