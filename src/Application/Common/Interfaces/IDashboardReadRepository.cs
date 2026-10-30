using VotacionesResidenciales.Application.Features.AdminDashboard.DTOs;

namespace VotacionesResidenciales.Application.Common.Interfaces;

public interface IDashboardReadRepository
{
    Task<IReadOnlyList<DashboardConjuntoDto>> ObtenerPorAdministradorAsync(
        Guid adminId,
        CancellationToken ct = default);
}