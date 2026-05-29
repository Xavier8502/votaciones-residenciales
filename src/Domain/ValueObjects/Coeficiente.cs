using System;
using System.Collections.Generic;
using System.Text;

namespace VotacionesResidenciales.Domain.ValueObjects
{
    public sealed class Coeficiente
    {
        public decimal Valor { get; }

        private Coeficiente(decimal valor) => Valor = valor;

        public static Coeficiente Crear(decimal valor)
        {
            if (valor <= 0)
                throw new ArgumentException("El coeficiente debe ser mayor a cero.");

            if (valor > 100)
                throw new ArgumentException("El coeficiente no puede ser mayor a 100.");

            return new Coeficiente(Math.Round(valor, 4));
        }

        public override string ToString() => Valor.ToString("F4");
        public override bool Equals(object? obj) => obj is Coeficiente c && c.Valor == Valor;
        public override int GetHashCode() => Valor.GetHashCode();
    }
}
