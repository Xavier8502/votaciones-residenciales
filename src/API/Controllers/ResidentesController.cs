using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VotacionesResidenciales.API.Extensions;
using VotacionesResidenciales.Application.Features.Residentes.Commands.ActualizarPin;
using VotacionesResidenciales.Application.Features.Residentes.Commands.CrearResidente;
using VotacionesResidenciales.Application.Features.Residentes.Queries.ObtenerResidentes;

namespace VotacionesResidenciales.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ResidentesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ResidentesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>Obtener residentes por conjunto</summary>
        [HttpGet("conjunto/{conjuntoId:guid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> ObtenerPorConjunto(
            Guid conjuntoId,
            [FromQuery] bool? soloPropietarios,
            CancellationToken ct)
        {
            var result = await _mediator.Send(
                new ObtenerResidentesQuery(conjuntoId, soloPropietarios), ct);

            return Ok(result.Datos);
        }

        /// <summary>Crear residente</summary>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Crear(
            [FromBody] CrearResidenteCommand command,
            CancellationToken ct)
        {
            var result = await _mediator.Send(command, ct);

            if (!result.Exitoso)
                return BadRequest(new { result.Error });

            return Created(string.Empty, new { Id = result.Datos });
        }

        /// <summary>Actualizar PIN del residente autenticado</summary>
        [HttpPatch("pin")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> ActualizarPin(
            [FromBody] ActualizarPinRequest request,
            CancellationToken ct)
        {
            var residenteId = User.GetResidenteId();

            var command = new ActualizarPinCommand(
                residenteId,
                request.PinActual,
                request.NuevoPin);

            var result = await _mediator.Send(command, ct);

            if (!result.Exitoso)
                return BadRequest(new { result.Error });

            return NoContent();
        }
    }

    // DTO local para no exponer ResidenteId desde el body
    public record ActualizarPinRequest(string PinActual, string NuevoPin);
}
