using MediatR;
using VotacionesResidenciales.Application.Common.Models;
using VotacionesResidenciales.Application.Features.AdminDashboard.DTOs;

namespace VotacionesResidenciales.Application.Features.AdminDashboard.Queries.ObtenerDashboard;

public record ObtenerDashboardQuery(Guid AdminId)
    : IRequest<Result<List<DashboardConjuntoDto>>>;