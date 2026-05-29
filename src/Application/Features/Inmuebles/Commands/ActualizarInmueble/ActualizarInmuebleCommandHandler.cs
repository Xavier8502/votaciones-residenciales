using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using VotacionesResidenciales.Application.Common.Exceptions;
using VotacionesResidenciales.Application.Common.Models;
using VotacionesResidenciales.Domain.Entities;
using VotacionesResidenciales.Domain.Interfaces;
using VotacionesResidenciales.Domain.Interfaces.Repositories;

namespace VotacionesResidenciales.Application.Features.Inmuebles.Commands.ActualizarInmueble
{
    public class ActualizarInmuebleCommandHandler
    : IRequestHandler<ActualizarInmuebleCommand, Result>
    {
        private readonly IInmuebleRepository _inmuebleRepo;
        private readonly IUnitOfWork _uow;

        public ActualizarInmuebleCommandHandler(
            IInmuebleRepository inmuebleRepo,
            IUnitOfWork uow)
        {
            _inmuebleRepo = inmuebleRepo;
            _uow = uow;
        }

        public async Task<Result> Handle(
            ActualizarInmuebleCommand request, CancellationToken ct)
        {
            var inmueble = await _inmuebleRepo
                .ObtenerPorIdAsync(request.Id, ct);

            if (inmueble is null)
                throw new NotFoundException(nameof(Inmueble), request.Id);

            inmueble.Actualizar(
                request.Coeficiente,
                request.Tipo,
                request.Torre,
                request.Piso);

            await _inmuebleRepo.ActualizarAsync(inmueble, ct);
            await _uow.GuardarCambiosAsync(ct);

            return Result.Exito();
        }
    }
}
