// src/Infrastructure/Persistence/AppDbContextSeed.cs
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using VotacionesResidenciales.Domain.Entities;
using VotacionesResidenciales.Domain.Enums;

namespace VotacionesResidenciales.Infrastructure.Persistence;

public static class AppDbContextSeed
{
    public static async Task SeedAsync(
        AppDbContext context,
        ILogger logger)
    {
        try
        {
            logger.LogInformation("Iniciando seed de datos...");

            // ── Conjunto ───────────────────────────────────
            Conjunto conjunto;

            if (!await context.Conjuntos.AnyAsync())
            {
                conjunto = Conjunto.Crear(
                    "Conjunto Residencial El Prado",
                    "900123456-7",
                    "Calle 123 # 45-67",
                    "Bogotá");

                conjunto.ActualizarContacto(
                    "3001234567",
                    "admin@elprado.com");

                await context.Conjuntos.AddAsync(conjunto);
                await context.SaveChangesAsync();

                logger.LogInformation("✅ Conjunto creado: {Nombre}",
                    conjunto.Nombre);
            }
            else
            {
                conjunto = await context.Conjuntos.FirstAsync();
                logger.LogInformation(
                    "⏭️ Conjunto ya existe: {Nombre}",
                    conjunto.Nombre);
            }

            // ── Inmuebles ──────────────────────────────────
            if (!await context.Inmuebles.AnyAsync())
            {
                var inmuebles = new List<Inmueble>
                {
                    Inmueble.Crear(conjunto.Id, "101", TipoInmueble.Apartamento, 3.5m, "A", "1"),
                    Inmueble.Crear(conjunto.Id, "102", TipoInmueble.Apartamento, 3.5m, "A", "1"),
                    Inmueble.Crear(conjunto.Id, "103", TipoInmueble.Apartamento, 3.2m, "A", "1"),
                    Inmueble.Crear(conjunto.Id, "201", TipoInmueble.Apartamento, 4.0m, "A", "2"),
                    Inmueble.Crear(conjunto.Id, "202", TipoInmueble.Apartamento, 4.0m, "A", "2"),
                    Inmueble.Crear(conjunto.Id, "203", TipoInmueble.Apartamento, 3.8m, "A", "2"),
                    Inmueble.Crear(conjunto.Id, "301", TipoInmueble.Apartamento, 4.5m, "A", "3"),
                    Inmueble.Crear(conjunto.Id, "302", TipoInmueble.Apartamento, 4.5m, "A", "3"),
                    Inmueble.Crear(conjunto.Id, "101", TipoInmueble.Apartamento, 3.5m, "B", "1"),
                    Inmueble.Crear(conjunto.Id, "102", TipoInmueble.Apartamento, 3.5m, "B", "1"),
                    Inmueble.Crear(conjunto.Id, "201", TipoInmueble.Apartamento, 4.0m, "B", "2"),
                    Inmueble.Crear(conjunto.Id, "202", TipoInmueble.Apartamento, 4.0m, "B", "2"),
                    Inmueble.Crear(conjunto.Id, "P01", TipoInmueble.Parqueadero, 1.0m),
                    Inmueble.Crear(conjunto.Id, "P02", TipoInmueble.Parqueadero, 1.0m),
                    Inmueble.Crear(conjunto.Id, "D01", TipoInmueble.Deposito, 0.5m),
                    Inmueble.Crear(conjunto.Id, "L01", TipoInmueble.Local, 2.0m),
                };

                await context.Inmuebles.AddRangeAsync(inmuebles);
                await context.SaveChangesAsync();

                logger.LogInformation("✅ Inmuebles creados: {Total}",
                    inmuebles.Count);
            }
            else
            {
                logger.LogInformation("⏭️ Inmuebles ya existen.");
            }

            // ── Admin del Conjunto ─────────────────────────────────
            if (!await context.AdminesConjunto.AnyAsync())
            {
                var pinHash = BCrypt.Net.BCrypt.HashPassword("1234");

                var admin = AdminConjunto.Crear(
                    conjunto.Id,
                    "9000000001",       // cédula diferente a los residentes
                    "Carlos",
                    "Administrador",
                    pinHash);

                admin.ActualizarContacto(
                    "3001111111",
                    "admin@elprado.com");

                await context.AdminesConjunto.AddAsync(admin);
                await context.SaveChangesAsync();

                logger.LogInformation(
                    "✅ Admin creado. Cédula: 9000000001 PIN: 1234");
            }
            else
            {
                logger.LogInformation("⏭️ Admin ya existe.");
            }

            // ── Residentes ─────────────────────────────────────────
            if (!await context.Residentes.AnyAsync())
            {
                var inmuebles = await context.Inmuebles
                    .Where(i => i.ConjuntoId == conjunto.Id)
                    .OrderBy(i => i.Torre)
                    .ThenBy(i => i.Numero)
                    .ToListAsync();

                var pinHash = BCrypt.Net.BCrypt.HashPassword("1234");

                var residentes = new List<Residente>
                    {
                        Residente.Crear(inmuebles[0].Id, "1000000001",
                            "María", "González", pinHash, true),
                        Residente.Crear(inmuebles[1].Id, "1000000002",
                            "Juan", "Pérez", pinHash, true),
                        Residente.Crear(inmuebles[2].Id, "1000000003",
                            "Ana", "Martínez", pinHash, true),
                        Residente.Crear(inmuebles[3].Id, "1000000004",
                            "Luis", "Rodríguez", pinHash, true),
                        Residente.Crear(inmuebles[4].Id, "1000000005",
                            "Sandra", "López", pinHash, true),
                        Residente.Crear(inmuebles[5].Id, "1000000006",
                            "Pedro", "García", pinHash, true),
                        Residente.Crear(inmuebles[6].Id, "1000000007",
                            "Laura", "Hernández", pinHash, true),
                        Residente.Crear(inmuebles[7].Id, "1000000008",
                            "Andrés", "Torres", pinHash, true),
                        Residente.Crear(inmuebles[8].Id, "1000000009",
                            "Diana", "Vargas", pinHash, true),
                        Residente.Crear(inmuebles[9].Id, "1000000010",
                            "Jorge", "Morales", pinHash, true),
                        Residente.Crear(inmuebles[10].Id, "1000000011",
                            "Patricia", "Jiménez", pinHash, true),
                        Residente.Crear(inmuebles[11].Id, "1000000012",
                            "Roberto", "Castillo", pinHash, true),
                    };

                await context.Residentes.AddRangeAsync(residentes);
                await context.SaveChangesAsync();

                logger.LogInformation("✅ Residentes creados: {Total}",
                    residentes.Count);
            }

            // ── Votación de ejemplo ────────────────────────
            if (!await context.Votaciones.AnyAsync())
            {
                var votacion = Votacion.Crear(
                    conjunto.Id,
                    "Elección del Administrador 2025",
                    "Votación para elegir el administrador del conjunto " +
                    "para el período 2025-2026.",
                    TipoPesoVoto.Igualitario,
                    51,
                    DateTime.UtcNow.AddMinutes(5),
                    DateTime.UtcNow.AddDays(3),
                    true);

                var pregunta1 = Pregunta.Crear(
                    votacion.Id,
                    "¿Aprueba usted la gestión del " +
                    "administrador actual?",
                    1);

                pregunta1.AgregarOpcion(
                    Opcion.Crear(pregunta1.Id, "Sí, apruebo", 1));
                pregunta1.AgregarOpcion(
                    Opcion.Crear(pregunta1.Id, "No apruebo", 2));
                pregunta1.AgregarOpcion(
                    Opcion.Crear(pregunta1.Id, "Me abstengo", 3));

                var pregunta2 = Pregunta.Crear(
                    votacion.Id,
                    "¿Está de acuerdo con el incremento de la " +
                    "cuota de administración para el 2025?",
                    2);

                pregunta2.AgregarOpcion(
                    Opcion.Crear(pregunta2.Id,
                        "Sí, estoy de acuerdo", 1));
                pregunta2.AgregarOpcion(
                    Opcion.Crear(pregunta2.Id,
                        "No estoy de acuerdo", 2));

                votacion.AgregarPregunta(pregunta1);
                votacion.AgregarPregunta(pregunta2);

                await context.Votaciones.AddAsync(votacion);
                await context.SaveChangesAsync();

                logger.LogInformation(
                    "✅ Votación creada: {Titulo}",
                    votacion.Titulo);
            }
            else
            {
                logger.LogInformation("⏭️ Votaciones ya existen.");
            }

            logger.LogInformation("🎉 Seed completado.");
        }
        catch (Exception ex)
        {
            logger.LogError(ex,
                "❌ Error durante el seed: {Mensaje}",
                ex.Message);
            throw;
        }
    }
}