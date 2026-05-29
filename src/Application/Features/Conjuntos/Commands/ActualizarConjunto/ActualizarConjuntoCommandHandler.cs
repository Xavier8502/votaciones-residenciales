using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using VotacionesResidenciales.Application.Common.Exceptions;
using VotacionesResidenciales.Application.Common.Models;
using VotacionesResidenciales.Domain.Entities;
using VotacionesResidenciales.Domain.Interfaces;
using VotacionesResidenciales.Domain.Interfaces.Repositories;

namespace VotacionesResidenciales.Application.Features.Conjuntos.Commands.ActualizarConjunto
{
    public class ActualizarConjuntoCommandHandler
    : IRequestHandler<ActualizarConjuntoCommand, Result>
    {
        private readonly IConjuntoRepository _conjuntoRepo;
        private readonly IUnitOfWork _uow;

        public ActualizarConjuntoCommandHandler(
            IConjuntoRepository conjuntoRepo,
            IUnitOfWork uow)
        {
            _conjuntoRepo = conjuntoRepo;
            _uow = uow;
        }

        public async Task<Result> Handle(
            ActualizarConjuntoCommand request, CancellationToken ct)
        {
            var conjunto = await _conjuntoRepo
                .ObtenerPorIdAsync(request.Id, ct);

            if (conjunto is null)
                throw new NotFoundException(nameof(Conjunto), request.Id);

            conjunto.Actualizar(
                request.Nombre,
                request.Direccion,
                request.Ciudad);

            conjunto.ActualizarContacto(request.Telefono, request.Email);

            await _conjuntoRepo.ActualizarAsync(conjunto, ct);
            await _uow.GuardarCambiosAsync(ct);

            return Result.Exito();
        }
    }
}
