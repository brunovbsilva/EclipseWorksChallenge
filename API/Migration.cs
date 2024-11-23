using Infra.Data;
using Microsoft.EntityFrameworkCore;

namespace API
{
    public static class Migration
    {
        public static void ApplyMigrations(this WebApplication app)
        {
            using var scope = app.Services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<Context>();
            context.Database.Migrate();
        }
    }
}
