using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using VotacionesResidenciales.Application.Common.Models;
using VotacionesResidenciales.Domain.Enums;

namespace VotacionesResidenciales.Application.Features.Inmuebles.Queries.ObtenerInmuebles
{
    public record ObtenerInmueblesQuery(
    Guid ConjuntoId,
    TipoInmueble? Tipo = null
) : IRequest<Result<List<InmuebleDto>>>;

    public record InmuebleDto(
        Guid Id,
        Guid ConjuntoId,
        string Numero,
        string Tipo,
        decimal Coeficiente,
        string? Torre,
        string? Piso,
        int TotalResidentes,
        DateTime CreadoEn
    );
}
