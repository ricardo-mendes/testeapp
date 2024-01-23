using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetAdmin.Web.Models.Domain;

namespace PetAdmin.Web.Infra.InfraMappers
{
    public class ScheduleItemEmployeeMapper : IEntityTypeConfiguration<ScheduleItemEmployee>
    {
        public void Configure(EntityTypeBuilder<ScheduleItemEmployee> builder)
        {
            builder.ToTable("ScheduleItemEmployee");

            builder.HasAnnotation("Relational:TableName", "ScheduleItemEmployee");

            builder.Property(e => e.Id).HasAnnotation("Relational:ColumnName", "ScheduleItemEmployeeId").ValueGeneratedOnAdd();

            builder.Property(e => e.Uid)
                .IsRequired()
                .HasAnnotation("Relational:ColumnName", "ScheduleItemEmployeeUid");

            builder.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(300)
                .IsUnicode(false)
                .HasAnnotation("Relational:ColumnName", "Name");

            builder.Property(e => e.Price)
                .HasAnnotation("Relational:ColumnName", "Price")
                .HasColumnType("decimal(18,2)");

            builder.Property(e => e.Hour)
                .HasAnnotation("Relational:ColumnName", "Hour");

            builder.Property(e => e.Minutes)
                .HasAnnotation("Relational:ColumnName", "Minutes");

            //---------------------

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

            builder.HasOne(d => d.Employee)
                .WithMany(p => p.ScheduleItemEmployeeList)
                .HasForeignKey(d => d.EmployeeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ScheduleItemEmployee_Employee");

            builder.HasOne(d => d.ScheduleItem)
                .WithMany(p => p.ScheduleItemEmployeeList)
                .HasForeignKey(d => d.ScheduleItemId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ScheduleItemEmployee_ScheduleItem");

            //------------

            builder.HasIndex(e => e.EmployeeId)
                     .HasName("x_ScheduleItemEmployee_EmployeeId");

            builder.HasIndex(e => e.ScheduleItemId)
                     .HasName("x_ScheduleItemEmployee_ScheduleItemId");
        }
    }
}
