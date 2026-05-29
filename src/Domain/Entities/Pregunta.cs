using System;
using System.Collections.Generic;
using System.Text;
using VotacionesResidenciales.Domain.Common;

namespace VotacionesResidenciales.Domain.Entities
{
    public class Pregunta : BaseEntity
    {
        public Guid VotacionId { get; private set; }
        public string Texto { get; private set; } = string.Empty;
        public int Orden { get; private set; }

        private readonly List<Opcion> _opciones = [];
        public IReadOnlyCollection<Opcion> Opciones => _opciones.AsReadOnly();

        private Pregunta() { }

        public static Pregunta Crear(Guid votacionId, string texto, int orden)
        {
            if (string.IsNullOrWhiteSpace(texto))
                throw new ArgumentException("El texto de la pregunta es requerido.");

            return new Pregunta
            {
                VotacionId = votacionId,
                Texto = texto.Trim(),
                Orden = orden
            };
        }

        public void AgregarOpcion(Opcion opcion)
        {
            if (_opciones.Count >= 10)
                throw new ArgumentException("Una pregunta no puede tener más de 10 opciones.");

            _opciones.Add(opcion);
        }
    }
}
