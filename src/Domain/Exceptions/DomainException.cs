using System;
using System.Collections.Generic;
using System.Text;

namespace VotacionesResidenciales.Domain.Exceptions
{
    public class DomainException : Exception
    {
        public DomainException(string mensaje) : base(mensaje) { }
    }
}
