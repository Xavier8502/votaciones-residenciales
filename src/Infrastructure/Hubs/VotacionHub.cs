using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace VotacionesResidenciales.Infrastructure.Hubs
{
    [Authorize]
    public class VotacionHub : Hub
    {
        // El cliente se une al grupo de una votación específica
        public async Task UnirseAVotacion(string votacionId)
            => await Groups.AddToGroupAsync(
                Context.ConnectionId, votacionId);

        // El cliente abandona el grupo
        public async Task SalirDeVotacion(string votacionId)
            => await Groups.RemoveFromGroupAsync(
                Context.ConnectionId, votacionId);

        public override async Task OnDisconnectedAsync(Exception? exception)
            => await base.OnDisconnectedAsync(exception);
    }
}
