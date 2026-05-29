using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace VotacionesResidenciales.Application.Features.Auth.Commands.Login
{
    public class LoginCommandValidator : AbstractValidator<LoginCommand>
    {
        public LoginCommandValidator()
        {
            RuleFor(x => x.Cedula)
                .NotEmpty().WithMessage("La cédula es requerida.")
                .MinimumLength(5).WithMessage("Cédula inválida.")
                .MaximumLength(12).WithMessage("Cédula inválida.")
                .Matches(@"^\d+$").WithMessage("La cédula solo debe contener números.");

            RuleFor(x => x.Pin)
                .NotEmpty().WithMessage("El PIN es requerido.")
                .Length(4, 6).WithMessage("El PIN debe tener entre 4 y 6 dígitos.")
                .Matches(@"^\d+$").WithMessage("El PIN solo debe contener números.");
        }
    }
}
