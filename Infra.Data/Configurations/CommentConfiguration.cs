using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infra.Data.Configurations
{
    internal class CommentConfiguration : IEntityTypeConfiguration<Comment>
    {
        public void Configure(EntityTypeBuilder<Comment> builder)
        {
            builder.ToTable("Comments");

            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).HasColumnName("Id").HasColumnType("uniqueidentifier").ValueGeneratedNever().IsRequired();
            builder.Property(x => x.Value).HasColumnName("Value").HasColumnType("varchar(max)").IsRequired();
            builder.Property(x => x.TaskId).HasColumnName("TaskId").HasColumnType("uniqueidentifier").IsRequired();

            builder.HasOne(x => x.Task).WithMany(x => x.Comments).HasForeignKey(x => x.TaskId).OnDelete(DeleteBehavior.NoAction);
        }
    }
}
