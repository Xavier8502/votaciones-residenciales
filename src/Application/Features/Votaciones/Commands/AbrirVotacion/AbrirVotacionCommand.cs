using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using VotacionesResidenciales.Application.Common.Models;

namespace VotacionesResidenciales.Application.Features.Votaciones.Commands.AbrirVotacion
{
    public record AbrirVotacionCommand(Guid VotacionId)
    : IRequest<Result>;
}
