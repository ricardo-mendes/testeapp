using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetAdmin.Web.Models.Domain;

namespace PetAdmin.Web.Infra.InfraMappers
{
    public class ScheduleItemClientMapper : IEntityTypeConfiguration<ScheduleItemClient>
    {
        public void Configure(EntityTypeBuilder<ScheduleItemClient> builder)
        {
            builder.ToTable("ScheduleItemClient");

            builder.HasAnnotation("Relational:TableName", "ScheduleItemClient");

            builder.Property(e => e.Id).HasAnnotation("Relational:ColumnName", "ScheduleItemClientId").ValueGeneratedOnAdd();

            builder.Property(e => e.Uid)
                .IsRequired()
                .HasAnnotation("Relational:ColumnName", "ScheduleItemClientUid");

            //-------------

            builder.Property(e => e.IsActive)
                .IsRequired()
                .HasAnnotation("Relational:ColumnName", "IsActive");

            builder.Property(e => e.IsDeleted)
                .IsRequired()
                .HasAnnotation("Relational:ColumnName", "IsDeleted");

            builder.Property(e => e.CreationTime)
                .IsRequired()
                .HasColumnType("datetime")
                .HasDefaultValueSql("(getutcdate())")
                .HasAnnotation("Relational:ColumnName", "CreationTime");

            builder.Property(e => e.CreatedUserId)
                .HasAnnotation("Relational:ColumnName", "CreatedUserId");

            builder.Property(e => e.LastModificationTime)
                .HasColumnType("datetime")
                .HasAnnotation("Relational:ColumnName", "LastModificationTime");

            builder.Property(e => e.LastModificationUserId)
                .HasAnnotation("Relational:ColumnName", "LastModificationUserId");

            builder.Property(e => e.DeletionTime)
                .HasColumnType("datetime")
                .HasAnnotation("Relational:ColumnName", "DeletionTime");

            builder.Property(e => e.DeletedUserId)
                .HasAnnotation("Relational:ColumnName", "DeletedUserId");

            //----------------

            builder.HasOne(d => d.Client)
                .WithMany(p => p.ScheduleItemClientList)
                .HasForeignKey(d => d.ClientId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ScheduleItemClient_Client");

            builder.HasOne(d => d.ScheduleItem)
                .WithMany(p => p.ScheduleItemClientList)
                .HasForeignKey(d => d.ScheduleItemId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ScheduleItemClient_ScheduleItem");

            // ------------

            builder.HasIndex(e => e.ClientId)
                     .HasName("x_ScheduleItemClient_ClientId");

            builder.HasIndex(e => e.ScheduleItemId)
                     .HasName("x_ScheduleItemClient_ScheduleItemId");
        }
    }
}
