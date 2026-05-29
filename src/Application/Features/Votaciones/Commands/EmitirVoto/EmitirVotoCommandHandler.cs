using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using VotacionesResidenciales.Application.Common.Exceptions;
using VotacionesResidenciales.Application.Common.Interfaces;
using VotacionesResidenciales.Application.Common.Models;
using VotacionesResidenciales.Domain.Entities;
using VotacionesResidenciales.Domain.Enums;
using VotacionesResidenciales.Domain.Interfaces;
using VotacionesResidenciales.Domain.Interfaces.Repositories;

namespace VotacionesResidenciales.Application.Features.Votaciones.Commands.EmitirVoto
{
    public class EmitirVotoCommandHandler
    : IRequestHandler<EmitirVotoCommand, Result<Guid>>
    {
        private readonly IVotacionRepository _votacionRepo;
        private readonly IResidenteRepository _residenteRepo;
        private readonly IInmuebleRepository _inmuebleRepo;
        private readonly IVotoRepository _votoRepo;
        private readonly IVotacionHubService _hubService;
        private readonly IUnitOfWork _uow;

        public EmitirVotoCommandHandler(
            IVotacionRepository votacionRepo,
            IResidenteRepository residenteRepo,
            IInmuebleRepository inmuebleRepo,
            IVotoRepository votoRepo,
            IVotacionHubService hubService,
            IUnitOfWork uow)
        {
            _votacionRepo = votacionRepo;
            _residenteRepo = residenteRepo;
            _inmuebleRepo = inmuebleRepo;
            _votoRepo = votoRepo;
            _hubService = hubService;
            _uow = uow;
        }

        public async Task<Result<Guid>> Handle(
            EmitirVotoCommand request, CancellationToken ct)
        {
            // 1. Validar que no haya votado ya
            var yaVoto = await _votoRepo
                .YaVotoAsync(request.ResidenteId, request.VotacionId, ct);

            if (yaVoto)
                return Result<Guid>.Fallo("El residente ya emitió su voto.");

            // 2. Obtener votación con detalles
            var votacion = await _votacionRepo
                .ObtenerConDetallesAsync(request.VotacionId, ct);

            if (votacion is null)
                throw new NotFoundException(nameof(Votacion), request.VotacionId);

            // 3. Obtener residente
            var residente = await _residenteRepo
                .ObtenerPorIdAsync(request.ResidenteId, ct);

            if (residente is null)
                throw new NotFoundException(nameof(Residente), request.ResidenteId);

            // 4. Calcular peso del voto
            decimal peso = 1;

            if (votacion.TipoPeso == TipoPesoVoto.PorCoeficiente)
            {
                var inmueble = await _inmuebleRepo
                    .ObtenerPorIdAsync(residente.InmuebleId, ct);

                peso = inmueble?.Coeficiente ?? 1;
            }

            // 5. Crear voto — la entidad valida reglas de negocio
            var voto = Voto.Crear(votacion, residente, peso);

            foreach (var respuesta in request.Respuestas)
            {
                var detalle = DetalleVoto.Crear(
                    voto.Id,
                    respuesta.PreguntaId,
                    respuesta.OpcionId);

                voto.AgregarDetalle(detalle);
            }

            await _votoRepo.AgregarAsync(voto, ct);
            await _uow.GuardarCambiosAsync(ct);

            // 6. Notificar en tiempo real
            var totalVotos = await _votoRepo
                .ContarVotosPorVotacionAsync(request.VotacionId, ct);

            var totalCoeficientes = await _inmuebleRepo
                .SumarCoeficientesAsync(votacion.ConjuntoId, ct);

            var porcentaje = votacion.TipoPeso == TipoPesoVoto.PorCoeficiente
                ? (totalVotos * peso / totalCoeficientes) * 100
                : 0;

            await _hubService.NotificarNuevoVotoAsync(
                request.VotacionId, totalVotos, porcentaje);

            return Result<Guid>.Exito(voto.Id);
        }
    }
}
