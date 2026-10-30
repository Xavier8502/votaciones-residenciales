using FluentAssertions;
using VotacionesResidenciales.Domain.Entities;

namespace VotacionesResidenciales.Domain.Tests.Entities;

public class ConjuntoTests
{
    [Fact]
    public void Crear_DatosValidos_DebeNormalizarCampos()
    {
        var conjunto = Conjunto.Crear(
            " El Prado ",
            " 900123456-7 ",
            " Calle 123 ",
            " Bogotá ");

        conjunto.Nombre.Should().Be("El Prado");
        conjunto.Nit.Should().Be("900123456-7");
        conjunto.Direccion.Should().Be("Calle 123");
        conjunto.Ciudad.Should().Be("Bogotá");
        conjunto.Id.Should().NotBeEmpty();
    }

    [Theory]
    [InlineData("", "900123456-7")]
    [InlineData("El Prado", "")]
    public void Crear_NombreONitVacio_DebeLanzarExcepcion(
        string nombre,
        string nit)
    {
        var accion = () => Conjunto.Crear(
            nombre,
            nit,
            "Calle 123",
            "Bogotá");

        accion.Should().Throw<ArgumentException>();
    }
}
