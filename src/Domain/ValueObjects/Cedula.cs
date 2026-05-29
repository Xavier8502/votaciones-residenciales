using System;
using System.Collections.Generic;
using System.Text;

namespace VotacionesResidenciales.Domain.ValueObjects
{
    public sealed class Cedula
    {
        public string Valor { get; }

        private Cedula(string valor) => Valor = valor;

        public static Cedula Crear(string valor)
        {
            if (string.IsNullOrWhiteSpace(valor))
                throw new ArgumentException("La cédula no puede estar vacía.");

            var limpia = valor.Trim().Replace(".", "").Replace("-", "");

            if (!limpia.All(char.IsDigit))
                throw new ArgumentException("La cédula solo puede contener números.");

            if (limpia.Length < 5 || limpia.Length > 12)
                throw new ArgumentException("La cédula debe tener entre 5 y 12 dígitos.");

            return new Cedula(limpia);
        }

        public override string ToString() => Valor;
        public override bool Equals(object? obj) => obj is Cedula c && c.Valor == Valor;
        public override int GetHashCode() => Valor.GetHashCode();
    }
}
