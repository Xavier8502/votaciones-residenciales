using System;
using System.Collections.Generic;
using System.Text;
using VotacionesResidenciales.Domain.Common;
using VotacionesResidenciales.Domain.Enums;

namespace VotacionesResidenciales.Domain.Entities
{
    public class Inmueble : BaseEntity
    {
        public Guid ConjuntoId { get; private set; }
        public string Numero { get; private set; } = string.Empty;
        public TipoInmueble Tipo { get; private set; }
        public decimal Coeficiente { get; private set; }
        public string? Torre { get; private set; }
        public string? Piso { get; private set; }

        public Conjunto? Conjunto { get; private set; }

        private readonly List<Residente> _residentes = [];
        public IReadOnlyCollection<Residente> Residentes => _residentes.AsReadOnly();

        private Inmueble() { }

        public static Inmueble Crear(Guid conjuntoId, string numero,
            TipoInmueble tipo, decimal coeficiente,
            string? torre = null, string? piso = null)
        {
            var coef = ValueObjects.Coeficiente.Crear(coeficiente);

            return new Inmueble
            {
                ConjuntoId = conjuntoId,
                Numero = numero.Trim(),
                Tipo = tipo,
                Coeficiente = coef.Valor,
                Torre = torre?.Trim(),
                Piso = piso?.Trim()
            };
        }

        public void Actualizar(decimal coeficiente, TipoInmueble tipo,
    string? torre, string? piso)
        {
            var coef = ValueObjects.Coeficiente.Crear(coeficiente);
            Coeficiente = coef.Valor;
            Tipo = tipo;
            Torre = torre?.Trim();
            Piso = piso?.Trim();
            MarcarModificado();
        }
    }
}
