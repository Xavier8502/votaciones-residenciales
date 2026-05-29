namespace VotacionesResidenciales.Infrastructure.Persistence;

public sealed class DatabaseSettings
{
    public const string SectionName = "Database";

    public string Provider { get; init; } = DatabaseProviders.SqlServer;
    public string ConnectionStringName { get; init; } = "DefaultConnection";
    public string? MigrationsAssemblyName { get; init; }
    public bool ApplyMigrationsOnStartup { get; init; } = true;
}

public static class DatabaseProviders
{
    public const string SqlServer = "SqlServer";
    public const string PostgreSql = "PostgreSql";
    public const string SqlServerMigrationsAssembly =
        "VotacionesResidenciales.Infrastructure";
    public const string PostgreSqlMigrationsAssembly =
        "VotacionesResidenciales.PostgreSqlMigrations";

    public static string Normalize(string? provider) =>
        provider?.Trim().ToLowerInvariant() switch
        {
            "sqlserver" or "mssql" => SqlServer,
            "postgresql" or "postgres" or "npgsql" => PostgreSql,
            _ => throw new InvalidOperationException(
                $"Proveedor de base de datos no soportado: '{provider ?? "<null>"}'. " +
                $"Use '{SqlServer}' o '{PostgreSql}'.")
        };

    public static string GetDefaultMigrationsAssembly(string provider) =>
        Normalize(provider) switch
        {
            SqlServer => SqlServerMigrationsAssembly,
            PostgreSql => PostgreSqlMigrationsAssembly,
            _ => throw new InvalidOperationException(
                $"Proveedor de base de datos no soportado: '{provider}'.")
        };
}
