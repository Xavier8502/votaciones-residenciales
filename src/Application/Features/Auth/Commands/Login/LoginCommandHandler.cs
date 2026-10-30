using MediatR;
using Microsoft.Extensions.Configuration;
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
        private readonly IConfiguration _configuration;

        public LoginCommandHandler(
            IResidenteRepository residenteRepo,
            IAdminConjuntoRepository adminRepo,
            IAuthService authService,
            IConfiguration configuration)
        {
            _residenteRepo = residenteRepo;
            _adminRepo = adminRepo;
            _authService = authService;
            _configuration = configuration;
        }

        public async Task<Result<LoginResponse>> Handle(
            LoginCommand request,
            CancellationToken ct)
        {
            // SuperAdmin: identidad configurada exclusivamente en el servidor.
            var superAdminCedula = _configuration["SuperAdmin:Cedula"];
            var superAdminPinHash = _configuration["SuperAdmin:PinHash"];

            if (!string.IsNullOrWhiteSpace(superAdminCedula) &&
                string.Equals(
                    request.Cedula?.Trim(),
                    superAdminCedula.Trim(),
                    StringComparison.Ordinal))
            {
                if (string.IsNullOrWhiteSpace(superAdminPinHash) ||
                    !_authService.VerificarPin(
                        request.Pin,
                        superAdminPinHash))
                {
                    return Result<LoginResponse>.Fallo(
                        "Cédula o PIN incorrectos.");
                }

                var tokenSuperAdmin = _authService.GenerarToken(
                    Guid.Empty,
                    superAdminCedula,
                    "SuperAdmin",
                    Guid.Empty);

                return Result<LoginResponse>.Exito(
                    new LoginResponse(
                        Token: tokenSuperAdmin,
                        NombreCompleto: "SuperAdministrador",
                        Rol: "SuperAdmin",
                        ConjuntoId: Guid.Empty,
                        ResidenteId: Guid.Empty));
            }

            // Administradores residenciales.
            var admin = await _adminRepo
                .ObtenerPorCedulaAsync(request.Cedula, ct);

            if (admin is not null)
            {
                if (!_authService.VerificarPin(request.Pin, admin.PinHash))
                {
                    return Result<LoginResponse>.Fallo(
                        "Cédula o PIN incorrectos.");
                }

                if (!admin.Activo)
                {
                    return Result<LoginResponse>.Fallo(
                        "Administrador inactivo.");
                }

                var asignacionesActivas = admin.Asignaciones
                    .Where(a =>
                        a.Activo &&
                        a.Conjunto != null &&
                        a.Conjunto.Activo)
                    .ToList();

                var conjuntos = asignacionesActivas
                    .Select(a => new ConjuntoAsignadoResponse(
                        a.ConjuntoId,
                        a.Conjunto!.Nombre))
                    .ToList();

                if (conjuntos.Count == 0)
                {
                    return Result<LoginResponse>.Fallo(
                        "El administrador no tiene conjuntos activos asignados.");
                }

                if (request.ConjuntoId is null && conjuntos.Count > 1)
                {
                    return Result<LoginResponse>.Exito(
                        new LoginResponse(
                            Token: null,
                            NombreCompleto: admin.NombreCompleto,
                            Rol: "AdminConjunto",
                            ConjuntoId: Guid.Empty,
                            ResidenteId: admin.Id,
                            Conjuntos: conjuntos,
                            RequiereSeleccionConjunto: true));
                }

                var conjuntoId = request.ConjuntoId ?? conjuntos[0].Id;

                var asignacionValida = asignacionesActivas.Any(
                    a => a.ConjuntoId == conjuntoId);

                if (!asignacionValida)
                {
                    return Result<LoginResponse>.Fallo(
                        "No tienes acceso al conjunto seleccionado.");
                }

                var conjuntoSeleccionado = conjuntos
                    .First(c => c.Id == conjuntoId);

                var token = _authService.GenerarToken(
                    admin.Id,
                    admin.Cedula,
                    "AdminConjunto",
                    conjuntoSeleccionado.Id);

                return Result<LoginResponse>.Exito(
                    new LoginResponse(
                        Token: token,
                        NombreCompleto: admin.NombreCompleto,
                        Rol: "AdminConjunto",
                        ConjuntoId: conjuntoSeleccionado.Id,
                        ResidenteId: admin.Id,
                        Conjuntos: conjuntos,
                        RequiereSeleccionConjunto: false));
            }

            // Residentes: flujo existente.
            var residente = await _residenteRepo
                .ObtenerPorCedulaAsync(request.Cedula, ct);

            if (residente is null)
            {
                return Result<LoginResponse>.Fallo(
                    "Cédula o PIN incorrectos.");
            }

            if (!_authService.VerificarPin(request.Pin, residente.PinHash))
            {
                return Result<LoginResponse>.Fallo(
                    "Cédula o PIN incorrectos.");
            }

            if (!residente.Activo)
            {
                return Result<LoginResponse>.Fallo(
                    "Residente inactivo.");
            }

            var rolResidente = residente.EsPropietario
                ? "Propietario"
                : "Residente";

            var conjuntoResidenteId = residente.Inmueble!.ConjuntoId;

            var tokenResidente = _authService.GenerarToken(
                residente.Id,
                residente.Cedula,
                rolResidente,
                conjuntoResidenteId);

            return Result<LoginResponse>.Exito(
                new LoginResponse(
                    Token: tokenResidente,
                    NombreCompleto: residente.NombreCompleto,
                    Rol: rolResidente,
                    ConjuntoId: conjuntoResidenteId,
                    ResidenteId: residente.Id));
        }
    }
}
