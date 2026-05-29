using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Text;
using VotacionesResidenciales.Application.Common.Interfaces;
using VotacionesResidenciales.Infrastructure.Hubs;

namespace VotacionesResidenciales.Infrastructure.Services
{
    public class VotacionHubService : IVotacionHubService
    {
        private readonly IHubContext<VotacionHub> _hubContext;

        public VotacionHubService(IHubContext<VotacionHub> hubContext)
        {
            _hubContext = hubContext;
        }

        public async Task NotificarNuevoVotoAsync(Guid votacionId,
            int totalVotos, decimal porcentajeParticipacion)
            => await _hubContext.Clients
                .Group(votacionId.ToString())
                .SendAsync("NuevoVoto", new
                {
                    VotacionId = votacionId,
                    TotalVotos = totalVotos,
                    PorcentajeParticipacion = porcentajeParticipacion
                });

        public async Task NotificarCambioEstadoAsync(
            Guid votacionId, string nuevoEstado)
            => await _hubContext.Clients
                .Group(votacionId.ToString())
                .SendAsync("CambioEstado", new
                {
                    VotacionId = votacionId,
                    Estado = nuevoEstado
                });
    }
}
