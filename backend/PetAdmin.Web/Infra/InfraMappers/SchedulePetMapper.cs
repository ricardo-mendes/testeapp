using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetAdmin.Web.Models.Domain;

namespace PetAdmin.Web.Infra.InfraMappers
{
    public class SchedulePetMapper : IEntityTypeConfiguration<SchedulePet>
    {
        public void Configure(EntityTypeBuilder<SchedulePet> builder)
        {
            builder.ToTable("SchedulePet");

            builder.HasAnnotation("Relational:TableName", "SchedulePet");

            builder.Property(e => e.Id).HasAnnotation("Relational:ColumnName", "SchedulePetId").ValueGeneratedOnAdd();

            builder.Property(e => e.Uid)
                .IsRequired()
                .HasAnnotation("Relational:ColumnName", "SchedulePetUid");

            builder.Property(e => e.Note)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasAnnotation("Relational:ColumnName", "Note");

            builder.Property(e => e.Status)
                .IsRequired()
                .HasAnnotation("Relational:ColumnName", "Status");

            //-------------

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

            //--------------

            builder.HasOne(d => d.PetLover)
                .WithMany(p => p.SchedulePetList)
                .HasForeignKey(d => d.PetLoverId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SchedulePet_PetLover");

            builder.HasOne(d => d.Pet)
                .WithMany(p => p.SchedulePetList)
                .HasForeignKey(d => d.PetId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SchedulePet_Pet");

            builder.HasOne(d => d.Schedule)
                .WithMany(p => p.SchedulePetList)
                .HasForeignKey(d => d.ScheduleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SchedulePet_Schedule");

            builder.HasOne(d => d.ScheduleItemEmployee)
                .WithMany(p => p.SchedulePetList)
                .HasForeignKey(d => d.ScheduleItemEmployeeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SchedulePet_ScheduleItemEmployee");

            //------------

            builder.HasIndex(e => e.ScheduleId)
                     .HasName("x_SchedulePet_ScheduleId");

            builder.HasIndex(e => e.PetLoverId)
                     .HasName("x_SchedulePet_PetLoverId");

            builder.HasIndex(e => e.PetId)
                     .HasName("x_SchedulePet_PetId");

            builder.HasIndex(e => e.ScheduleItemEmployeeId)
                     .HasName("x_SchedulePet_ScheduleItemEmployeeId");
        }
    }
}
