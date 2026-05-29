using System;
using System.Collections.Generic;
using System.Text;

namespace VotacionesResidenciales.Domain.Exceptions
{
    public class VotoDuplicadoException : DomainException
    {
        public VotoDuplicadoException(Guid residenteId, Guid votacionId)
            : base($"El residente {residenteId} ya votó en la votación {votacionId}.") { }
    }
}
