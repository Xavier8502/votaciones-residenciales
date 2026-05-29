using System;
using System.Collections.Generic;
using System.Text;

namespace VotacionesResidenciales.Domain.Exceptions
{
    public class QuorumNoAlcanzadoException : DomainException
    {
        public QuorumNoAlcanzadoException(decimal actual, decimal requerido)
            : base($"Quórum insuficiente. Actual: {actual:F2}% — Requerido: {requerido:F2}%") { }
    }
}
