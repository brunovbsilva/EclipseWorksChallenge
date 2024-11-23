using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infra.Data.Configurations
{
    internal class LogConfiguration : IEntityTypeConfiguration<Log>
    {
        public void Configure(EntityTypeBuilder<Log> builder)
        {
            builder.ToTable("Logs");

            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).HasColumnName("Id").HasColumnType("uniqueidentifier").ValueGeneratedNever().IsRequired();
            builder.Property(x => x.ObjectFrom).HasColumnName("From").HasColumnType("varchar(max)").IsRequired();
            builder.Property(x => x.ObjectTo).HasColumnName("To").HasColumnType("varchar(max)").IsRequired();
            builder.Property(x => x.Type).HasColumnName("Type").HasColumnType("varchar(50)").IsRequired();
            builder.Property(x => x.Date).HasColumnName("Date").HasColumnType("datetime").IsRequired();
            builder.Property(x => x.UserId).HasColumnName("UserId").HasColumnType("uniqueidentifier").IsRequired();

            builder.HasOne(x => x.User).WithMany(x => x.Logs).HasForeignKey(x => x.UserId).OnDelete(DeleteBehavior.NoAction);
        }
    }
}
