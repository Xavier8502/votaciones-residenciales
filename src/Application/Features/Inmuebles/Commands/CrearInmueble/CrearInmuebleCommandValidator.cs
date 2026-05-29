using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace VotacionesResidenciales.Application.Features.Inmuebles.Commands.CrearInmueble
{
    public class CrearInmuebleCommandValidator
    : AbstractValidator<CrearInmuebleCommand>
    {
        public CrearInmuebleCommandValidator()
        {
            RuleFor(x => x.ConjuntoId)
                .NotEmpty().WithMessage("El conjunto es requerido.");

            RuleFor(x => x.Numero)
                .NotEmpty().WithMessage("El número del inmueble es requerido.")
                .MaximumLength(20).WithMessage("El número no puede superar 20 caracteres.");

            RuleFor(x => x.Coeficiente)
                .GreaterThan(0).WithMessage("El coeficiente debe ser mayor a cero.")
                .LessThanOrEqualTo(100).WithMessage("El coeficiente no puede ser mayor a 100.");

            RuleFor(x => x.Torre)
                .MaximumLength(20).WithMessage("La torre no puede superar 20 caracteres.")
                .When(x => !string.IsNullOrWhiteSpace(x.Torre));

            RuleFor(x => x.Piso)
                .MaximumLength(10).WithMessage("El piso no puede superar 10 caracteres.")
                .When(x => !string.IsNullOrWhiteSpace(x.Piso));
        }
    }
}
