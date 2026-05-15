using AgendAI.Infra.Persistence.Seed;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace AgendAI.Infra.Persistence;

public static class DatabaseInitializer
{
    private const string ProductVersion = "8.0.11";

    public static async Task InitializeAsync(IServiceProvider services, IHostEnvironment environment)
    {
        using var scope = services.CreateScope();
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<AgendAiDbContext>>();
        var db = scope.ServiceProvider.GetRequiredService<AgendAiDbContext>();

        try
        {
            if (db.Database.IsInMemory())
            {
                await db.Database.EnsureCreatedAsync();

                if (!await db.Usuarios.AnyAsync())
                    await AgendAiDbSeeder.SeedAsync(db);

                logger.LogInformation("Banco em memória pronto (dados mock para testes).");
                return;
            }

            await ApplyMigrationsAsync(db, logger);

            if (!await db.Usuarios.AnyAsync())
            {
                await AgendAiDbSeeder.SeedAsync(db);
                logger.LogInformation("Migrações aplicadas e seed inicial executado.");
                return;
            }

            logger.LogInformation("Migrações aplicadas; banco já possui usuários.");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Falha ao inicializar o banco de dados.");
            throw;
        }
    }

    private static async Task ApplyMigrationsAsync(AgendAiDbContext db, ILogger logger)
    {
        var pending = (await db.Database.GetPendingMigrationsAsync()).ToList();
        if (pending.Count == 0)
            return;

        var applied = await db.Database.GetAppliedMigrationsAsync();
        if (!applied.Any() && await LegacySchemaExistsAsync(db))
        {
            await BaselineMigrationsAsync(db, pending, logger);
            return;
        }

        try
        {
            await db.Database.MigrateAsync();
        }
        catch (SqlException ex) when (ex.Number == 2714)
        {
            if (!await LegacySchemaExistsAsync(db))
                throw;

            logger.LogWarning(
                ex,
                "Migração falhou porque o esquema já existe; registrando migrações pendentes como aplicadas.");
            await BaselineMigrationsAsync(db, pending, logger);
        }
    }

    private static async Task<bool> LegacySchemaExistsAsync(AgendAiDbContext db) =>
        await db.Database.SqlQueryRaw<int>(
                """
                SELECT COUNT(*) AS [Value]
                FROM INFORMATION_SCHEMA.TABLES
                WHERE TABLE_SCHEMA = 'dbo' AND TABLE_NAME = 'Usuarios'
                """)
            .SingleAsync() > 0;

    private static async Task BaselineMigrationsAsync(
        AgendAiDbContext db,
        IReadOnlyList<string> migrationIds,
        ILogger logger)
    {
        foreach (var migrationId in migrationIds)
        {
            await db.Database.ExecuteSqlInterpolatedAsync(
                $"""
                 IF NOT EXISTS (SELECT 1 FROM [__EFMigrationsHistory] WHERE [MigrationId] = {migrationId})
                 INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
                 VALUES ({migrationId}, {ProductVersion})
                 """);
        }

        logger.LogWarning(
            "Esquema legado detectado; {Count} migração(ões) registrada(s) sem recriar tabelas.",
            migrationIds.Count);
    }
}
