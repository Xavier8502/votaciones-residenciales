using Microsoft.EntityFrameworkCore;
using VotacionesResidenciales.Domain.Common;
using VotacionesResidenciales.Domain.Entities;
using VotacionesResidenciales.Domain.Enums;
using VotacionesResidenciales.Infrastructure.Persistence;
using VotacionesResidenciales.Infrastructure.Persistence.Repositories;

namespace VotacionesResidenciales.Infrastructure.Tests.Persistence.Repositories;

public class DashboardReadRepositoryTests
{
    [Fact]
    public async Task ObtenerPorAdministradorAsync_DebeRetornarSoloConjuntosActivosAsignados()
    {
        // Arrange
        await using var context = CreateContext();

        var conjuntoAsignado = CrearConjunto("Conjunto Asignado", "900001");
        var conjuntoNoAsignado = CrearConjunto("Conjunto No Asignado", "900002");
        var conjuntoInactivo = CrearConjunto("Conjunto Inactivo", "900003");

        var admin = CrearAdministrador("9000000001");
        admin.AgregarAsignacion(conjuntoAsignado);
        admin.AgregarAsignacion(conjuntoInactivo);

        var otroAdmin = CrearAdministrador("9000000002");
        otroAdmin.AgregarAsignacion(conjuntoNoAsignado);

        context.AddRange(
            conjuntoAsignado,
            conjuntoNoAsignado,
            conjuntoInactivo,
            admin,
            otroAdmin);

        context.Entry(conjuntoInactivo)
            .Property(x => x.Activo).CurrentValue = false;

        await context.SaveChangesAsync();

        var repository = new DashboardReadRepository(context);

        // Act
        var resultado = await repository.ObtenerPorAdministradorAsync(admin.Id);

        // Assert
        var conjunto = Assert.Single(resultado);

        Assert.Equal(conjuntoAsignado.Id, conjunto.Conjunto.Id);
        Assert.Equal("Conjunto Asignado", conjunto.Conjunto.Nombre);
    }

    [Fact]
    public async Task ObtenerPorAdministradorAsync_DebeCalcularIndicadoresSoloConRegistrosActivos()
    {
        // Arrange
        await using var context = CreateContext();

        var conjunto = CrearConjunto("Conjunto Indicadores", "900004");
        var admin = CrearAdministrador("9000000003");

        admin.AgregarAsignacion(conjunto);

        var inmuebleActivo = Inmueble.Crear(
            conjunto.Id,
            "101",
            TipoInmueble.Apartamento,
            30.5m);

        var inmuebleInactivo = Inmueble.Crear(
            conjunto.Id,
            "102",
            TipoInmueble.Apartamento,
            99m);

        var residenteActivo1 = Residente.Crear(
            inmuebleActivo.Id,
            "1000000001",
            "Residente",
            "Uno",
            "hash");

        var residenteActivo2 = Residente.Crear(
            inmuebleActivo.Id,
            "1000000002",
            "Residente",
            "Dos",
            "hash");

        var residenteInactivo = Residente.Crear(
            inmuebleActivo.Id,
            "1000000003",
            "Residente",
            "Tres",
            "hash");

        var votacionActiva = CrearVotacion(conjunto.Id, "Votación activa");
        var votacionInactiva = CrearVotacion(conjunto.Id, "Votación inactiva");

        context.AddRange(
            conjunto,
            admin,
            inmuebleActivo,
            inmuebleInactivo,
            residenteActivo1,
            residenteActivo2,
            residenteInactivo,
            votacionActiva,
            votacionInactiva);

        context.Entry(inmuebleInactivo)
            .Property(x => x.Activo).CurrentValue = false;

        context.Entry(residenteInactivo)
            .Property(x => x.Activo).CurrentValue = false;

        context.Entry(votacionInactiva)
            .Property(x => x.Activo).CurrentValue = false;

        await context.SaveChangesAsync();

        var repository = new DashboardReadRepository(context);

        // Act
        var resultado = await repository.ObtenerPorAdministradorAsync(admin.Id);

        // Assert
        var dashboard = Assert.Single(resultado);

        Assert.Equal(1, dashboard.TotalInmuebles);
        Assert.Equal(2, dashboard.TotalResidentes);
        Assert.Equal(1, dashboard.TotalVotaciones);

        Assert.Equal(1, dashboard.Conjunto.TotalInmuebles);
        Assert.Equal(30.5m, dashboard.Conjunto.TotalCoeficientes);
    }

    [Fact]
    public async Task ObtenerPorAdministradorAsync_SinAsignaciones_DebeRetornarListaVacia()
    {
        // Arrange
        await using var context = CreateContext();

        var conjunto = CrearConjunto("Conjunto de otro administrador", "900005");

        var adminAsignado = CrearAdministrador("9000000004");
        adminAsignado.AgregarAsignacion(conjunto);

        var adminSinAsignaciones = CrearAdministrador("9000000005");

        context.AddRange(conjunto, adminAsignado, adminSinAsignaciones);
        await context.SaveChangesAsync();

        var repository = new DashboardReadRepository(context);

        // Act
        var resultado = await repository.ObtenerPorAdministradorAsync(
            adminSinAsignaciones.Id);

        // Assert
        Assert.Empty(resultado);
    }

    [Fact]
    public async Task ObtenerPorAdministradorAsync_AdministradorInactivo_DebeRetornarListaVacia()
    {
        // Arrange
        await using var context = CreateContext();

        var conjunto = CrearConjunto("Conjunto de administrador inactivo", "900006");
        var admin = CrearAdministrador("9000000006");

        admin.AgregarAsignacion(conjunto);

        context.AddRange(conjunto, admin);

        context.Entry(admin)
            .Property(x => x.Activo).CurrentValue = false;

        await context.SaveChangesAsync();

        var repository = new DashboardReadRepository(context);

        // Act
        var resultado = await repository.ObtenerPorAdministradorAsync(admin.Id);

        // Assert
        Assert.Empty(resultado);
    }

    [Fact]
    public async Task ObtenerPorAdministradorAsync_AsignacionInactiva_DebeExcluirConjunto()
    {
        await using var context = CreateContext();

        var conjunto = CrearConjunto("Conjunto Asignacion Inactiva", "900007");
        var admin = CrearAdministrador("9000000007");

        admin.AgregarAsignacion(conjunto);

        var asignacion = admin.Asignaciones.Single();

        context.AddRange(conjunto, admin);

        context.Entry(asignacion)
            .Property(x => x.Activo).CurrentValue = false;

        await context.SaveChangesAsync();

        var repository = new DashboardReadRepository(context);

        var resultado = await repository.ObtenerPorAdministradorAsync(admin.Id);

        Assert.Empty(resultado);
    }
    private static AppDbContext CreateContext()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase($"DashboardTests_{Guid.NewGuid()}")
            .Options;

        return new AppDbContext(options);
    }

    private static Conjunto CrearConjunto(string nombre, string nit)
    {
        return Conjunto.Crear(
            nombre,
            nit,
            "Dirección de prueba",
            "Bogotá");
    }

    private static AdminConjunto CrearAdministrador(string cedula)
    {
        return AdminConjunto.Crear(
            cedula,
            "Administrador",
            "Prueba",
            "hash");
    }

    private static Votacion CrearVotacion(Guid conjuntoId, string titulo)
    {
        var fechaInicio = DateTime.UtcNow;

        return Votacion.Crear(
            conjuntoId,
            titulo,
            null,
            TipoPesoVoto.Igualitario,
            50m,
            fechaInicio,
            fechaInicio.AddDays(7));
    }
}