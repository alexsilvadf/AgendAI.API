using AgendAI.Infra.Persistence.Seed;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace AgendAI.Infra.Persistence;

public static class DatabaseInitializer
{
    public static async Task InitializeAsync(IServiceProvider services, IHostEnvironment environment)
    {
        using var scope = services.CreateScope();
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<AgendAiDbContext>>();
        var db = scope.ServiceProvider.GetRequiredService<AgendAiDbContext>();

        try
        {
            if (environment.IsDevelopment())
            {
                await db.Database.EnsureCreatedAsync();
                await AgendAiDbSeeder.SeedAsync(db);
                logger.LogInformation("Banco inicializado e seed de desenvolvimento aplicado.");
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Falha ao inicializar o banco de dados.");
            throw;
        }
    }
}
