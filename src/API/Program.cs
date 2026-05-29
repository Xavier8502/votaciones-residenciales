using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;
using Serilog;
using VotacionesResidenciales.API.Middleware;
using VotacionesResidenciales.Application;
using VotacionesResidenciales.Infrastructure;
using VotacionesResidenciales.Infrastructure.Hubs;
using VotacionesResidenciales.Infrastructure.Persistence;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateBootstrapLogger();

try
{
    Log.Information("Iniciando VotacionesResidenciales.API");

    var builder = WebApplication.CreateBuilder(args);

    builder.Configuration
        .AddJsonFile("appsettings.Local.json", optional: true, reloadOnChange: true)
        .AddJsonFile(
            $"appsettings.{builder.Environment.EnvironmentName}.local.json",
            optional: true,
            reloadOnChange: true);

    builder.Host.UseSerilog((ctx, lc) => lc
        .ReadFrom.Configuration(ctx.Configuration)
        .WriteTo.Console()
        .WriteTo.File(
            path: "logs/api-.log",
            rollingInterval: RollingInterval.Day,
            retainedFileCountLimit: 30));

    builder.Services.AddApplication();
    builder.Services.AddInfrastructure(builder.Configuration);
    builder.Services.AddControllers();

    var databaseSettings = AppDbContextOptionsConfigurator.GetSettings(
        builder.Configuration);

    builder.Services.AddOpenApi(options =>
    {
        options.AddDocumentTransformer((document, context, ct) =>
        {
            document.Info.Title = "Votaciones Residenciales API";
            document.Info.Version = "v1";
            document.Info.Description =
                "API para gestion de votaciones en conjuntos residenciales.";
            return Task.CompletedTask;
        });
    });

    const string corsPolicyName = "ApiCors";
    var allowedOrigins = builder.Configuration
        .GetSection("Cors:AllowedOrigins")
        .Get<string[]>() ?? [];

    if (allowedOrigins.Length > 0)
    {
        builder.Services.AddCors(options =>
        {
            options.AddPolicy(corsPolicyName, policy =>
                policy
                    .WithOrigins(allowedOrigins)
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowCredentials());
        });
    }

    var app = builder.Build();

    app.UseMiddleware<ExceptionMiddleware>();

    if (app.Environment.IsDevelopment())
    {
        app.MapOpenApi();
        app.MapScalarApiReference(options =>
        {
            options.Title = "Votaciones Residenciales API";
            options.Theme = ScalarTheme.DeepSpace;
            options.DefaultHttpClient = new(
                ScalarTarget.JavaScript,
                ScalarClient.Fetch);
            options.Authentication = new ScalarAuthenticationOptions
            {
                PreferredSecurityScheme = "Bearer"
            };
        });
    }

    app.UseSerilogRequestLogging();
    app.UseHttpsRedirection();

    if (allowedOrigins.Length > 0)
    {
        app.UseCors(corsPolicyName);
    }

    app.UseAuthentication();
    app.UseAuthorization();

    app.MapControllers();
    app.MapHub<VotacionHub>("/hubs/votacion");

    using (var scope = app.Services.CreateScope())
    {
        var context = scope.ServiceProvider
            .GetRequiredService<AppDbContext>();

        var logger = scope.ServiceProvider
            .GetRequiredService<ILogger<Program>>();

        if (databaseSettings.ApplyMigrationsOnStartup)
        {
            logger.LogInformation(
                "Aplicando migraciones usando proveedor {Provider}...",
                databaseSettings.Provider);

            try
            {
                await context.Database.MigrateAsync();
            }
            catch (FileNotFoundException ex)
            {
                logger.LogWarning(
                    ex,
                    "No se encontro un ensamblado requerido para aplicar migraciones " +
                    "automaticas. Assembly esperado: {MigrationsAssembly}. " +
                    "Se omitira la auto-aplicacion de migraciones en este arranque.",
                    databaseSettings.MigrationsAssemblyName);
            }
        }

        await AppDbContextSeed.SeedAsync(context, logger);
    }

    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "La aplicacion fallo al iniciar.");
}
finally
{
    Log.CloseAndFlush();
}
