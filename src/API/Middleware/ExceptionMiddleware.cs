using System.Net;
using System.Text.Json;
using VotacionesResidenciales.Application.Common.Exceptions;
using VotacionesResidenciales.Domain.Exceptions;

namespace VotacionesResidenciales.API.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;

        public ExceptionMiddleware(RequestDelegate next,
            ILogger<ExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error no controlado: {Mensaje}", ex.Message);
                await ManejarExcepcionAsync(context, ex);
            }
        }

        private static async Task ManejarExcepcionAsync(
            HttpContext context, Exception ex)
        {
            context.Response.ContentType = "application/json";

            var (statusCode, mensaje, errores) = ex switch
            {
                NotFoundException nfe =>
                    (HttpStatusCode.NotFound,
                     nfe.Message,
                     (IDictionary<string, string[]>?)null),

                ValidationException ve =>
                    (HttpStatusCode.BadRequest,
                     "Error de validación.",
                     ve.Errores),

                DomainException de =>
                    (HttpStatusCode.UnprocessableEntity,
                     de.Message,
                     null),

                UnauthorizedAccessException =>
                    (HttpStatusCode.Unauthorized,
                     "No autorizado.",
                     null),

                _ =>
                    (HttpStatusCode.InternalServerError,
                     "Ocurrió un error interno. Intente más tarde.",
                     null)
            };

            context.Response.StatusCode = (int)statusCode;

            var respuesta = new
            {
                Status = (int)statusCode,
                Mensaje = mensaje,
                Errores = errores
            };

            var json = JsonSerializer.Serialize(respuesta,
                new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                });

            await context.Response.WriteAsync(json);
        }
    }
}
