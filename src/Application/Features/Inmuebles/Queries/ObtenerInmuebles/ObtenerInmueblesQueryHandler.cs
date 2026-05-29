using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using VotacionesResidenciales.Application.Common.Exceptions;
using VotacionesResidenciales.Application.Common.Models;
using VotacionesResidenciales.Domain.Entities;
using VotacionesResidenciales.Domain.Interfaces.Repositories;

namespace VotacionesResidenciales.Application.Features.Inmuebles.Queries.ObtenerInmuebles
{
    public class ObtenerInmueblesQueryHandler
    : IRequestHandler<ObtenerInmueblesQuery, Result<List<InmuebleDto>>>
    {
        private readonly IInmuebleRepository _inmuebleRepo;
        private readonly IConjuntoRepository _conjuntoRepo;

        public ObtenerInmueblesQueryHandler(
            IInmuebleRepository inmuebleRepo,
            IConjuntoRepository conjuntoRepo)
        {
            _inmuebleRepo = inmuebleRepo;
            _conjuntoRepo = conjuntoRepo;
        }

        public async Task<Result<List<InmuebleDto>>> Handle(
            ObtenerInmueblesQuery request, CancellationToken ct)
        {
            var conjunto = await _conjuntoRepo
                .ObtenerPorIdAsync(request.ConjuntoId, ct);

            if (conjunto is null)
                throw new NotFoundException(nameof(Conjunto), request.ConjuntoId);

            var inmuebles = request.Tipo.HasValue
                ? await _inmuebleRepo.ObtenerPorTipoAsync(
                    request.ConjuntoId, request.Tipo.Value, ct)
                : await _inmuebleRepo.ObtenerPorConjuntoAsync(
                    request.ConjuntoId, ct);

            var resultado = inmuebles
                .Where(i => i.Activo)
                .OrderBy(i => i.Torre)
                .ThenBy(i => i.Numero)
                .Select(i => new InmuebleDto(
                    i.Id,
                    i.ConjuntoId,
                    i.Numero,
                    i.Tipo.ToString(),
                    i.Coeficiente,
                    i.Torre,
                    i.Piso,
                    i.Residentes.Count,
                    i.CreadoEn))
                .ToList();

            return Result<List<InmuebleDto>>.Exito(resultado);
        }
    }
}
