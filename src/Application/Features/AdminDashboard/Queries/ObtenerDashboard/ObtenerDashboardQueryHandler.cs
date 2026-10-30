using MediatR;
using VotacionesResidenciales.Application.Common.Interfaces;
using VotacionesResidenciales.Application.Common.Models;
using VotacionesResidenciales.Application.Features.AdminDashboard.DTOs;

namespace VotacionesResidenciales.Application.Features.AdminDashboard.Queries.ObtenerDashboard;

public class ObtenerDashboardQueryHandler
    : IRequestHandler<ObtenerDashboardQuery, Result<List<DashboardConjuntoDto>>>
{
    private readonly IDashboardReadRepository _dashboardRepository;

    public ObtenerDashboardQueryHandler(
        IDashboardReadRepository dashboardRepository)
    {
        _dashboardRepository = dashboardRepository;
    }

    public async Task<Result<List<DashboardConjuntoDto>>> Handle(
        ObtenerDashboardQuery request,
        CancellationToken ct)
    {
        if (request.AdminId == Guid.Empty)
        {
            return Result<List<DashboardConjuntoDto>>.Fallo(
                "No fue posible identificar al administrador autenticado.");
        }

        var conjuntos = await _dashboardRepository
            .ObtenerPorAdministradorAsync(request.AdminId, ct);

        return Result<List<DashboardConjuntoDto>>.Exito(conjuntos.ToList());
    }
}