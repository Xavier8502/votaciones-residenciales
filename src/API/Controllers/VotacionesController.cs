using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VotacionesResidenciales.API.Extensions;
using VotacionesResidenciales.Application.Features.Votaciones.Commands.AbrirVotacion;
using VotacionesResidenciales.Application.Features.Votaciones.Commands.CerrarVotacion;
using VotacionesResidenciales.Application.Features.Votaciones.Commands.CrearVotacion;
using VotacionesResidenciales.Application.Features.Votaciones.Commands.EmitirVoto;
using VotacionesResidenciales.Application.Features.Votaciones.Queries.ObtenerResultados;
using VotacionesResidenciales.Application.Features.Votaciones.Queries.ObtenerVotacion;
using VotacionesResidenciales.Application.Features.Votaciones.Queries.ObtenerVotaciones;
using VotacionesResidenciales.Domain.Enums;

namespace VotacionesResidenciales.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class VotacionesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public VotacionesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>Obtener votaciones por conjunto</summary>
        [HttpGet("conjunto/{conjuntoId:guid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> ObtenerPorConjunto(
            Guid conjuntoId,
            [FromQuery] EstadoVotacion? estado,
            CancellationToken ct)
        {
            var result = await _mediator.Send(
                new ObtenerVotacionesQuery(conjuntoId, estado), ct);

            return Ok(result.Datos);
        }

        /// <summary>Obtener votación por Id</summary>
        [HttpGet("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> ObtenerPorId(
            Guid id, CancellationToken ct)
        {
            var result = await _mediator.Send(
                new ObtenerVotacionQuery(id), ct);

            return Ok(result.Datos);
        }

        /// <summary>Obtener resultados de una votación</summary>
        [HttpGet("{id:guid}/resultados")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> ObtenerResultados(
            Guid id, CancellationToken ct)
        {
            var result = await _mediator.Send(
                new ObtenerResultadosQuery(id), ct);

            if (!result.Exitoso)
                return BadRequest(new { result.Error });

            return Ok(result.Datos);
        }

        /// <summary>Crear votación</summary>
        [HttpPost]
        [Authorize(Roles = "AdminConjunto")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Crear(
            [FromBody] CrearVotacionCommand command,
            CancellationToken ct)
        {
            var result = await _mediator.Send(command, ct);

            if (!result.Exitoso)
                return BadRequest(new { result.Error });

            return CreatedAtAction(
                nameof(ObtenerPorId),
                new { id = result.Datos },
                new { Id = result.Datos });
        }

        /// <summary>Abrir votación</summary>
        [HttpPatch("{id:guid}/abrir")]
        [Authorize(Roles = "AdminConjunto")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        public async Task<IActionResult> Abrir(
            Guid id, CancellationToken ct)
        {
            var result = await _mediator.Send(
                new AbrirVotacionCommand(id), ct);

            if (!result.Exitoso)
                return BadRequest(new { result.Error });

            return NoContent();
        }

        /// <summary>Cerrar votación</summary>
        [HttpPatch("{id:guid}/cerrar")]
        [Authorize(Roles = "AdminConjunto")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        public async Task<IActionResult> Cerrar(
            Guid id, CancellationToken ct)
        {
            var result = await _mediator.Send(
                new CerrarVotacionCommand(id), ct);

            if (!result.Exitoso)
                return BadRequest(new { result.Error });

            return NoContent();
        }

        /// <summary>Emitir voto</summary>
        [HttpPost("{id:guid}/votar")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        public async Task<IActionResult> Votar(
            Guid id,
            [FromBody] EmitirVotoRequest request,
            CancellationToken ct)
        {
            var residenteId = User.GetResidenteId();

            var command = new EmitirVotoCommand(
                id,
                residenteId,
                request.Respuestas);

            var result = await _mediator.Send(command, ct);

            if (!result.Exitoso)
                return BadRequest(new { result.Error });

            return Created(string.Empty, new { Id = result.Datos });
        }
    }

    // DTO local para no exponer ResidenteId desde el body
    public record EmitirVotoRequest(List<RespuestaDto> Respuestas);
}
