using NSubstitute;
using VotacionesResidenciales.Application.Common.Interfaces;
using VotacionesResidenciales.Application.Features.AdminDashboard.DTOs;
using VotacionesResidenciales.Application.Features.AdminDashboard.Queries.ObtenerDashboard;

namespace VotacionesResidenciales.Application.Tests.Features.AdminDashboard;

public class ObtenerDashboardQueryHandlerTests
{
    private readonly IDashboardReadRepository _repository;
    private readonly ObtenerDashboardQueryHandler _handler;

    public ObtenerDashboardQueryHandlerTests()
    {
        _repository = Substitute.For<IDashboardReadRepository>();
        _handler = new ObtenerDashboardQueryHandler(_repository);
    }

    [Fact]
    public async Task Handle_AdminIdVacio_DebeRetornarFallo()
    {
        // Arrange
        var query = new ObtenerDashboardQuery(Guid.Empty);

        // Act
        var resultado = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.False(resultado.Exitoso);
        Assert.Equal(
            "No fue posible identificar al administrador autenticado.",
            resultado.Error);

        await _repository.DidNotReceive()
            .ObtenerPorAdministradorAsync(
                Arg.Any<Guid>(),
                Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_AdministradorValido_DebeRetornarDashboard()
    {
        // Arrange
        var adminId = Guid.NewGuid();

        var dashboard = new List<DashboardConjuntoDto>
        {
            new(
                new DashboardConjuntoDetalleDto(
                    Guid.NewGuid(),
                    "Conjunto El Prado",
                    "900123456-7",
                    "Calle 123",
                    "Bogotá",
                    "3001234567",
                    "admin@elprado.com",
                    16,
                    50.5m,
                    DateTime.UtcNow,
                    true),
                16,
                12,
                1)
        };

        _repository
            .ObtenerPorAdministradorAsync(adminId, Arg.Any<CancellationToken>())
            .Returns(dashboard);

        var query = new ObtenerDashboardQuery(adminId);

        // Act
        var resultado = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.True(resultado.Exitoso);
        Assert.NotNull(resultado.Datos);

        var conjunto = Assert.Single(resultado.Datos);

        Assert.Equal("Conjunto El Prado", conjunto.Conjunto.Nombre);
        Assert.Equal(16, conjunto.TotalInmuebles);
        Assert.Equal(12, conjunto.TotalResidentes);
        Assert.Equal(1, conjunto.TotalVotaciones);

        await _repository.Received(1)
            .ObtenerPorAdministradorAsync(adminId, Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_SinConjuntos_DebeRetornarListaVaciaExitosa()
    {
        // Arrange
        var adminId = Guid.NewGuid();

        _repository
            .ObtenerPorAdministradorAsync(adminId, Arg.Any<CancellationToken>())
            .Returns(new List<DashboardConjuntoDto>());

        var query = new ObtenerDashboardQuery(adminId);

        // Act
        var resultado = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.True(resultado.Exitoso);
        Assert.NotNull(resultado.Datos);
        Assert.Empty(resultado.Datos);
    }
}