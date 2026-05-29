using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using VotacionesResidenciales.Application.Common.Exceptions;
using VotacionesResidenciales.Application.Common.Interfaces;
using VotacionesResidenciales.Application.Common.Models;
using VotacionesResidenciales.Domain.Entities;
using VotacionesResidenciales.Domain.Interfaces;
using VotacionesResidenciales.Domain.Interfaces.Repositories;

namespace VotacionesResidenciales.Application.Features.Votaciones.Commands.AbrirVotacion
{
    public class AbrirVotacionCommandHandler
    : IRequestHandler<AbrirVotacionCommand, Result>
    {
        private readonly IVotacionRepository _votacionRepo;
        private readonly IVotacionHubService _hubService;
        private readonly IUnitOfWork _uow;

        public AbrirVotacionCommandHandler(
            IVotacionRepository votacionRepo,
            IVotacionHubService hubService,
            IUnitOfWork uow)
        {
            _votacionRepo = votacionRepo;
            _hubService = hubService;
            _uow = uow;
        }

        public async Task<Result> Handle(
            AbrirVotacionCommand request, CancellationToken ct)
        {
            var votacion = await _votacionRepo
                .ObtenerConDetallesAsync(request.VotacionId, ct);

            if (votacion is null)
                throw new NotFoundException(nameof(Votacion), request.VotacionId);

            votacion.Abrir();

            await _votacionRepo.ActualizarAsync(votacion, ct);
            await _uow.GuardarCambiosAsync(ct);

            await _hubService.NotificarCambioEstadoAsync(
                request.VotacionId, "Abierta");

            return Result.Exito();
        }
    }
}
