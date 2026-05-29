using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using VotacionesResidenciales.Application.Common.Models;
using VotacionesResidenciales.Domain.Entities;
using VotacionesResidenciales.Domain.Interfaces;
using VotacionesResidenciales.Domain.Interfaces.Repositories;

namespace VotacionesResidenciales.Application.Features.Conjuntos.Commands.CrearConjunto
{
    public class CrearConjuntoCommandHandler
    : IRequestHandler<CrearConjuntoCommand, Result<Guid>>
    {
        private readonly IConjuntoRepository _conjuntoRepo;
        private readonly IUnitOfWork _uow;

        public CrearConjuntoCommandHandler(
            IConjuntoRepository conjuntoRepo,
            IUnitOfWork uow)
        {
            _conjuntoRepo = conjuntoRepo;
            _uow = uow;
        }

        public async Task<Result<Guid>> Handle(
            CrearConjuntoCommand request, CancellationToken ct)
        {
            var existe = await _conjuntoRepo
                .ExisteNitAsync(request.Nit, ct);

            if (existe)
                return Result<Guid>.Fallo(
                    $"Ya existe un conjunto con el NIT {request.Nit}.");

            var conjunto = Conjunto.Crear(
                request.Nombre,
                request.Nit,
                request.Direccion,
                request.Ciudad);

            conjunto.ActualizarContacto(request.Telefono, request.Email);

            await _conjuntoRepo.AgregarAsync(conjunto, ct);
            await _uow.GuardarCambiosAsync(ct);

            return Result<Guid>.Exito(conjunto.Id);
        }
    }
}
