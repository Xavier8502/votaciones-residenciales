using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VotacionesResidenciales.Application.Features.Auth.Commands.Login;

namespace VotacionesResidenciales.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AuthController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>Login con cédula y PIN</summary>
        [HttpPost("login")]
        [ProducesResponseType(typeof(LoginResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Login(
            [FromBody] LoginCommand command,
            CancellationToken ct)
        {
            var result = await _mediator.Send(command, ct);

            if (!result.Exitoso)
                return BadRequest(new { result.Error });

            return Ok(result.Datos);
        }
    }
}
