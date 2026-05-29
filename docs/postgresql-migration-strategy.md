# PostgreSQL Migration Strategy

This repository keeps two migration tracks:

- SQL Server migrations remain in `src/Infrastructure/Persistence/Migrations`
- PostgreSQL migrations live in `src/PostgreSqlMigrations/Persistence/Migrations`

This follows the EF Core recommendation of keeping separate migration sets per provider when the same model needs to target multiple databases.

Official references:

- https://learn.microsoft.com/en-us/ef/core/managing-schemas/migrations/providers
- https://learn.microsoft.com/en-us/ef/core/managing-schemas/migrations/projects

## Local PostgreSQL connection

The local PostgreSQL route captured for this project is:

- Host: `localhost`
- Port: `5433`
- Database: `mi_basedatos`
- Username: `admin`

The screenshot does not expose the password, so the sample config uses `__SET_ME__`.

## Example connection string

```text
Host=localhost;Port=5433;Database=mi_basedatos;Username=admin;Password=__SET_ME__
```

## Scaffold a new PostgreSQL migration

```powershell
$env:Database__Provider='PostgreSql'
$env:Database__ConnectionStringName='PostgreSqlConnection'
dotnet ef migrations add <MigrationName> `
  --project C:\code\VotacionesResidenciales\src\PostgreSqlMigrations\VotacionesResidenciales.PostgreSqlMigrations.csproj `
  --startup-project C:\code\VotacionesResidenciales\src\API\VotacionesResidenciales.API.csproj `
  --context AppDbContext `
  --output-dir Persistence/Migrations
Remove-Item Env:Database__Provider
Remove-Item Env:Database__ConnectionStringName
```

## Apply PostgreSQL migrations

```powershell
$env:Database__Provider='PostgreSql'
$env:Database__ConnectionStringName='PostgreSqlConnection'
dotnet ef database update `
  --project C:\code\VotacionesResidenciales\src\PostgreSqlMigrations\VotacionesResidenciales.PostgreSqlMigrations.csproj `
  --startup-project C:\code\VotacionesResidenciales\src\API\VotacionesResidenciales.API.csproj `
  --context AppDbContext
Remove-Item Env:Database__Provider
Remove-Item Env:Database__ConnectionStringName
```

## Runtime note for Render

For PostgreSQL deployments, keep:

```text
Database__ApplyMigrationsOnStartup=false
```

The API does not automatically ship the PostgreSQL migrations assembly at runtime. The safe workflow is:

1. build the API
2. run `dotnet ef database update` against `src/PostgreSqlMigrations`
3. start the API

That keeps startup deterministic and avoids migration assembly loading issues in free-host environments.
