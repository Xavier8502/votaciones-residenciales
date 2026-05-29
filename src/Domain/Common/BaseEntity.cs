using System;
using System.Collections.Generic;
using System.Text;

namespace VotacionesResidenciales.Domain.Common
{
    public abstract class BaseEntity
    {
        public Guid Id { get; protected set; } = Guid.NewGuid();
        public DateTime CreadoEn { get; protected set; } = DateTime.UtcNow;
        public DateTime? ModificadoEn { get; protected set; }
        public bool Activo { get; protected set; } = true;

        protected void MarcarModificado()
        {
            ModificadoEn = DateTime.UtcNow;
        }
    }
}
