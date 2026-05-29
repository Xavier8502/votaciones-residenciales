using System;
using System.Collections.Generic;
using System.Text;
using VotacionesResidenciales.Domain.Common;

namespace VotacionesResidenciales.Domain.Entities
{
    public class Residente : BaseEntity
    {
        public Guid InmuebleId { get; private set; }
        public string Cedula { get; private set; } = string.Empty;
        public string Nombre { get; private set; } = string.Empty;
        public string Apellido { get; private set; } = string.Empty;
        public string PinHash { get; private set; } = string.Empty;
        public string? Telefono { get; private set; }
        public string? Email { get; private set; }
        public bool EsPropietario { get; private set; }

        public Inmueble? Inmueble { get; private set; }

        private Residente() { }

        public static Residente Crear(Guid inmuebleId, string cedula,
            string nombre, string apellido,
            string pinHash, bool esPropietario = true)
        {
            var ced = ValueObjects.Cedula.Crear(cedula);

            return new Residente
            {
                InmuebleId = inmuebleId,
                Cedula = ced.Valor,
                Nombre = nombre.Trim(),
                Apellido = apellido.Trim(),
                PinHash = pinHash,
                EsPropietario = esPropietario
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
