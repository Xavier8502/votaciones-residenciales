using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using VotacionesResidenciales.Application.Common.Exceptions;
using VotacionesResidenciales.Application.Common.Models;
using VotacionesResidenciales.Domain.Entities;
using VotacionesResidenciales.Domain.Interfaces.Repositories;

namespace VotacionesResidenciales.Application.Features.Conjuntos.Queries.ObtenerConjunto
{
    public class ObtenerConjuntoQueryHandler
    : IRequestHandler<ObtenerConjuntoQuery, Result<ConjuntoDetalleDto>>
    {
        private readonly IConjuntoRepository _conjuntoRepo;
        private readonly IInmuebleRepository _inmuebleRepo;

        public ObtenerConjuntoQueryHandler(
            IConjuntoRepository conjuntoRepo,
            IInmuebleRepository inmuebleRepo)
        {
            _conjuntoRepo = conjuntoRepo;
            _inmuebleRepo = inmuebleRepo;
        }

        public async Task<Result<ConjuntoDetalleDto>> Handle(
            ObtenerConjuntoQuery request, CancellationToken ct)
        {
            var conjunto = await _conjuntoRepo
                .ObtenerPorIdAsync(request.Id, ct);

            if (conjunto is null)
                throw new NotFoundException(nameof(Conjunto), request.Id);

            var inmuebles = await _inmuebleRepo
                .ObtenerPorConjuntoAsync(request.Id, ct);

            var listaInmuebles = inmuebles.ToList();

            return Result<ConjuntoDetalleDto>.Exito(new ConjuntoDetalleDto(
                conjunto.Id,
                conjunto.Nombre,
                conjunto.Nit,
                conjunto.Direccion,
                conjunto.Ciudad,
                conjunto.Telefono,
                conjunto.Email,
                listaInmuebles.Count,
                listaInmuebles.Sum(i => i.Coeficiente),
                conjunto.CreadoEn));
        }
    }
}
