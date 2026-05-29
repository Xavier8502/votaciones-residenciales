using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace VotacionesResidenciales.Application.Common.Behaviors
{
    public class LoggingBehavior<TRequest, TResponse>
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
    {
        private readonly ILogger<LoggingBehavior<TRequest, TResponse>> _logger;

        public LoggingBehavior(
            ILogger<LoggingBehavior<TRequest, TResponse>> logger)
        {
            _logger = logger;
        }

        public async Task<TResponse> Handle(TRequest request,
            RequestHandlerDelegate<TResponse> next,
            CancellationToken ct)
        {
            var nombre = typeof(TRequest).Name;

            _logger.LogInformation("Ejecutando request: {Nombre}", nombre);

            var response = await next();

            _logger.LogInformation("Request completado: {Nombre}", nombre);

            return response;
        }
    }
}
