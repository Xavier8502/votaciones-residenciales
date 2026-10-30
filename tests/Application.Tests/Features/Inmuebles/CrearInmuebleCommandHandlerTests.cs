using NSubstitute;
using VotacionesResidenciales.Application.Common.Exceptions;
using VotacionesResidenciales.Application.Features.Inmuebles.Commands.CrearInmueble;
using VotacionesResidenciales.Domain.Entities;
using VotacionesResidenciales.Domain.Enums;
using VotacionesResidenciales.Domain.Interfaces;
using VotacionesResidenciales.Domain.Interfaces.Repositories;

namespace VotacionesResidenciales.Application.Tests.Features.Inmuebles;

public class CrearInmuebleCommandHandlerTests
{
    private readonly IInmuebleRepository _inmuebleRepo =
        Substitute.For<IInmuebleRepository>();

    private readonly IConjuntoRepository _conjuntoRepo =
        Substitute.For<IConjuntoRepository>();

    private readonly IUnitOfWork _uow =
        Substitute.For<IUnitOfWork>();

    private CrearInmuebleCommandHandler CrearHandler() =>
        new(_inmuebleRepo, _conjuntoRepo, _uow);

    [Fact]
    public async Task Handle_ConjuntoExistente_DebeCrearYGuardarInmueble()
    {
        var conjunto = Conjunto.Crear(
            "El Prado", "900123456-7", "Calle 123", "Bogotá");

        var command = new CrearInmuebleCommand(
            conjunto.Id,
            "101",
            TipoInmueble.Apartamento,
            3.5m,
            "A",
            "1");

        _conjuntoRepo.ObtenerPorIdAsync(
            conjunto.Id, Arg.Any<CancellationToken>())
            .Returns(conjunto);

        _inmuebleRepo.ExisteNumeroEnConjuntoAsync(
            conjunto.Id, "101", Arg.Any<CancellationToken>())
            .Returns(false);

        _uow.GuardarCambiosAsync(Arg.Any<CancellationToken>())
            .Returns(1);

        var result = await CrearHandler().Handle(
            command, CancellationToken.None);

        Assert.True(result.Exitoso);
        Assert.NotEqual(Guid.Empty, result.Datos);

        await _inmuebleRepo.Received(1).AgregarAsync(
            Arg.Is<Inmueble>(i =>
                i.ConjuntoId == conjunto.Id &&
                i.Numero == "101" &&
                i.Coeficiente == 3.5m),
            Arg.Any<CancellationToken>());

        await _uow.Received(1).GuardarCambiosAsync(
            Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_ConjuntoInexistente_DebeLanzarNotFoundException()
    {
        var conjuntoId = Guid.NewGuid();

        _conjuntoRepo.ObtenerPorIdAsync(
            conjuntoId, Arg.Any<CancellationToken>())
            .Returns((Conjunto?)null);

        var command = new CrearInmuebleCommand(
            conjuntoId,
            "101",
            TipoInmueble.Apartamento,
            3.5m,
            null,
            null);

        await Assert.ThrowsAsync<NotFoundException>(
            () => CrearHandler().Handle(
                command, CancellationToken.None));

        await _inmuebleRepo.DidNotReceive().AgregarAsync(
            Arg.Any<Inmueble>(),
            Arg.Any<CancellationToken>());

        await _uow.DidNotReceive().GuardarCambiosAsync(
            Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_NumeroDuplicado_DebeFallarSinGuardar()
    {
        var conjunto = Conjunto.Crear(
            "El Prado", "900123456-7", "Calle 123", "Bogotá");

        _conjuntoRepo.ObtenerPorIdAsync(
            conjunto.Id, Arg.Any<CancellationToken>())
            .Returns(conjunto);

        _inmuebleRepo.ExisteNumeroEnConjuntoAsync(
            conjunto.Id, "101", Arg.Any<CancellationToken>())
            .Returns(true);

        var command = new CrearInmuebleCommand(
            conjunto.Id,
            "101",
            TipoInmueble.Apartamento,
            3.5m,
            "A",
            "1");

        var result = await CrearHandler().Handle(
            command, CancellationToken.None);

        Assert.False(result.Exitoso);
        Assert.Contains("Ya existe el inmueble", result.Error);

        await _inmuebleRepo.DidNotReceive().AgregarAsync(
            Arg.Any<Inmueble>(),
            Arg.Any<CancellationToken>());

        await _uow.DidNotReceive().GuardarCambiosAsync(
            Arg.Any<CancellationToken>());
    }
}
