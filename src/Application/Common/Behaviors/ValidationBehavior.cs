using FluentValidation;
using MediatR;
using ValidationException =
    VotacionesResidenciales.Application.Common.Exceptions.ValidationException;


namespace VotacionesResidenciales.Application.Common.Behaviors
{
    public class ValidationBehavior<TRequest, TResponse>
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
    {
        private readonly IEnumerable<IValidator<TRequest>> _validators;

        public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
        {
            _validators = validators;
        }

        public async Task<TResponse> Handle(TRequest request,
            RequestHandlerDelegate<TResponse> next,
            CancellationToken ct)
        {
            if (!_validators.Any()) return await next();

            var context = new ValidationContext<TRequest>(request);

            var errores = _validators
                .Select(v => v.Validate(context))
                .SelectMany(r => r.Errors)
                .Where(f => f != null)
                .GroupBy(f => f.PropertyName,
                         f => f.ErrorMessage,
                        (prop, msgs) => new { prop, msgs })
                .ToDictionary(x => x.prop, x => x.msgs.ToArray());

            if (errores.Count != 0)
                throw new ValidationException(errores);

            return await next();
        }
    }
}
