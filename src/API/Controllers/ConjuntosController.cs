using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VotacionesResidenciales.Application.Features.Conjuntos.Commands.ActualizarConjunto;
using VotacionesResidenciales.Application.Features.Conjuntos.Commands.CrearConjunto;
using VotacionesResidenciales.Application.Features.Conjuntos.Queries.ObtenerConjunto;
using VotacionesResidenciales.Application.Features.Conjuntos.Queries.ObtenerConjuntos;

namespace VotacionesResidenciales.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ConjuntosController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ConjuntosController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>Obtener todos los conjuntos</summary>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> ObtenerTodos(CancellationToken ct)
        {
            var result = await _mediator.Send(
                new ObtenerConjuntosQuery(), ct);

            return Ok(result.Datos);
        }

        /// <summary>Obtener conjunto por Id</summary>
        [HttpGet("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> ObtenerPorId(
            Guid id, CancellationToken ct)
        {
            var result = await _mediator.Send(
                new ObtenerConjuntoQuery(id), ct);

            return Ok(result.Datos);
        }

        /// <summary>Crear conjunto</summary>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Crear(
            [FromBody] CrearConjuntoCommand command,
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

        /// <summary>Actualizar conjunto</summary>
        [HttpPut("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Actualizar(
            Guid id,
            [FromBody] ActualizarConjuntoCommand command,
            CancellationToken ct)
        {
            if (id != command.Id)
                return BadRequest(new { Error = "El Id no coincide." });

            var result = await _mediator.Send(command, ct);

            if (!result.Exitoso)
                return BadRequest(new { result.Error });

            return NoContent();
        }
    }
}
