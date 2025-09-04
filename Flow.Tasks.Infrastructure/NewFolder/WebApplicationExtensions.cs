using Flow.Tasks.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Builder;
using Flow.Tasks.Domain.Entities;

public static class WebApplicationExtensions
{
    public static async Task ApplyMigrationsAsync(this WebApplication app, bool seed = false)
    {
        using var scope = app.Services.CreateScope();
        var logger = scope.ServiceProvider.GetRequiredService<ILoggerFactory>()
                                          .CreateLogger("DbMigration");

        var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();


        const int maxRetries = 5;
        var delay = TimeSpan.FromSeconds(2);

        for (var attempt = 1; attempt <= maxRetries; attempt++)
        {
            try
            {
                logger.LogInformation("Applying EF Core migrations (attempt {Attempt}/{Max})…", attempt, maxRetries);
                await db.Database.MigrateAsync(); // creates DB if missing, then applies migrations
                logger.LogInformation("Migrations applied.");

                if (seed)
                {
                    await SeedAsync(db, logger);
                }
                return;
            }
            catch (Exception ex)
            {
                logger.LogWarning(ex, "Migration failed at attempt {Attempt}/{Max}", attempt, maxRetries);
                if (attempt == maxRetries) throw; // rethrow after last attempt
                await Task.Delay(delay);
            }
        }
    }

    private static async Task SeedAsync(AppDbContext db, ILogger logger)
    {
        // seed a few users if none exist
        if (!await db.Users.AnyAsync())
        {
            db.Users.AddRange(
                new UserEntity { FirstName = "Annie", LastName = "Desmarais", Email = "annie@example.com", IsActive = true },
                new UserEntity { FirstName = "Cíntia", LastName = "Soares", Email = "cintia@example.com", IsActive = true },
                new UserEntity { FirstName = "Claudy", LastName = "Picard", Email = "claudy@example.com", IsActive = true }
            );
            await db.SaveChangesAsync();
            logger.LogInformation("Seeded default users.");
        }
    }
}
