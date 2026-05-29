using System;
using System.Collections.Generic;
using System.Text;
using VotacionesResidenciales.Domain.Common;

namespace VotacionesResidenciales.Domain.Entities
{
    public class Conjunto : BaseEntity
    {
        public string Nombre { get; private set; } = string.Empty;
        public string Nit { get; private set; } = string.Empty;
        public string Direccion { get; private set; } = string.Empty;
        public string Ciudad { get; private set; } = string.Empty;
        public string? Telefono { get; private set; }
        public string? Email { get; private set; }

        private readonly List<Inmueble> _inmuebles = [];
        public IReadOnlyCollection<Inmueble> Inmuebles => _inmuebles.AsReadOnly();

        private Conjunto() { }

        public static Conjunto Crear(string nombre, string nit,
            string direccion, string ciudad)
        {
            if (string.IsNullOrWhiteSpace(nombre))
                throw new ArgumentException("El nombre del conjunto es requerido.");

            if (string.IsNullOrWhiteSpace(nit))
                throw new ArgumentException("El NIT es requerido.");

            return new Conjunto
            {
                Nombre = nombre.Trim(),
                Nit = nit.Trim(),
                Direccion = direccion.Trim(),
                Ciudad = ciudad.Trim()
            };
        }

        public void ActualizarContacto(string? telefono, string? email)
        {
            Telefono = telefono?.Trim();
            Email = email?.Trim();
            MarcarModificado();
        }

        public void Actualizar(string nombre, string direccion, string ciudad)
        {
            if (string.IsNullOrWhiteSpace(nombre))
                throw new ArgumentException("El nombre es requerido.");

            Nombre = nombre.Trim();
            Direccion = direccion.Trim();
            Ciudad = ciudad.Trim();
            MarcarModificado();
        }
    }
}
