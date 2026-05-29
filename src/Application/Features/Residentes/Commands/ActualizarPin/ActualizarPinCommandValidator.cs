using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace VotacionesResidenciales.Application.Features.Residentes.Commands.ActualizarPin
{
    public class ActualizarPinCommandValidator
    : AbstractValidator<ActualizarPinCommand>
    {
        public ActualizarPinCommandValidator()
        {
            RuleFor(x => x.ResidenteId)
                .NotEmpty().WithMessage("El Id del residente es requerido.");

            RuleFor(x => x.PinActual)
                .NotEmpty().WithMessage("El PIN actual es requerido.")
                .Length(4, 6).WithMessage("El PIN debe tener entre 4 y 6 dígitos.")
                .Matches(@"^\d+$").WithMessage("El PIN solo debe contener números.");

            RuleFor(x => x.NuevoPin)
                .NotEmpty().WithMessage("El nuevo PIN es requerido.")
                .Length(4, 6).WithMessage("El PIN debe tener entre 4 y 6 dígitos.")
                .Matches(@"^\d+$").WithMessage("El PIN solo debe contener números.")
                .NotEqual(x => x.PinActual)
                .WithMessage("El nuevo PIN debe ser diferente al actual.");
        }
    }
}
