using System;
using System.Collections.Generic;
using System.Text;

namespace VotacionesResidenciales.Application.Common.Interfaces
{
    public interface IAuthService
    {
        string GenerarToken(Guid residenteId, string cedula,
            string rol, Guid conjuntoId);
        string HashPin(string pin);
        bool VerificarPin(string pin, string hash);
    }
}
