using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using VotacionesResidenciales.Application.Common.Interfaces;
using VotacionesResidenciales.Domain.Interfaces;
using VotacionesResidenciales.Domain.Interfaces.Repositories;
using VotacionesResidenciales.Infrastructure.Hubs;
using VotacionesResidenciales.Infrastructure.Persistence;
using VotacionesResidenciales.Infrastructure.Persistence.Repositories;
using VotacionesResidenciales.Infrastructure.Services;

namespace VotacionesResidenciales.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            // ── DbContext ──────────────────────────────────────
            services.AddDbContext<AppDbContext>(options =>
                AppDbContextOptionsConfigurator.Configure(
                    options,
                    configuration));

            // ── Repositories ───────────────────────────────────
            services.AddScoped<IConjuntoRepository, ConjuntoRepository>();
            services.AddScoped<IInmuebleRepository, InmuebleRepository>();
            services.AddScoped<IResidenteRepository, ResidenteRepository>();
            services.AddScoped<IVotacionRepository, VotacionRepository>();
            services.AddScoped<IVotoRepository, VotoRepository>();
            services.AddScoped<IAdminConjuntoRepository, AdminConjuntoRepository>();

            // ── UnitOfWork ─────────────────────────────────────
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            // ── Services ───────────────────────────────────────
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IVotacionHubService, VotacionHubService>();

            // ── SignalR ────────────────────────────────────────
            services.AddSignalR();

            // ── JWT Auth ───────────────────────────────────────
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters =
                        new TokenValidationParameters
                        {
                            ValidateIssuer = true,
                            ValidateAudience = true,
                            ValidateLifetime = true,
                            ValidateIssuerSigningKey = true,
                            ValidIssuer = configuration["Jwt:Issuer"],
                            ValidAudience = configuration["Jwt:Audience"],
                            IssuerSigningKey = new SymmetricSecurityKey(
                                Encoding.UTF8.GetBytes(
                                    configuration["Jwt:Key"]!))
                        };

                    // Soporte SignalR con JWT
                    options.Events = new JwtBearerEvents
                    {
                        OnMessageReceived = context =>
                        {
                            var accessToken = context.Request
                                .Query["access_token"];

                            var path = context.HttpContext.Request.Path;

                            if (!string.IsNullOrEmpty(accessToken)
                                && path.StartsWithSegments("/hubs"))
                            {
                                context.Token = accessToken;
                            }

                            return Task.CompletedTask;
                        }
                    };
                });

            return services;
        }
    }
}
