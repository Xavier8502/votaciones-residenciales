using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace VotacionesResidenciales.Application.Features.Inmuebles.Commands.ActualizarInmueble
{
    public class ActualizarInmuebleCommandValidator
    : AbstractValidator<ActualizarInmuebleCommand>
    {
        public ActualizarInmuebleCommandValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("El Id es requerido.");

            RuleFor(x => x.Coeficiente)
                .GreaterThan(0).WithMessage("El coeficiente debe ser mayor a cero.")
                .LessThanOrEqualTo(100).WithMessage("El coeficiente no puede ser mayor a 100.");
        }
    }
}
