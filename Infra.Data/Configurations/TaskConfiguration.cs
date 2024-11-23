using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infra.Data.Configurations
{
    internal class TaskConfiguration : IEntityTypeConfiguration<Domain.Entities.Task>
    {
        public void Configure(EntityTypeBuilder<Domain.Entities.Task> builder)
        {
            builder.ToTable("Tasks");

            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).HasColumnName("Id").HasColumnType("uniqueidentifier").ValueGeneratedNever().IsRequired();
            builder.Property(x => x.Title).HasColumnName("Title").HasColumnType("varchar(50)").IsRequired();
            builder.Property(x => x.Description).HasColumnName("Description").HasColumnType("varchar(max)").IsRequired();
            builder.Property(x => x.Status).HasColumnName("Status").HasColumnType("int").IsRequired();
            builder.Property(x => x.Priority).HasColumnName("Priority").HasColumnType("int").IsRequired();
            builder.Property(x => x.ProjectId).HasColumnName("ProjectId").HasColumnType("uniqueidentifier").IsRequired();

            builder.HasOne(x => x.Project).WithMany(x => x.Tasks).HasForeignKey(x => x.ProjectId).OnDelete(DeleteBehavior.NoAction);
            builder.HasMany(x => x.Comments).WithOne(x => x.Task).HasForeignKey(x => x.TaskId).OnDelete(DeleteBehavior.Cascade);
        }
    }
}
