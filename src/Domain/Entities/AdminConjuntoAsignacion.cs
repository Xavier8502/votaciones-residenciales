
using System;
using VotacionesResidenciales.Domain.Common;

namespace VotacionesResidenciales.Domain.Entities
{
    public class AdminConjuntoAsignacion : BaseEntity
    {
        public Guid AdminConjuntoId { get; private set; }
        public Guid ConjuntoId { get; private set; }

        public AdminConjunto? AdminConjunto { get; private set; }
        public Conjunto? Conjunto { get; private set; }

        private AdminConjuntoAsignacion() { }

        private AdminConjuntoAsignacion(
            Guid adminConjuntoId,
            Guid conjuntoId)
        {
            if (adminConjuntoId == Guid.Empty)
                throw new ArgumentException(
                    "El administrador es obligatorio.",
                    nameof(adminConjuntoId));

            if (conjuntoId == Guid.Empty)
                throw new ArgumentException(
                    "El conjunto es obligatorio.",
                    nameof(conjuntoId));

            AdminConjuntoId = adminConjuntoId;
            ConjuntoId = conjuntoId;
        }

        public static AdminConjuntoAsignacion Crear(
            Guid adminConjuntoId,
            Guid conjuntoId)
        {
            return new AdminConjuntoAsignacion(
                adminConjuntoId,
                conjuntoId);
        }
    }
}
