using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace VotacionesResidenciales.Infrastructure.Persistence;

public static class AppDbContextOptionsConfigurator
{
    public static DatabaseSettings GetSettings(IConfiguration configuration)
    {
        var databaseSection = configuration.GetSection(
            DatabaseSettings.SectionName);

        var settings = databaseSection.Get<DatabaseSettings>()
            ?? new DatabaseSettings();

        var normalizedProvider = DatabaseProviders.Normalize(settings.Provider);
        var explicitConnectionStringName =
            databaseSection["ConnectionStringName"];
        var resolvedConnectionStringName =
            string.IsNullOrWhiteSpace(explicitConnectionStringName)
                ? normalizedProvider == DatabaseProviders.PostgreSql
                    ? "PostgreSqlConnection"
                    : "DefaultConnection"
                : settings.ConnectionStringName;

        return new DatabaseSettings
        {
            Provider = normalizedProvider,
            ConnectionStringName = resolvedConnectionStringName,
            MigrationsAssemblyName = string.IsNullOrWhiteSpace(
                settings.MigrationsAssemblyName)
                ? DatabaseProviders.GetDefaultMigrationsAssembly(
                    normalizedProvider)
                : settings.MigrationsAssemblyName,
            ApplyMigrationsOnStartup = settings.ApplyMigrationsOnStartup
        };
    }

    public static void Configure(
        DbContextOptionsBuilder options,
        IConfiguration configuration)
    {
        var settings = GetSettings(configuration);
        var connectionString = configuration.GetConnectionString(
            settings.ConnectionStringName);

        if (string.IsNullOrWhiteSpace(connectionString))
        {
            throw new InvalidOperationException(
                $"No se encontro la cadena de conexion '{settings.ConnectionStringName}'.");
        }

        Configure(
            options,
            settings.Provider,
            connectionString,
            settings.MigrationsAssemblyName);
    }

    public static void Configure(
        DbContextOptionsBuilder options,
        string provider,
        string connectionString,
        string? migrationsAssembly = null)
    {
        migrationsAssembly ??= typeof(AppDbContext).Assembly.FullName;

        switch (DatabaseProviders.Normalize(provider))
        {
            case DatabaseProviders.SqlServer:
                options.UseSqlServer(
                    connectionString,
                    sqlServer => sqlServer
                        .MigrationsAssembly(migrationsAssembly)
                        .EnableRetryOnFailure());
                break;

            case DatabaseProviders.PostgreSql:
                options.UseNpgsql(
                    connectionString,
                    npgsql => npgsql
                        .MigrationsAssembly(migrationsAssembly)
                        .EnableRetryOnFailure());
                break;
        }
    }
}
