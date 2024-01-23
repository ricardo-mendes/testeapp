using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetAdmin.Web.Models.Domain;

namespace PetAdmin.Web.Infra.InfraMappers
{
    public class PetLoverMapper : IEntityTypeConfiguration<PetLover>
    {
        public void Configure(EntityTypeBuilder<PetLover> builder)
        {
            builder.ToTable("PetLover");

            builder.HasAnnotation("Relational:TableName", "PetLover");

            builder.Property(e => e.Id).HasAnnotation("Relational:ColumnName", "PetLoverId").ValueGeneratedOnAdd();

            builder.Property(e => e.Uid)
                .IsRequired()
                .HasAnnotation("Relational:ColumnName", "PetLoverUid");

            builder.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(500)
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

            builder.Property(e => e.Gender)
                .HasAnnotation("Relational:ColumnName", "Gender");

            builder.Property(e => e.ClientId)
                .HasAnnotation("Relational:ColumnName", "ClientId");

            builder.Property(e => e.LocationId)
                .IsRequired()
                .HasAnnotation("Relational:ColumnName", "LocationId");

            builder.Property(e => e.IsClub)
                .IsRequired()
                .HasAnnotation("Relational:ColumnName", "IsClub");

            //--------------

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

            builder.HasOne(d => d.Client)
                .WithMany(p => p.PetLoverList)
                .HasForeignKey(d => d.ClientId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PetLover_Client");

            builder.HasOne(d => d.User)
                .WithMany(p => p.PetLoverList)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PetLover_User");

            builder.HasOne(d => d.Location)
                .WithMany(p => p.PetLoverList)
                .HasForeignKey(d => d.LocationId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PetLover_Location");

            // ------------

            builder.HasIndex(e => e.UserId)
                     .HasName("x_PetLover_UserId");

            builder.HasIndex(e => e.ClientId)
                     .HasName("x_PetLover_ClientId");

            builder.HasIndex(e => e.LocationId)
                     .HasName("x_PetLover_LocationId");
        }
    }
}
