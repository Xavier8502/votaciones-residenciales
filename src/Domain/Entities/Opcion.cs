using System;
using System.Collections.Generic;
using System.Text;
using VotacionesResidenciales.Domain.Common;

namespace VotacionesResidenciales.Domain.Entities
{
    public class Opcion : BaseEntity
    {
        public Guid PreguntaId { get; private set; }
        public string Texto { get; private set; } = string.Empty;
        public int Orden { get; private set; }

        private Opcion() { }

        public static Opcion Crear(Guid preguntaId, string texto, int orden)
        {
            if (string.IsNullOrWhiteSpace(texto))
                throw new ArgumentException("El texto de la opción es requerido.");

            return new Opcion
            {
                PreguntaId = preguntaId,
                Texto = texto.Trim(),
                Orden = orden
            };
        }
    }
}
