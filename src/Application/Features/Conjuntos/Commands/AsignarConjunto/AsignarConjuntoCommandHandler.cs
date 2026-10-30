using MediatR;
using VotacionesResidenciales.Application.Common.Models;
using VotacionesResidenciales.Domain.Entities;
using VotacionesResidenciales.Domain.Interfaces;
using VotacionesResidenciales.Domain.Interfaces.Repositories;

namespace VotacionesResidenciales.Application.Features.AdminConjuntos.Commands.AsignarConjunto;

public class AsignarConjuntoCommandHandler
    : IRequestHandler<AsignarConjuntoCommand, Result<bool>>
{
    private readonly IAdminConjuntoRepository _adminRepo;
    private readonly IUnitOfWork _unitOfWork;

    public AsignarConjuntoCommandHandler(
        IAdminConjuntoRepository adminRepo,
        IUnitOfWork unitOfWork)
    {
        _adminRepo = adminRepo;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<bool>> Handle(
        AsignarConjuntoCommand request,
        CancellationToken ct)
    {
        var admin = await _adminRepo.ObtenerPorIdAsync(
            request.AdminConjuntoId, ct);

        if (admin is null)
            return Result<bool>.Fallo("El administrador no existe.");

        var conjunto = await _adminRepo.ObtenerConjuntoActivoAsync(
            request.ConjuntoId, ct);

        if (conjunto is null)
            return Result<bool>.Fallo(
                "El conjunto no existe o está inactivo.");

        var existe = await _adminRepo.ExisteAsignacionAsync(
            request.AdminConjuntoId,
            request.ConjuntoId,
            ct);

        if (existe)
            return Result<bool>.Fallo(
                "El administrador ya tiene una asignación para ese conjunto.");

        var asignacion = AdminConjuntoAsignacion.Crear(
            admin.Id,
            conjunto.Id);

        await _adminRepo.AgregarAsignacionAsync(asignacion, ct);
        await _unitOfWork.GuardarCambiosAsync(ct);

        return Result<bool>.Exito(true);
    }
}
