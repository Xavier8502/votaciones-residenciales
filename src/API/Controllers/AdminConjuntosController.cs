using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VotacionesResidenciales.Application.Features.AdminConjuntos.Commands.AsignarConjunto;
using VotacionesResidenciales.Domain.Interfaces.Repositories;

namespace VotacionesResidenciales.API.Controllers
{
    [ApiController]
    [Route("api/adminconjuntos")]
    [Authorize(Roles = "SuperAdmin")]
    public class AdminConjuntosController : ControllerBase
    {
        private readonly IAdminConjuntoRepository _adminRepo;
        private readonly IMediator _mediator;

        public AdminConjuntosController(
            IAdminConjuntoRepository adminRepo,
            IMediator mediator)
        {
            _adminRepo = adminRepo;
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> ObtenerTodos(
            CancellationToken ct)
        {
            var administradores =
                await _adminRepo.ObtenerTodosConAsignacionesAsync(ct);

            var respuesta = administradores.Select(admin => new
            {
                admin.Id,
                admin.Cedula,
                admin.Nombre,
                admin.Apellido,
                admin.Activo,
                Conjuntos = admin.Asignaciones.Select(asignacion => new
                {
                    asignacion.ConjuntoId,
                    Nombre = asignacion.Conjunto?.Nombre,
                    asignacion.Activo,
                    ConjuntoActivo = asignacion.Conjunto?.Activo ?? false
                }).ToList()
            });

            return Ok(respuesta);
        }

        [HttpPost("asignaciones")]
        public async Task<IActionResult> AsignarConjunto(
            [FromBody] AsignarConjuntoCommand command,
            CancellationToken ct)
        {
            var resultado = await _mediator.Send(command, ct);

            if (!resultado.Exitoso)
                return BadRequest(new { resultado.Error });

            return Ok(new
            {
                Mensaje = "Conjunto asignado correctamente."
            });
        }
    }
}
