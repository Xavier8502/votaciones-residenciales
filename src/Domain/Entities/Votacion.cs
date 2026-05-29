using System;
using System.Collections.Generic;
using System.Text;
using VotacionesResidenciales.Domain.Common;
using VotacionesResidenciales.Domain.Enums;
using VotacionesResidenciales.Domain.Exceptions;

namespace VotacionesResidenciales.Domain.Entities
{
    public class Votacion : BaseEntity
    {
        public Guid ConjuntoId { get; private set; }
        public string Titulo { get; private set; } = string.Empty;
        public string? Descripcion { get; private set; }
        public EstadoVotacion Estado { get; private set; }
        public TipoPesoVoto TipoPeso { get; private set; }
        public decimal QuorumRequerido { get; private set; }
        public DateTime FechaInicio { get; private set; }
        public DateTime FechaFin { get; private set; }
        public bool MostrarResultadosParciales { get; private set; }
        public string? ActaUrl { get; private set; }

        private readonly List<Pregunta> _preguntas = [];
        public IReadOnlyCollection<Pregunta> Preguntas => _preguntas.AsReadOnly();

        private readonly List<Voto> _votos = [];
        public IReadOnlyCollection<Voto> Votos => _votos.AsReadOnly();

        private Votacion() { }

        public static Votacion Crear(Guid conjuntoId, string titulo,
            string? descripcion, TipoPesoVoto tipoPeso,
            decimal quorumRequerido, DateTime fechaInicio,
            DateTime fechaFin, bool mostrarResultadosParciales = false)
        {
            if (string.IsNullOrWhiteSpace(titulo))
                throw new ArgumentException("El título es requerido.");

            if (quorumRequerido <= 0 || quorumRequerido > 100)
                throw new ArgumentException("El quórum debe estar entre 1 y 100.");

            if (fechaFin <= fechaInicio)
                throw new ArgumentException("La fecha fin debe ser posterior a la fecha inicio.");

            return new Votacion
            {
                ConjuntoId = conjuntoId,
                Titulo = titulo.Trim(),
                Descripcion = descripcion?.Trim(),
                Estado = EstadoVotacion.Borrador,
                TipoPeso = tipoPeso,
                QuorumRequerido = quorumRequerido,
                FechaInicio = fechaInicio,
                FechaFin = fechaFin,
                MostrarResultadosParciales = mostrarResultadosParciales
            };
        }

        public void Abrir()
        {
            if (Estado != EstadoVotacion.Borrador)
                throw new DomainException("Solo se puede abrir una votación en estado Borrador.");

            if (!_preguntas.Any())
                throw new DomainException("La votación debe tener al menos una pregunta.");

            Estado = EstadoVotacion.Abierta;
            MarcarModificado();
        }

        public void Cerrar()
        {
            if (Estado != EstadoVotacion.Abierta)
                throw new DomainException("Solo se puede cerrar una votación abierta.");

            Estado = EstadoVotacion.Cerrada;
            MarcarModificado();
        }

        public void MarcarActaGenerada(string actaUrl)
        {
            if (Estado != EstadoVotacion.Cerrada)
                throw new DomainException("Solo se puede generar acta de una votación cerrada.");

            ActaUrl = actaUrl;
            Estado = EstadoVotacion.ActaGenerada;
            MarcarModificado();
        }

        public void AgregarPregunta(Pregunta pregunta)
        {
            if (Estado != EstadoVotacion.Borrador)
                throw new DomainException("No se pueden agregar preguntas a una votación que no está en borrador.");

            _preguntas.Add(pregunta);
        }

        public bool EstaAbierta() => Estado == EstadoVotacion.Abierta;
            //&& DateTime.UtcNow >= FechaInicio
            //&& DateTime.UtcNow <= FechaFin;
    }
}
