namespace VotacionesResidenciales.Application.Features.AdminDashboard.DTOs;

public record DashboardConjuntoDetalleDto(
    Guid Id,
    string Nombre,
    string Nit,
    string Direccion,
    string Ciudad,
    string? Telefono,
    string? Email,
    int TotalInmuebles,
    decimal TotalCoeficientes,
    DateTime CreadoEn,
    bool Activo
);

public record DashboardConjuntoDto(
    DashboardConjuntoDetalleDto Conjunto,
    int TotalInmuebles,
    int TotalResidentes,
    int TotalVotaciones
);