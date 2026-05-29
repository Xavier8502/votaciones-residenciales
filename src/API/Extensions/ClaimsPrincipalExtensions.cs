using System.Security.Claims;

namespace VotacionesResidenciales.API.Extensions
{
    public static class ClaimsPrincipalExtensions
    {
        public static Guid GetResidenteId(this ClaimsPrincipal user)
        {
            var value = user.FindFirstValue(ClaimTypes.NameIdentifier);
            return Guid.TryParse(value, out var id) ? id : Guid.Empty;
        }

        public static Guid GetConjuntoId(this ClaimsPrincipal user)
        {
            var value = user.FindFirstValue("ConjuntoId");
            return Guid.TryParse(value, out var id) ? id : Guid.Empty;
        }

        public static string GetRol(this ClaimsPrincipal user)
            => user.FindFirstValue(ClaimTypes.Role) ?? string.Empty;
    }
}
