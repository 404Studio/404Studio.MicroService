using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using YH.Etms.Settlement.Api.Domain.Aggregates.TransportTaskAggregate;

namespace YH.Etms.Settlement.Api.Infrastructure.EntityConfigurations
{
    public class PayableEntityTypeConfiguration : IEntityTypeConfiguration<Payable>
    {
        public void Configure(EntityTypeBuilder<Payable> builder)
        {
            builder.ToTable("payable");

            builder.HasKey(p => p.Id);
            builder.Property(p => p.Id).IsRequired();

            builder.Property(p => p.SettlementUnit).HasMaxLength(256).IsRequired();
            builder.Property(p => p.SettlementUnitCode).HasMaxLength(256).IsRequired();
            builder.Property(p => p.Amount).IsRequired().Metadata.Relational().ColumnType = "decimal(18,2)";
            builder.Property(p => p.Status).IsRequired();
            builder.Property(p => p.Note).HasMaxLength(200).IsRequired(false);

            builder.HasKey(p => p.Id);
            builder.HasQueryFilter(v => v.IsDelete == false);
        }
    }

    public class PayableItemEntityTypeConfiguration : IEntityTypeConfiguration<PayableItem>
    {
        public void Configure(EntityTypeBuilder<PayableItem> builder)
        {
            builder.ToTable("payableitem");

            builder.HasKey(p => p.Id);
            builder.Property(p => p.Id).IsRequired();

            builder.Property(p => p.PayableBasis).IsRequired();
            builder.Property(p => p.CostType).IsRequired();
            builder.Property(p => p.Amount).IsRequired().Metadata.Relational().ColumnType = "decimal(18,2)";
            builder.Property(p => p.Note).HasMaxLength(200).IsRequired(false);
            builder.Property(p => p.Author).HasMaxLength(256).IsRequired(false);
            builder.Property(p => p.ModifiedBy).HasMaxLength(256).IsRequired(false);
            builder.Property(p => p.ModifiedAt).IsRequired(false);
            builder.Property(p => p.LowestPrice).IsRequired().Metadata.Relational().ColumnType = "decimal(18,2)";
            builder.Property(p => p.Number).IsRequired().Metadata.Relational().ColumnType = "decimal(18,2)";
            builder.Property(p => p.Unit).HasMaxLength(25).IsRequired(false);
            builder.Property(p => p.UnitPrice).IsRequired().Metadata.Relational().ColumnType = "decimal(18,2)";

            builder.HasKey(p => p.Id);
            builder.HasQueryFilter(v => v.IsDelete == false);
        }
    }



    public class TransportTaskEntityTypeConfiguration : IEntityTypeConfiguration<TransportTask>
    {
        public void Configure(EntityTypeBuilder<TransportTask> builder)
        {
            builder.ToTable("transporttask");

            builder.HasKey(p => p.Id);
            builder.Property(p => p.Id).IsRequired();

            builder.Property(p => p.OperationID).IsRequired();
            builder.Property(p => p.Weight).IsRequired().Metadata.Relational().ColumnType = "decimal(18,6)";
            builder.Property(p => p.Volume).IsRequired().Metadata.Relational().ColumnType = "decimal(18,6)";
            builder.Property(p => p.Line).IsRequired();
            builder.Property(p => p.LineCode).HasMaxLength(256).IsRequired();
            builder.Property(p => p.CargoType).IsRequired();
            builder.Property(p => p.PackageType).IsRequired();
            builder.Property(p => p.TransportMode).IsRequired();

            builder.HasKey(p => p.Id);
            builder.HasOne(t => t.Payable).WithOne(p => p.TransportTask);
            builder.HasQueryFilter(v => v.IsDelete == false);
        }
    }
}
