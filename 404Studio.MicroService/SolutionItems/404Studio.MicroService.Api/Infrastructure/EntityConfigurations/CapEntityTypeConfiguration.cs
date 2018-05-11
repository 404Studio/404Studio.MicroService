using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using YH.Etms.Utility.Models.CapEventModel;

namespace YH.Etms.Settlement.Api.Infrastructure.EntityConfigurations
{
    public class PublishEntityTypeConfiguration : IEntityTypeConfiguration<Publish>
    {
        public void Configure(EntityTypeBuilder<Publish> builder)
        {
            builder.ToTable("cap.published");
            builder.HasKey(p => p.Id);
            builder.Property(p => p.Id).IsRequired();

            builder.Property(p => p.Name).HasMaxLength(200).IsRequired();
            builder.Property(p => p.Content).IsRequired(false);
            builder.Property(p => p.Retries).IsRequired(false);
            builder.Property(p => p.Added).IsRequired();
            builder.Property(p => p.ExpiresAt).IsRequired(false);
            builder.Property(p => p.StatusName).HasMaxLength(40).IsRequired();
        }
    }

    public class ReceiveEntityTypeConfiguration : IEntityTypeConfiguration<Receive>
    {
        public void Configure(EntityTypeBuilder<Receive> builder)
        {
            builder.ToTable("cap.received");
            builder.HasKey(p => p.Id);
            builder.Property(p => p.Id).IsRequired();

            builder.Property(p => p.Name).HasMaxLength(400).IsRequired();
            builder.Property(p => p.Group).HasMaxLength(200).IsRequired(false);
            builder.Property(p => p.Content).IsRequired(false);
            builder.Property(p => p.Retries).IsRequired(false);
            builder.Property(p => p.Added).IsRequired();
            builder.Property(p => p.ExpiresAt).IsRequired(false);
            builder.Property(p => p.StatusName).HasMaxLength(50).IsRequired();
        }
    }
}
