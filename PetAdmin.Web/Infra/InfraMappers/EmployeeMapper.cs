using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetAdmin.Web.Models.Domain;

namespace PetAdmin.Web.Infra.InfraMappers
{
    public class EmployeeMapper : IEntityTypeConfiguration<Employee>
    {
        public void Configure(EntityTypeBuilder<Employee> builder)
        {
            builder.ToTable("Employee");

            builder.HasAnnotation("Relational:TableName", "Employee");

            builder.Property(e => e.Id).HasAnnotation("Relational:ColumnName", "EmployeeId").ValueGeneratedOnAdd();

            builder.Property(e => e.Uid)
                .IsRequired()
                .HasAnnotation("Relational:ColumnName", "EmployeeUid");

            builder.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(250)
                .IsUnicode(false)
                .HasAnnotation("Relational:ColumnName", "Name");

            builder.Property(e => e.Email)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasAnnotation("Relational:ColumnName", "Email");

            builder.Property(e => e.PhoneNumber)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasAnnotation("Relational:ColumnName", "PhoneNumber");

            //------------

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

            builder.Property(e => e.IsActive)
                .IsRequired()
                .HasAnnotation("Relational:ColumnName", "IsActive");

            //----------------

            builder.HasOne(d => d.Client)
                .WithMany(p => p.EmployeeList)
                .HasForeignKey(d => d.ClientId)
                .HasAnnotation("Relational:ColumnName", "ClientId")
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Employee_Client");

            //------------

            builder.HasIndex(e => e.ClientId)
                     .HasName("x_Employee_ClientId");
        }
    }
}
