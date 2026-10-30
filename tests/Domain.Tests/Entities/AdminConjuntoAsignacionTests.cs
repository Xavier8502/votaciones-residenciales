using FluentAssertions;
using VotacionesResidenciales.Domain.Entities;

namespace VotacionesResidenciales.Domain.Tests.Entities;

public class AdminConjuntoAsignacionTests
{
    [Fact]
    public void Crear_IdsValidos_DebeCrearAsignacion()
    {
        var adminId = Guid.NewGuid();
        var conjuntoId = Guid.NewGuid();

        var asignacion = AdminConjuntoAsignacion.Crear(
            adminId,
            conjuntoId);

        asignacion.AdminConjuntoId.Should().Be(adminId);
        asignacion.ConjuntoId.Should().Be(conjuntoId);
        asignacion.Id.Should().NotBeEmpty();
    }

    [Theory]
    [InlineData(true, false)]
    [InlineData(false, true)]
    [InlineData(true, true)]
    public void Crear_IdVacio_DebeLanzarExcepcion(
        bool adminVacio,
        bool conjuntoVacio)
    {
        var adminId = adminVacio ? Guid.Empty : Guid.NewGuid();
        var conjuntoId = conjuntoVacio ? Guid.Empty : Guid.NewGuid();

        var accion = () => AdminConjuntoAsignacion.Crear(
            adminId,
            conjuntoId);

        accion.Should().Throw<ArgumentException>();
    }
}
