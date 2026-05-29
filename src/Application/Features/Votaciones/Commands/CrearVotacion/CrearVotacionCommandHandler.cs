using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using VotacionesResidenciales.Application.Common.Exceptions;
using VotacionesResidenciales.Application.Common.Models;
using VotacionesResidenciales.Domain.Entities;
using VotacionesResidenciales.Domain.Interfaces;
using VotacionesResidenciales.Domain.Interfaces.Repositories;

namespace VotacionesResidenciales.Application.Features.Votaciones.Commands.CrearVotacion
{
    public class CrearVotacionCommandHandler
    : IRequestHandler<CrearVotacionCommand, Result<Guid>>
    {
        private readonly IVotacionRepository _votacionRepo;
        private readonly IConjuntoRepository _conjuntoRepo;
        private readonly IUnitOfWork _uow;

        public CrearVotacionCommandHandler(
            IVotacionRepository votacionRepo,
            IConjuntoRepository conjuntoRepo,
            IUnitOfWork uow)
        {
            _votacionRepo = votacionRepo;
            _conjuntoRepo = conjuntoRepo;
            _uow = uow;
        }

        public async Task<Result<Guid>> Handle(
            CrearVotacionCommand request, CancellationToken ct)
        {
            var conjunto = await _conjuntoRepo
                .ObtenerPorIdAsync(request.ConjuntoId, ct);

            if (conjunto is null)
                throw new NotFoundException(nameof(Conjunto), request.ConjuntoId);

            var votacion = Votacion.Crear(
                request.ConjuntoId,
                request.Titulo,
                request.Descripcion,
                request.TipoPeso,
                request.QuorumRequerido,
                request.FechaInicio,
                request.FechaFin,
                request.MostrarResultadosParciales);

            foreach (var preguntaDto in request.Preguntas
                .OrderBy(p => p.Orden))
            {
                var pregunta = Pregunta.Crear(
                    votacion.Id,
                    preguntaDto.Texto,
                    preguntaDto.Orden);

                foreach (var opcionDto in preguntaDto.Opciones
                    .OrderBy(o => o.Orden))
                {
                    var opcion = Opcion.Crear(
                        pregunta.Id,
                        opcionDto.Texto,
                        opcionDto.Orden);

                    pregunta.AgregarOpcion(opcion);
                }

                votacion.AgregarPregunta(pregunta);
            }

            await _votacionRepo.AgregarAsync(votacion, ct);
            await _uow.GuardarCambiosAsync(ct);

            return Result<Guid>.Exito(votacion.Id);
        }
    }
}
