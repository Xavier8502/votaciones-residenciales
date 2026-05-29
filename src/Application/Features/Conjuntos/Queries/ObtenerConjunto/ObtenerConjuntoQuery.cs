using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using VotacionesResidenciales.Application.Common.Models;

namespace VotacionesResidenciales.Application.Features.Conjuntos.Queries.ObtenerConjunto
{
    public record ObtenerConjuntoQuery(Guid Id)
    : IRequest<Result<ConjuntoDetalleDto>>;

    public record ConjuntoDetalleDto(
        Guid Id,
        string Nombre,
        string Nit,
        string Direccion,
        string Ciudad,
        string? Telefono,
        string? Email,
        int TotalInmuebles,
        decimal TotalCoeficientes,
        DateTime CreadoEn
    );
}
