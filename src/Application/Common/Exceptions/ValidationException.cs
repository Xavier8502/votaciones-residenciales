using System;
using System.Collections.Generic;
using System.Text;

namespace VotacionesResidenciales.Application.Common.Exceptions
{
    public class ValidationException : Exception
    {
        public IDictionary<string, string[]> Errores { get; }

        public ValidationException(IDictionary<string, string[]> errores)
            : base("Se produjeron uno o más errores de validación.")
        {
            Errores = errores;
        }
    }
}
