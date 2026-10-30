using NSubstitute;
using VotacionesResidenciales.Application.Common.Exceptions;
using VotacionesResidenciales.Application.Features.Inmuebles.Commands.ActualizarInmueble;
using VotacionesResidenciales.Domain.Entities;
using VotacionesResidenciales.Domain.Enums;
using VotacionesResidenciales.Domain.Interfaces;
using VotacionesResidenciales.Domain.Interfaces.Repositories;

namespace VotacionesResidenciales.Application.Tests.Features.Inmuebles;

public class ActualizarInmuebleCommandHandlerTests
{
    private readonly IInmuebleRepository _inmuebleRepo =
        Substitute.For<IInmuebleRepository>();

    private readonly IUnitOfWork _uow =
        Substitute.For<IUnitOfWork>();

    private ActualizarInmuebleCommandHandler CrearHandler() =>
        new(_inmuebleRepo, _uow);

    [Fact]
    public async Task Handle_InmuebleExistente_DebeActualizarYGuardar()
    {
        var conjuntoId = Guid.NewGuid();

        var inmueble = Inmueble.Crear(
            conjuntoId,
            "101",
            TipoInmueble.Apartamento,
            3.5m,
            "A",
            "1");

        _inmuebleRepo.ObtenerPorIdAsync(
            inmueble.Id, Arg.Any<CancellationToken>())
            .Returns(inmueble);

        _uow.GuardarCambiosAsync(Arg.Any<CancellationToken>())
            .Returns(1);

        var command = new ActualizarInmuebleCommand(
            inmueble.Id,
            4.25m,
            TipoInmueble.Local,
            "B",
            "2");

        var result = await CrearHandler().Handle(
            command, CancellationToken.None);

        Assert.True(result.Exitoso);
        Assert.Equal(conjuntoId, inmueble.ConjuntoId);
        Assert.Equal(4.25m, inmueble.Coeficiente);
        Assert.Equal(TipoInmueble.Local, inmueble.Tipo);
        Assert.Equal("B", inmueble.Torre);
        Assert.Equal("2", inmueble.Piso);

        await _inmuebleRepo.Received(1).ActualizarAsync(
            inmueble, Arg.Any<CancellationToken>());

        await _uow.Received(1).GuardarCambiosAsync(
            Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_InmuebleInexistente_DebeLanzarExcepcionSinGuardar()
    {
        var inmuebleId = Guid.NewGuid();

        _inmuebleRepo.ObtenerPorIdAsync(
            inmuebleId, Arg.Any<CancellationToken>())
            .Returns((Inmueble?)null);

        var command = new ActualizarInmuebleCommand(
            inmuebleId,
            4m,
            TipoInmueble.Apartamento,
            null,
            null);

        await Assert.ThrowsAsync<NotFoundException>(
            () => CrearHandler().Handle(
                command, CancellationToken.None));

        await _inmuebleRepo.DidNotReceive().ActualizarAsync(
            Arg.Any<Inmueble>(),
            Arg.Any<CancellationToken>());

        await _uow.DidNotReceive().GuardarCambiosAsync(
            Arg.Any<CancellationToken>());
    }
}
