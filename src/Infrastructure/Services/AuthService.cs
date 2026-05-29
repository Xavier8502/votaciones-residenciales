using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using VotacionesResidenciales.Application.Common.Interfaces;

namespace VotacionesResidenciales.Infrastructure.Services
{
    public class AuthService : IAuthService
    {
        private readonly IConfiguration _config;

        public AuthService(IConfiguration config)
        {
            _config = config;
        }

        public string GenerarToken(Guid residenteId, string cedula,
            string rol, Guid conjuntoId)
        {
            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_config["Jwt:Key"]!));

            var credenciales = new SigningCredentials(
                key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
            new Claim(ClaimTypes.NameIdentifier, residenteId.ToString()),
            new Claim(ClaimTypes.Name, cedula),
            new Claim(ClaimTypes.Role, rol),
            new Claim("ConjuntoId", conjuntoId.ToString())
        };

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(
                    int.Parse(_config["Jwt:ExpiresHours"] ?? "8")),
                signingCredentials: credenciales);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public string HashPin(string pin)
            => BCrypt.Net.BCrypt.HashPassword(pin);

        public bool VerificarPin(string pin, string hash)
            => BCrypt.Net.BCrypt.Verify(pin, hash);
    }
}
