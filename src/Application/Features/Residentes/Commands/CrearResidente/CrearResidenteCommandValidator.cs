using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace VotacionesResidenciales.Application.Features.Residentes.Commands.CrearResidente
{
    public class CrearResidenteCommandValidator
    : AbstractValidator<CrearResidenteCommand>
    {
        public CrearResidenteCommandValidator()
        {
            RuleFor(x => x.InmuebleId)
                .NotEmpty().WithMessage("El inmueble es requerido.");

            RuleFor(x => x.Cedula)
                .NotEmpty().WithMessage("La cédula es requerida.")
                .MinimumLength(5).WithMessage("Cédula inválida.")
                .MaximumLength(12).WithMessage("Cédula inválida.")
                .Matches(@"^\d+$").WithMessage("La cédula solo debe contener números.");

            RuleFor(x => x.Nombre)
                .NotEmpty().WithMessage("El nombre es requerido.")
                .MaximumLength(100).WithMessage("El nombre no puede superar 100 caracteres.");

            RuleFor(x => x.Apellido)
                .NotEmpty().WithMessage("El apellido es requerido.")
                .MaximumLength(100).WithMessage("El apellido no puede superar 100 caracteres.");

            RuleFor(x => x.Pin)
                .NotEmpty().WithMessage("El PIN es requerido.")
                .Length(4, 6).WithMessage("El PIN debe tener entre 4 y 6 dígitos.")
                .Matches(@"^\d+$").WithMessage("El PIN solo debe contener números.");

            RuleFor(x => x.Email)
                .EmailAddress().WithMessage("El email no es válido.")
                .When(x => !string.IsNullOrWhiteSpace(x.Email));

            RuleFor(x => x.Telefono)
                .MaximumLength(20).WithMessage("El teléfono no puede superar 20 caracteres.")
                .When(x => !string.IsNullOrWhiteSpace(x.Telefono));
        }
    }
}
