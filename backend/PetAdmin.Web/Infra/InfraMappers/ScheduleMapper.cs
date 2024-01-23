using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetAdmin.Web.Models.Domain;

namespace PetAdmin.Web.Infra.InfraMappers
{
    public class ScheduleMapper : IEntityTypeConfiguration<Schedule>
    {
        public void Configure(EntityTypeBuilder<Schedule> builder)
        {
            builder.ToTable("Schedule");

            builder.HasAnnotation("Relational:TableName", "Schedule");

            builder.Property(e => e.Id).HasAnnotation("Relational:ColumnName", "ScheduleId").ValueGeneratedOnAdd();

            builder.Property(e => e.Uid)
                .IsRequired()
                .HasAnnotation("Relational:ColumnName", "ScheduleUid");

            builder.Property(e => e.Period)
                .HasAnnotation("Relational:ColumnName", "Period");

            builder.Property(e => e.Date)
                .HasColumnType("datetime")
                .IsRequired()
                .HasAnnotation("Relational:ColumnName", "Date");

            builder.Property(e => e.DateWithHour)
                .IsRequired()
                .HasAnnotation("Relational:ColumnName", "DateWithHour");

            builder.Property(e => e.Status)
                .IsRequired()
                .HasAnnotation("Relational:ColumnName", "Status");

            builder.Property(e => e.QuantityAllowed)
                .IsRequired()
                .HasAnnotation("Relational:ColumnName", "QuantityAllowed");

            builder.Property(e => e.QuantityOccupied)
                .IsRequired()
                .HasAnnotation("Relational:ColumnName", "QuantityOccupied");

            //--------

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

            //--------------

            builder.HasOne(d => d.Employee)
                .WithMany(p => p.ScheduleList)
                .HasForeignKey(d => d.EmployeeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Schedule_Employee");

            //---------------

            builder.HasAlternateKey(s => new { s.EmployeeId, s.DateWithHour }).HasName("UC_Employee_DateWithHour");

            //------------

            builder.HasIndex(e => e.EmployeeId)
                     .HasName("x_Schedule_EmployeeId");
        }
    }
}
