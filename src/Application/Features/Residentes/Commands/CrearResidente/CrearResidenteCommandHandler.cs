using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using VotacionesResidenciales.Application.Common.Exceptions;
using VotacionesResidenciales.Application.Common.Interfaces;
using VotacionesResidenciales.Application.Common.Models;
using VotacionesResidenciales.Domain.Entities;
using VotacionesResidenciales.Domain.Interfaces;
using VotacionesResidenciales.Domain.Interfaces.Repositories;

namespace VotacionesResidenciales.Application.Features.Residentes.Commands.CrearResidente
{
    public class CrearResidenteCommandHandler
    : IRequestHandler<CrearResidenteCommand, Result<Guid>>
    {
        private readonly IResidenteRepository _residenteRepo;
        private readonly IInmuebleRepository _inmuebleRepo;
        private readonly IAuthService _authService;
        private readonly IUnitOfWork _uow;

        public CrearResidenteCommandHandler(
            IResidenteRepository residenteRepo,
            IInmuebleRepository inmuebleRepo,
            IAuthService authService,
            IUnitOfWork uow)
        {
            _residenteRepo = residenteRepo;
            _inmuebleRepo = inmuebleRepo;
            _authService = authService;
            _uow = uow;
        }

        public async Task<Result<Guid>> Handle(
            CrearResidenteCommand request, CancellationToken ct)
        {
            var inmueble = await _inmuebleRepo
                .ObtenerPorIdAsync(request.InmuebleId, ct);

            if (inmueble is null)
                throw new NotFoundException(nameof(Inmueble), request.InmuebleId);

            var existe = await _residenteRepo
                .ObtenerPorCedulaAsync(request.Cedula, ct);

            if (existe is not null)
                return Result<Guid>.Fallo(
                    $"Ya existe un residente con la cédula {request.Cedula}.");

            var pinHash = _authService.HashPin(request.Pin);

            var residente = Residente.Crear(
                request.InmuebleId,
                request.Cedula,
                request.Nombre,
                request.Apellido,
                pinHash,
                request.EsPropietario);

            residente.ActualizarContacto(request.Telefono, request.Email);

            await _residenteRepo.AgregarAsync(residente, ct);
            await _uow.GuardarCambiosAsync(ct);

            return Result<Guid>.Exito(residente.Id);
        }
    }
}
