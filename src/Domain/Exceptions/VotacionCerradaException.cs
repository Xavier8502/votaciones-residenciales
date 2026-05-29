using System;
using System.Collections.Generic;
using System.Text;

namespace VotacionesResidenciales.Domain.Exceptions
{
    public class VotacionCerradaException : DomainException
    {
        public VotacionCerradaException(Guid votacionId)
            : base($"La votación {votacionId} no está abierta para recibir votos.") { }
    }
}
