using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace VotacionesResidenciales.Infrastructure.Persistence;

public sealed class AppDbContextDesignTimeFactory
    : IDesignTimeDbContextFactory<AppDbContext>
{
    public AppDbContext CreateDbContext(string[] args)
    {
        var configuration = BuildConfiguration(args);
        var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();

        AppDbContextOptionsConfigurator.Configure(optionsBuilder, configuration);

        return new AppDbContext(optionsBuilder.Options);
    }

    private static IConfiguration BuildConfiguration(string[] args)
    {
        var environment = ResolveEnvironment(args);
        var basePath = ResolveBasePath();
        var argumentOverrides = ParseArguments(args);

        return new ConfigurationBuilder()
            .SetBasePath(basePath)
            .AddJsonFile("appsettings.json", optional: true)
            .AddJsonFile($"appsettings.{environment}.json", optional: true)
            .AddEnvironmentVariables()
            .AddInMemoryCollection(argumentOverrides)
            .Build();
    }

    private static string ResolveEnvironment(string[] args)
    {
        var environmentArg = args.FirstOrDefault(
            arg => arg.StartsWith("environment=", StringComparison.OrdinalIgnoreCase));

        if (!string.IsNullOrWhiteSpace(environmentArg))
        {
            return environmentArg.Split('=', 2)[1];
        }

        return Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")
            ?? "Development";
    }

    private static Dictionary<string, string?> ParseArguments(string[] args)
    {
        var values = new Dictionary<string, string?>(
            StringComparer.OrdinalIgnoreCase);

        foreach (var arg in args)
        {
            var separatorIndex = arg.IndexOf('=');
            if (separatorIndex <= 0)
            {
                continue;
            }

            var key = arg[..separatorIndex].Trim();
            var value = arg[(separatorIndex + 1)..].Trim();

            if (!string.IsNullOrWhiteSpace(key))
            {
                values[key] = value;
            }
        }

        return values;
    }

    private static string ResolveBasePath()
    {
        var currentDirectory = Directory.GetCurrentDirectory();
        var candidatePaths = new[]
        {
            currentDirectory,
            Path.GetFullPath(Path.Combine(currentDirectory, "..", "API")),
            Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "..", "API"))
        };

        foreach (var candidate in candidatePaths.Distinct())
        {
            if (File.Exists(Path.Combine(candidate, "appsettings.json")))
            {
                return candidate;
            }
        }

        throw new InvalidOperationException(
            "No fue posible ubicar appsettings.json para crear el AppDbContext en tiempo de diseno.");
    }
}
