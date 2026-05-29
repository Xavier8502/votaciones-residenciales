using System;
using System.Collections.Generic;
using System.Text;
using VotacionesResidenciales.Domain.Common;

namespace VotacionesResidenciales.Domain.Entities
{
    public class AdminConjunto : BaseEntity
    {
        public Guid ConjuntoId { get; private set; }
        public string Cedula { get; private set; } = string.Empty;
        public string Nombre { get; private set; } = string.Empty;
        public string Apellido { get; private set; } = string.Empty;
        public string PinHash { get; private set; } = string.Empty;
        public string? Telefono { get; private set; }
        public string? Email { get; private set; }

        public Conjunto? Conjunto { get; private set; }

        private AdminConjunto() { }

        public static AdminConjunto Crear(
            Guid conjuntoId,
            string cedula,
            string nombre,
            string apellido,
            string pinHash)
        {
            var ced = ValueObjects.Cedula.Crear(cedula);
            
            return new AdminConjunto
            {
                ConjuntoId = conjuntoId,
                Cedula = ced.Valor,
                Nombre = nombre.Trim(),
                Apellido = apellido.Trim(),
                PinHash = pinHash
            };
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
