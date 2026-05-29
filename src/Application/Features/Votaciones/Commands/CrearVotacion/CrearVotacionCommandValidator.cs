using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace VotacionesResidenciales.Application.Features.Votaciones.Commands.CrearVotacion
{
    public class CrearVotacionCommandValidator
    : AbstractValidator<CrearVotacionCommand>
    {
        public CrearVotacionCommandValidator()
        {
            RuleFor(x => x.Titulo)
                .NotEmpty().WithMessage("El título es requerido.")
                .MaximumLength(200).WithMessage("El título no puede superar 200 caracteres.");

            RuleFor(x => x.QuorumRequerido)
                .InclusiveBetween(1, 100)
                .WithMessage("El quórum debe estar entre 1 y 100.");

            RuleFor(x => x.FechaInicio)
                .GreaterThan(DateTime.UtcNow)
                .WithMessage("La fecha inicio debe ser futura.");

            RuleFor(x => x.FechaFin)
                .GreaterThan(x => x.FechaInicio)
                .WithMessage("La fecha fin debe ser posterior a la fecha inicio.");

            RuleFor(x => x.Preguntas)
                .NotEmpty().WithMessage("Debe tener al menos una pregunta.")
                .Must(p => p.Count <= 20)
                .WithMessage("No puede tener más de 20 preguntas.");

            RuleForEach(x => x.Preguntas).ChildRules(pregunta =>
            {
                pregunta.RuleFor(p => p.Texto)
                    .NotEmpty().WithMessage("El texto de la pregunta es requerido.");

                pregunta.RuleFor(p => p.Opciones)
                    .NotEmpty().WithMessage("Cada pregunta debe tener al menos 2 opciones.")
                    .Must(o => o.Count >= 2)
                    .WithMessage("Cada pregunta debe tener al menos 2 opciones.");
            });
        }
    }
}
