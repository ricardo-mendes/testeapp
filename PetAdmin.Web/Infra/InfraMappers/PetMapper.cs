using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetAdmin.Web.Models.Domain;

namespace PetAdmin.Web.Infra.InfraMappers
{
    public class PetMapper : IEntityTypeConfiguration<Pet>
    {
        public void Configure(EntityTypeBuilder<Pet> builder)
        {
            builder.ToTable("Pet");

            builder.HasAnnotation("Relational:TableName", "Pet");

            builder.Property(e => e.Id).HasAnnotation("Relational:ColumnName", "PetId").ValueGeneratedOnAdd();

            builder.Property(e => e.Uid)
                .IsRequired()
                .HasAnnotation("Relational:ColumnName", "PetUid");

            builder.Property(e => e.PetTypeId)
                .IsRequired()
                .HasAnnotation("Relational:ColumnName", "PetTypeId");

            builder.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasAnnotation("Relational:ColumnName", "Name");

            builder.Property(e => e.Breed)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasAnnotation("Relational:ColumnName", "Breed");

            builder.Property(e => e.BirthDate)
                .HasColumnType("datetime")
                .HasAnnotation("Relational:ColumnName", "BirthDate");

            builder.Property(e => e.Gender)
                .IsRequired()
                .HasAnnotation("Relational:ColumnName", "Gender");

            builder.Property(e => e.Size)
                .IsRequired()
               .HasAnnotation("Relational:ColumnName", "Size");

            builder.Property(e => e.Coat)
                .IsRequired()
               .HasAnnotation("Relational:ColumnName", "Coat");

            builder.Property(e => e.Note)
                .HasMaxLength(400)
                .IsUnicode(false)
                .HasAnnotation("Relational:ColumnName", "Note");

            builder.Property(e => e.IsClub)
                .IsRequired()
                .HasAnnotation("Relational:ColumnName", "IsClub");

            builder.Property(e => e.PhotoUrl)
                .HasMaxLength(300)
                .HasAnnotation("Relational:ColumnName", "PhotoUrl");

            builder.Property(e => e.PhotoName)
                .HasMaxLength(180)
                .HasAnnotation("Relational:ColumnName", "PhotoName");

            //---------------

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

            //-------------------

            builder.HasOne(d => d.PetLover)
                .WithMany(p => p.PetList)
                .HasForeignKey(d => d.PetLoverId)
                .HasAnnotation("Relational:ColumnName", "PetLoverId")
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Pet_PetLover");

            //------------

            builder.HasIndex(e => e.PetLoverId)
                     .HasName("x_Pet_PetLoverId");
        }
    }
}
