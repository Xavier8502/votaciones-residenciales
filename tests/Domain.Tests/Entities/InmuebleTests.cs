using FluentAssertions;
using VotacionesResidenciales.Domain.Entities;
using VotacionesResidenciales.Domain.Enums;

namespace VotacionesResidenciales.Domain.Tests.Entities;

public class InmuebleTests
{
    [Fact]
    public void Crear_DatosValidos_DebeConservarLaRelacionConConjunto()
    {
        var conjuntoId = Guid.NewGuid();

        var inmueble = Inmueble.Crear(
            conjuntoId,
            " 101 ",
            TipoInmueble.Apartamento,
            3.56789m,
            " A ",
            " 2 ");

        inmueble.ConjuntoId.Should().Be(conjuntoId);
        inmueble.Numero.Should().Be("101");
        inmueble.Coeficiente.Should().Be(3.5679m);
        inmueble.Torre.Should().Be("A");
        inmueble.Piso.Should().Be("2");
        inmueble.Id.Should().NotBeEmpty();
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(100.01)]
    public void Crear_CoeficienteInvalido_DebeLanzarExcepcion(
        decimal coeficiente)
    {
        var accion = () => Inmueble.Crear(
            Guid.NewGuid(),
            "101",
            TipoInmueble.Apartamento,
            coeficiente);

        accion.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void Actualizar_DatosValidos_DebeActualizarDatosSinCambiarConjunto()
    {
        var conjuntoId = Guid.NewGuid();

        var inmueble = Inmueble.Crear(
            conjuntoId,
            "101",
            TipoInmueble.Apartamento,
            3m,
            "A",
            "1");

        inmueble.Actualizar(
            4.25m,
            TipoInmueble.Local,
            "B",
            "2");

        inmueble.ConjuntoId.Should().Be(conjuntoId);
        inmueble.Coeficiente.Should().Be(4.25m);
        inmueble.Tipo.Should().Be(TipoInmueble.Local);
        inmueble.Torre.Should().Be("B");
        inmueble.Piso.Should().Be("2");
        inmueble.ModificadoEn.Should().NotBeNull();
    }
}
