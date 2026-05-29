using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using VotacionesResidenciales.Application.Common.Models;

namespace VotacionesResidenciales.Application.Features.Votaciones.Commands.CerrarVotacion
{
    public record CerrarVotacionCommand(Guid VotacionId)
     : IRequest<Result>;
}
