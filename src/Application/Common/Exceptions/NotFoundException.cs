using System;
using System.Collections.Generic;
using System.Text;

namespace VotacionesResidenciales.Application.Common.Exceptions
{
    public class NotFoundException : Exception
    {
        public NotFoundException(string entidad, object id)
            : base($"{entidad} con id '{id}' no fue encontrado.") { }
    }
}
