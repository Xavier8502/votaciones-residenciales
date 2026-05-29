using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using VotacionesResidenciales.Application.Common.Exceptions;
using VotacionesResidenciales.Application.Common.Models;
using VotacionesResidenciales.Domain.Entities;
using VotacionesResidenciales.Domain.Interfaces.Repositories;

namespace VotacionesResidenciales.Application.Features.Residentes.Queries.ObtenerResidentes
{
    public class ObtenerResidentesQueryHandler
    : IRequestHandler<ObtenerResidentesQuery, Result<List<ResidenteDto>>>
    {
        private readonly IResidenteRepository _residenteRepo;
        private readonly IConjuntoRepository _conjuntoRepo;

        public ObtenerResidentesQueryHandler(
            IResidenteRepository residenteRepo,
            IConjuntoRepository conjuntoRepo)
        {
            _residenteRepo = residenteRepo;
            _conjuntoRepo = conjuntoRepo;
        }

        public async Task<Result<List<ResidenteDto>>> Handle(
            ObtenerResidentesQuery request, CancellationToken ct)
        {
            var conjunto = await _conjuntoRepo
                .ObtenerPorIdAsync(request.ConjuntoId, ct);

            if (conjunto is null)
                throw new NotFoundException(nameof(Conjunto), request.ConjuntoId);

            var residentes = await _residenteRepo
                .ObtenerPorConjuntoAsync(request.ConjuntoId, ct);

            if (request.SoloPropietarios.HasValue)
                residentes = residentes
                    .Where(r => r.EsPropietario == request.SoloPropietarios.Value);

            var resultado = residentes
                .OrderBy(r => r.Apellido)
                .ThenBy(r => r.Nombre)
                .Select(r => new ResidenteDto(
                    r.Id,
                    r.InmuebleId,
                    r.Inmueble?.Numero ?? string.Empty,
                    r.Cedula,
                    r.NombreCompleto,
                    r.Telefono,
                    r.Email,
                    r.EsPropietario,
                    r.Activo,
                    r.CreadoEn))
                .ToList();

            return Result<List<ResidenteDto>>.Exito(resultado);
        }
    }
}
