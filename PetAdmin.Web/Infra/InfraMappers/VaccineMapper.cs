using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetAdmin.Web.Models.Domain;

namespace PetAdmin.Web.Infra.InfraMappers
{
    public class VaccineMapper : IEntityTypeConfiguration<Vaccine>
    {
        public void Configure(EntityTypeBuilder<Vaccine> builder)
        {
            builder.ToTable("Vaccine");

            builder.HasAnnotation("Relational:TableName", "Vaccine");

            builder.Property(e => e.Id).HasAnnotation("Relational:ColumnName", "VaccineId").ValueGeneratedOnAdd();

            builder.Property(e => e.Uid)
                .IsRequired()
                .HasAnnotation("Relational:ColumnName", "VaccineUid");

            builder.Property(e => e.PetId)
                .IsRequired()
                .HasAnnotation("Relational:ColumnName", "PetId");

            builder.Property(e => e.Name)
                .IsRequired()
                .HasAnnotation("Relational:ColumnName", "Name");

            builder.Property(e => e.Date)
                .IsRequired()
                .HasAnnotation("Relational:ColumnName", "Date");

            builder.Property(e => e.RevaccineDate).HasAnnotation("Relational:ColumnName", "RevaccineDate");

            builder.Property(e => e.ClinicName).HasAnnotation("Relational:ColumnName", "ClinicName");

            builder.Property(e => e.Note).HasAnnotation("Relational:ColumnName", "Note");

            builder.Property(e => e.PhotoUrl)
                .HasMaxLength(300)
                .HasAnnotation("Relational:ColumnName", "PhotoUrl");

            builder.Property(e => e.PhotoName)
                .HasMaxLength(180)
                .HasAnnotation("Relational:ColumnName", "PhotoName");

            //------------------

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

            builder.HasOne(v => v.Pet)
                .WithMany(p => p.VaccineList)
                .HasForeignKey(d => d.PetId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Vaccine_Pet");

            //------------

            builder.HasIndex(e => e.PetId)
                     .HasName("x_Vaccine_PetId");
        }
    }
}
