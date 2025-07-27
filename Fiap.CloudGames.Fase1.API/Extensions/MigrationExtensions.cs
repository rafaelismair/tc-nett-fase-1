using Fiap.CloudGames.Fase1.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Fiap.CloudGames.Fase1.API.Extensions
{
    public static class MigrationExtensions
    {
        public static void ApplyMigrations(this IApplicationBuilder app)
        {
            using IServiceScope scope = app.ApplicationServices.CreateScope();

            using ApplicationDbContext dbContext = 
                scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            dbContext.Database.Migrate();
        }
    }
}
