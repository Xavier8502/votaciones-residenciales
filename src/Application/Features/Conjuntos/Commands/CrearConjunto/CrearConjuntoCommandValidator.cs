using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace VotacionesResidenciales.Application.Features.Conjuntos.Commands.CrearConjunto
{
    public class CrearConjuntoCommandValidator
    : AbstractValidator<CrearConjuntoCommand>
    {
        public CrearConjuntoCommandValidator()
        {
            RuleFor(x => x.Nombre)
                .NotEmpty().WithMessage("El nombre es requerido.")
                .MaximumLength(200).WithMessage("El nombre no puede superar 200 caracteres.");

            RuleFor(x => x.Nit)
                .NotEmpty().WithMessage("El NIT es requerido.")
                .MaximumLength(20).WithMessage("El NIT no puede superar 20 caracteres.")
                .Matches(@"^[\d\-]+$").WithMessage("El NIT solo puede contener números y guiones.");

            RuleFor(x => x.Direccion)
                .NotEmpty().WithMessage("La dirección es requerida.")
                .MaximumLength(300).WithMessage("La dirección no puede superar 300 caracteres.");

            RuleFor(x => x.Ciudad)
                .NotEmpty().WithMessage("La ciudad es requerida.")
                .MaximumLength(100).WithMessage("La ciudad no puede superar 100 caracteres.");

            RuleFor(x => x.Email)
                .EmailAddress().WithMessage("El email no es válido.")
                .When(x => !string.IsNullOrWhiteSpace(x.Email));

            RuleFor(x => x.Telefono)
                .MaximumLength(20).WithMessage("El teléfono no puede superar 20 caracteres.")
                .When(x => !string.IsNullOrWhiteSpace(x.Telefono));
        }
    }
}
