using Infra.Data.Configurations;
using Microsoft.EntityFrameworkCore;

namespace Infra.Data
{
    public class Context(DbContextOptions<Context> options) : DbContext(options)
    {
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(UserConfiguration).Assembly);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ProjectConfiguration).Assembly);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(TaskConfiguration).Assembly);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(LogConfiguration).Assembly);
            base.OnModelCreating(modelBuilder);
        }
    }
}
