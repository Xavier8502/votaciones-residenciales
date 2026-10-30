using Microsoft.EntityFrameworkCore;
using VotacionesResidenciales.Application.Common.Interfaces;
using VotacionesResidenciales.Application.Features.AdminDashboard.DTOs;
using VotacionesResidenciales.Infrastructure.Persistence;

namespace VotacionesResidenciales.Infrastructure.Persistence.Repositories;

public class DashboardReadRepository : IDashboardReadRepository
{
    private readonly AppDbContext _context;

    public DashboardReadRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IReadOnlyList<DashboardConjuntoDto>>
        ObtenerPorAdministradorAsync(
            Guid adminId,
            CancellationToken ct = default)
    {
        var query = _context.Conjuntos
            .AsNoTracking()
            .Where(conjunto =>
                conjunto.Activo &&
                _context.AdminConjuntoAsignaciones.Any(asignacion =>
                    asignacion.AdminConjuntoId == adminId &&
                    asignacion.ConjuntoId == conjunto.Id &&
                    asignacion.Activo &&
                    _context.Ad­minesConjunto.Any(admin =>
                        admin.Id == adminId && admin.Activo)));

        return await query
            .OrderBy(conjunto => conjunto.Nombre)
            .Select(conjunto => new DashboardConjuntoDto(
                new DashboardConjuntoDetalleDto(
                    conjunto.Id,
                    conjunto.Nombre,
                    conjunto.Nit,
                    conjunto.Direccion,
                    conjunto.Ciudad,
                    conjunto.Telefono,
                    conjunto.Email,

                    _context.Inmuebles.Count(inmueble =>
                        inmueble.ConjuntoId == conjunto.Id &&
                        inmueble.Activo),

                    _context.Inmuebles
                        .Where(inmueble =>
                            inmueble.ConjuntoId == conjunto.Id &&
                            inmueble.Activo)
                        .Sum(inmueble =>
                            (decimal?)inmueble.Coeficiente) ?? 0m,

                    conjunto.CreadoEn,
                    conjunto.Activo
                ),

                _context.Inmuebles.Count(inmueble =>
                    inmueble.ConjuntoId == conjunto.Id &&
                    inmueble.Activo),

                _context.Residentes.Count(residente =>
                    residente.Activo &&
                    residente.Inmueble != null &&
                    residente.Inmueble.ConjuntoId == conjunto.Id),

                _context.Votaciones.Count(votacion =>
                    votacion.ConjuntoId == conjunto.Id &&
                    votacion.Activo)
            ))
            .ToListAsync(ct);
    }
}