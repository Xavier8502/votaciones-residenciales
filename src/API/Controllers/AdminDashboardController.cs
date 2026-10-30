using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VotacionesResidenciales.Application.Features.AdminDashboard.Queries.ObtenerDashboard;

namespace VotacionesResidenciales.API.Controllers;

[ApiController]
[Route("api/admin/dashboard")]
[Authorize(Roles = "AdminConjunto")]
public class AdminDashboardController : ControllerBase
{
    private readonly IMediator _mediator;

    public AdminDashboardController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Obtener(CancellationToken ct)
    {
        var adminIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (!Guid.TryParse(adminIdClaim, out var adminId) ||
            adminId == Guid.Empty)
        {
            return Unauthorized();
        }

        var result = await _mediator.Send(
            new ObtenerDashboardQuery(adminId), ct);

        if (!result.Exitoso)
            return BadRequest(new { result.Error });

        return Ok(result.Datos);
    }
}