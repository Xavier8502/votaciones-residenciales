using System;
using System.Collections.Generic;
using System.Text;

namespace VotacionesResidenciales.Application.Common.Interfaces
{
    public interface IVotacionHubService
    {
        Task NotificarNuevoVotoAsync(Guid votacionId,
            int totalVotos, decimal porcentajeParticipacion);
        Task NotificarCambioEstadoAsync(Guid votacionId, string nuevoEstado);
    }
}
