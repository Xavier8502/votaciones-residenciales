using NSubstitute;
using VotacionesResidenciales.Application.Features.AdminConjuntos.Commands.AsignarConjunto;
using VotacionesResidenciales.Domain.Entities;
using VotacionesResidenciales.Domain.Interfaces;
using VotacionesResidenciales.Domain.Interfaces.Repositories;

namespace VotacionesResidenciales.Application.Tests.Features.AdminConjuntos;

public class AsignarConjuntoCommandHandlerTests
{
    private readonly IAdminConjuntoRepository _adminRepo =
        Substitute.For<IAdminConjuntoRepository>();

    private readonly IUnitOfWork _uow =
        Substitute.For<IUnitOfWork>();

    private AsignarConjuntoCommandHandler CrearHandler() =>
        new(_adminRepo, _uow);

    private static AdminConjunto CrearAdmin() =>
        AdminConjunto.Crear("12345678", "Carlos", "Pérez", "pin-hash");

    private static Conjunto CrearConjunto() =>
        Conjunto.Crear(
            "El Prado",
            "900123456-7",
            "Calle 123",
            "Bogotá");

    [Fact]
    public async Task Handle_AdministradorYConjuntoValidos_DebeAsignarYGuardar()
    {
        var admin = CrearAdmin();
        var conjunto = CrearConjunto();

        _adminRepo.ObtenerPorIdAsync(
            admin.Id, Arg.Any<CancellationToken>())
            .Returns(admin);

        _adminRepo.ObtenerConjuntoActivoAsync(
            conjunto.Id, Arg.Any<CancellationToken>())
            .Returns(conjunto);

        _adminRepo.ExisteAsignacionAsync(
            admin.Id,
            conjunto.Id,
            Arg.Any<CancellationToken>())
            .Returns(false);

        _uow.GuardarCambiosAsync(Arg.Any<CancellationToken>())
            .Returns(1);

        var command = new AsignarConjuntoCommand(
            admin.Id,
            conjunto.Id);

        var result = await CrearHandler().Handle(
            command, CancellationToken.None);

        Assert.True(result.Exitoso);
        Assert.True(result.Datos);

        await _adminRepo.Received(1).AgregarAsignacionAsync(
            Arg.Is<AdminConjuntoAsignacion>(a =>
                a.AdminConjuntoId == admin.Id &&
                a.ConjuntoId == conjunto.Id),
            Arg.Any<CancellationToken>());

        await _uow.Received(1).GuardarCambiosAsync(
            Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_AdministradorInexistente_DebeFallarSinGuardar()
    {
        var adminId = Guid.NewGuid();
        var conjuntoId = Guid.NewGuid();

        _adminRepo.ObtenerPorIdAsync(
            adminId, Arg.Any<CancellationToken>())
            .Returns((AdminConjunto?)null);

        var command = new AsignarConjuntoCommand(adminId, conjuntoId);

        var result = await CrearHandler().Handle(
            command, CancellationToken.None);

        Assert.False(result.Exitoso);
        Assert.Equal("El administrador no existe.", result.Error);

        await _adminRepo.DidNotReceive().AgregarAsignacionAsync(
            Arg.Any<AdminConjuntoAsignacion>(),
            Arg.Any<CancellationToken>());

        await _uow.DidNotReceive().GuardarCambiosAsync(
            Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_ConjuntoInexistenteOInactivo_DebeFallarSinGuardar()
    {
        var admin = CrearAdmin();
        var conjuntoId = Guid.NewGuid();

        _adminRepo.ObtenerPorIdAsync(
            admin.Id, Arg.Any<CancellationToken>())
            .Returns(admin);

        _adminRepo.ObtenerConjuntoActivoAsync(
            conjuntoId, Arg.Any<CancellationToken>())
            .Returns((Conjunto?)null);

        var command = new AsignarConjuntoCommand(
            admin.Id,
            conjuntoId);

        var result = await CrearHandler().Handle(
            command, CancellationToken.None);

        Assert.False(result.Exitoso);
        Assert.Equal(
            "El conjunto no existe o está inactivo.",
            result.Error);

        await _adminRepo.DidNotReceive().AgregarAsignacionAsync(
            Arg.Any<AdminConjuntoAsignacion>(),
            Arg.Any<CancellationToken>());

        await _uow.DidNotReceive().GuardarCambiosAsync(
            Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_AsignacionDuplicada_DebeFallarSinGuardar()
    {
        var admin = CrearAdmin();
        var conjunto = CrearConjunto();

        _adminRepo.ObtenerPorIdAsync(
            admin.Id, Arg.Any<CancellationToken>())
            .Returns(admin);

        _adminRepo.ObtenerConjuntoActivoAsync(
            conjunto.Id, Arg.Any<CancellationToken>())
            .Returns(conjunto);

        _adminRepo.ExisteAsignacionAsync(
            admin.Id,
            conjunto.Id,
            Arg.Any<CancellationToken>())
            .Returns(true);

        var command = new AsignarConjuntoCommand(
            admin.Id,
            conjunto.Id);

        var result = await CrearHandler().Handle(
            command, CancellationToken.None);

        Assert.False(result.Exitoso);
        Assert.Equal(
            "El administrador ya tiene una asignación para ese conjunto.",
            result.Error);

        await _adminRepo.DidNotReceive().AgregarAsignacionAsync(
            Arg.Any<AdminConjuntoAsignacion>(),
            Arg.Any<CancellationToken>());

        await _uow.DidNotReceive().GuardarCambiosAsync(
            Arg.Any<CancellationToken>());
    }
}
