using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using VotacionesResidenciales.Application.Common.Exceptions;
using VotacionesResidenciales.Application.Common.Models;
using VotacionesResidenciales.Domain.Entities;
using VotacionesResidenciales.Domain.Interfaces;
using VotacionesResidenciales.Domain.Interfaces.Repositories;

namespace VotacionesResidenciales.Application.Features.Inmuebles.Commands.CrearInmueble
{
    public class CrearInmuebleCommandHandler
    : IRequestHandler<CrearInmuebleCommand, Result<Guid>>
    {
        private readonly IInmuebleRepository _inmuebleRepo;
        private readonly IConjuntoRepository _conjuntoRepo;
        private readonly IUnitOfWork _uow;

        public CrearInmuebleCommandHandler(
            IInmuebleRepository inmuebleRepo,
            IConjuntoRepository conjuntoRepo,
            IUnitOfWork uow)
        {
            _inmuebleRepo = inmuebleRepo;
            _conjuntoRepo = conjuntoRepo;
            _uow = uow;
        }

        public async Task<Result<Guid>> Handle(
            CrearInmuebleCommand request, CancellationToken ct)
        {
            var conjunto = await _conjuntoRepo
                .ObtenerPorIdAsync(request.ConjuntoId, ct);

            if (conjunto is null)
                throw new NotFoundException(nameof(Conjunto), request.ConjuntoId);

            var existe = await _inmuebleRepo
                .ExisteNumeroEnConjuntoAsync(request.ConjuntoId, request.Numero, ct);

            if (existe)
                return Result<Guid>.Fallo(
                    $"Ya existe el inmueble {request.Numero} en este conjunto.");

            var inmueble = Inmueble.Crear(
                request.ConjuntoId,
                request.Numero,
                request.Tipo,
                request.Coeficiente,
                request.Torre,
                request.Piso);

            await _inmuebleRepo.AgregarAsync(inmueble, ct);
            await _uow.GuardarCambiosAsync(ct);

            return Result<Guid>.Exito(inmueble.Id);
        }
    }
}
