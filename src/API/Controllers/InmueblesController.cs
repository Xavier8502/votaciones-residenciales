using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VotacionesResidenciales.Application.Features.Inmuebles.Commands.ActualizarInmueble;
using VotacionesResidenciales.Application.Features.Inmuebles.Commands.CrearInmueble;
using VotacionesResidenciales.Application.Features.Inmuebles.Queries.ObtenerInmuebles;
using VotacionesResidenciales.Domain.Enums;

namespace VotacionesResidenciales.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class InmueblesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public InmueblesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>Obtener inmuebles por conjunto</summary>
        [HttpGet("conjunto/{conjuntoId:guid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> ObtenerPorConjunto(
            Guid conjuntoId,
            [FromQuery] TipoInmueble? tipo,
            CancellationToken ct)
        {
            var result = await _mediator.Send(
                new ObtenerInmueblesQuery(conjuntoId, tipo), ct);

            return Ok(result.Datos);
        }

        /// <summary>Crear inmueble</summary>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Crear(
            [FromBody] CrearInmuebleCommand command,
            CancellationToken ct)
        {
            var result = await _mediator.Send(command, ct);

            if (!result.Exitoso)
                return BadRequest(new { result.Error });

            return Created(string.Empty, new { Id = result.Datos });
        }

        /// <summary>Actualizar inmueble</summary>
        [HttpPut("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Actualizar(
            Guid id,
            [FromBody] ActualizarInmuebleCommand command,
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
