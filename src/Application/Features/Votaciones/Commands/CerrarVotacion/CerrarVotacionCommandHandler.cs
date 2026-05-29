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

namespace VotacionesResidenciales.Application.Features.Votaciones.Commands.CerrarVotacion
{
    public class CerrarVotacionCommandHandler
    : IRequestHandler<CerrarVotacionCommand, Result>
    {
        private readonly IVotacionRepository _votacionRepo;
        private readonly IVotacionHubService _hubService;
        private readonly IUnitOfWork _uow;

        public CerrarVotacionCommandHandler(
            IVotacionRepository votacionRepo,
            IVotacionHubService hubService,
            IUnitOfWork uow)
        {
            _votacionRepo = votacionRepo;
            _hubService = hubService;
            _uow = uow;
        }

        public async Task<Result> Handle(
            CerrarVotacionCommand request, CancellationToken ct)
        {
            var votacion = await _votacionRepo
                .ObtenerPorIdAsync(request.VotacionId, ct);

            if (votacion is null)
                throw new NotFoundException(nameof(Votacion), request.VotacionId);

            votacion.Cerrar();

            await _votacionRepo.ActualizarAsync(votacion, ct);
            await _uow.GuardarCambiosAsync(ct);

            await _hubService.NotificarCambioEstadoAsync(
                request.VotacionId, "Cerrada");

            return Result.Exito();
        }
    }
}
