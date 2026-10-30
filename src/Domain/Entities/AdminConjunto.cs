
using System;
using System.Collections.Generic;
using VotacionesResidenciales.Domain.Common;

namespace VotacionesResidenciales.Domain.Entities
{
    public class AdminConjunto : BaseEntity
    {
        public string Cedula { get; private set; } = string.Empty;
        public string Nombre { get; private set; } = string.Empty;
        public string Apellido { get; private set; } = string.Empty;
        public string PinHash { get; private set; } = string.Empty;
        public string? Telefono { get; private set; }
        public string? Email { get; private set; }

        private readonly List<AdminConjuntoAsignacion> _asignaciones = [];

        public IReadOnlyCollection<AdminConjuntoAsignacion> Asignaciones =>
            _asignaciones.AsReadOnly();

        private AdminConjunto() { }

        public static AdminConjunto Crear(
            string cedula,
            string nombre,
            string apellido,
            string pinHash)
        {
            var ced = ValueObjects.Cedula.Crear(cedula);

            return new AdminConjunto
            {
                Cedula = ced.Valor,
                Nombre = nombre.Trim(),
                Apellido = apellido.Trim(),
                PinHash = pinHash
            };
        }

        public void AgregarAsignacion(Conjunto conjunto)
        {
            ArgumentNullException.ThrowIfNull(conjunto);

            if (_asignaciones.Exists(a => a.ConjuntoId == conjunto.Id))
                return;

            _asignaciones.Add(
                AdminConjuntoAsignacion.Crear(Id, conjunto.Id));
        }

        public void ActualizarPin(string nuevoPinHash)
        {
            PinHash = nuevoPinHash;
            MarcarModificado();
        }

        public void ActualizarContacto(string? telefono, string? email)
        {
            Telefono = telefono?.Trim();
            Email = email?.Trim();
            MarcarModificado();
        }

        public string NombreCompleto => $"{Nombre} {Apellido}";
    }
}
