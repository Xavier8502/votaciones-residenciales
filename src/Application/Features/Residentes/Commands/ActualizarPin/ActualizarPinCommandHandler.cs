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

namespace VotacionesResidenciales.Application.Features.Residentes.Commands.ActualizarPin
{
    public class ActualizarPinCommandHandler
    : IRequestHandler<ActualizarPinCommand, Result>
    {
        private readonly IResidenteRepository _residenteRepo;
        private readonly IAuthService _authService;
        private readonly IUnitOfWork _uow;

        public ActualizarPinCommandHandler(
            IResidenteRepository residenteRepo,
            IAuthService authService,
            IUnitOfWork uow)
        {
            _residenteRepo = residenteRepo;
            _authService = authService;
            _uow = uow;
        }

        public async Task<Result> Handle(
            ActualizarPinCommand request, CancellationToken ct)
        {
            var residente = await _residenteRepo
                .ObtenerPorIdAsync(request.ResidenteId, ct);

            if (residente is null)
                throw new NotFoundException(nameof(Residente), request.ResidenteId);

            if (!_authService.VerificarPin(request.PinActual, residente.PinHash))
                return Result.Fallo("El PIN actual es incorrecto.");

            var nuevoPinHash = _authService.HashPin(request.NuevoPin);

            residente.ActualizarPin(nuevoPinHash);

            await _residenteRepo.ActualizarAsync(residente, ct);
            await _uow.GuardarCambiosAsync(ct);

            return Result.Exito();
        }
    }
}
