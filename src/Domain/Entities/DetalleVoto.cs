using System;
using System.Collections.Generic;
using System.Text;
using VotacionesResidenciales.Domain.Common;

namespace VotacionesResidenciales.Domain.Entities
{
    public class DetalleVoto : BaseEntity
    {
        public Guid VotoId { get; private set; }
        public Guid PreguntaId { get; private set; }
        public Guid OpcionId { get; private set; }

        private DetalleVoto() { }

        public static DetalleVoto Crear(Guid votoId, Guid preguntaId, Guid opcionId)
        {
            return new DetalleVoto
            {
                VotoId = votoId,
                PreguntaId = preguntaId,
                OpcionId = opcionId
            };
        }
    }
}
