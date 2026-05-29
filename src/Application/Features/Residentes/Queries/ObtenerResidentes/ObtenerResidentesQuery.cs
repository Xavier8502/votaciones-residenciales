using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using VotacionesResidenciales.Application.Common.Models;

namespace VotacionesResidenciales.Application.Features.Residentes.Queries.ObtenerResidentes
{
    public record ObtenerResidentesQuery(
    Guid ConjuntoId,
    bool? SoloPropietarios = null
) : IRequest<Result<List<ResidenteDto>>>;

    public record ResidenteDto(
        Guid Id,
        Guid InmuebleId,
        string NumeroInmueble,
        string Cedula,
        string NombreCompleto,
        string? Telefono,
        string? Email,
        bool EsPropietario,
        bool Activo,
        DateTime CreadoEn
    );
}
