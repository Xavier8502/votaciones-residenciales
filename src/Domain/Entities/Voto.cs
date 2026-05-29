using System;
using System.Collections.Generic;
using System.Text;
using VotacionesResidenciales.Domain.Common;
using VotacionesResidenciales.Domain.Exceptions;

namespace VotacionesResidenciales.Domain.Entities
{
    public class Voto : BaseEntity
    {
        public Guid VotacionId { get; private set; }
        public Guid ResidenteId { get; private set; }
        public decimal PesoAplicado { get; private set; }
        public DateTime FechaHora { get; private set; }

        private readonly List<DetalleVoto> _detalles = [];
        public IReadOnlyCollection<DetalleVoto> Detalles => _detalles.AsReadOnly();

        private Voto() { }

        public static Voto Crear(Votacion votacion, Residente residente,
            decimal pesoAplicado)
        {
            if (!votacion.EstaAbierta())
                throw new VotacionCerradaException(votacion.Id);

            if (votacion.Votos.Any(v => v.ResidenteId == residente.Id))
                throw new VotoDuplicadoException(residente.Id, votacion.Id);

            return new Voto
            {
                VotacionId = votacion.Id,
                ResidenteId = residente.Id,
                PesoAplicado = pesoAplicado,
                FechaHora = DateTime.UtcNow
            };
        }

        public void AgregarDetalle(DetalleVoto detalle)
        {
            _detalles.Add(detalle);
        }
    }
}
