using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using VotacionesResidenciales.Application.Common.Interfaces;
using VotacionesResidenciales.Application.Common.Models;
using VotacionesResidenciales.Domain.Interfaces.Repositories;

namespace VotacionesResidenciales.Application.Features.Auth.Commands.Login
{
    public class LoginCommandHandler
     : IRequestHandler<LoginCommand, Result<LoginResponse>>
    {
        private readonly IResidenteRepository _residenteRepo;
        private readonly IAdminConjuntoRepository _adminRepo;
        private readonly IAuthService _authService;

        public LoginCommandHandler(
            IResidenteRepository residenteRepo,
            IAdminConjuntoRepository adminRepo,
            IAuthService authService)
        {
            _residenteRepo = residenteRepo;
            _adminRepo = adminRepo;
            _authService = authService;
        }

        public async Task<Result<LoginResponse>> Handle(
            LoginCommand request, CancellationToken ct)
        {
            // ── Buscar primero en Admins ───────────────────────
            var admin = await _adminRepo
                .ObtenerPorCedulaAsync(request.Cedula, ct);

            if (admin is not null)
            {
                if (!_authService.VerificarPin(
                    request.Pin, admin.PinHash))
                    return Result<LoginResponse>.Fallo(
                        "Cédula o PIN incorrectos.");

                if (!admin.Activo)
                    return Result<LoginResponse>.Fallo(
                        "Administrador inactivo.");

                var token = _authService.GenerarToken(
                    admin.Id,
                    admin.Cedula,
                    "AdminConjunto",
                    admin.ConjuntoId);

                return Result<LoginResponse>.Exito(new LoginResponse(
                    token,
                    admin.NombreCompleto,
                    "AdminConjunto",
                    admin.ConjuntoId,
                    admin.Id));
            }

            // ── Buscar en Residentes ───────────────────────────
            var residente = await _residenteRepo
                .ObtenerPorCedulaAsync(request.Cedula, ct);

            if (residente is null)
                return Result<LoginResponse>.Fallo(
                    "Cédula o PIN incorrectos.");

            if (!_authService.VerificarPin(
                request.Pin, residente.PinHash))
                return Result<LoginResponse>.Fallo(
                    "Cédula o PIN incorrectos.");

            if (!residente.Activo)
                return Result<LoginResponse>.Fallo(
                    "Residente inactivo.");

            var rolResidente = residente.EsPropietario
                ? "Propietario" : "Residente";

            var tokenResidente = _authService.GenerarToken(
                residente.Id,
                residente.Cedula,
                rolResidente,
                residente.Inmueble!.ConjuntoId);

            return Result<LoginResponse>.Exito(new LoginResponse(
                tokenResidente,
                residente.NombreCompleto,
                rolResidente,
                residente.Inmueble.ConjuntoId,
                residente.Id));
        }
    }
}
