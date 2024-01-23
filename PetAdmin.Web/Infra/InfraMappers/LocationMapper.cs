using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetAdmin.Web.Models.Domain;

namespace PetAdmin.Web.Infra.InfraMappers
{
    public class LocationMapper : IEntityTypeConfiguration<Location>
    {
        public void Configure(EntityTypeBuilder<Location> builder)
        {
            builder.ToTable("Location");

            builder.HasAnnotation("Relational:TableName", "Location");

            builder.Property(e => e.Id).HasAnnotation("Relational:ColumnName", "LocationId").ValueGeneratedOnAdd();

            builder.Property(e => e.Uid)
                .IsRequired()
                .HasAnnotation("Relational:ColumnName", "LocationUid");

            builder.Property(e => e.CountryCode)
                .HasMaxLength(3)
                .IsUnicode(false)
                .HasAnnotation("Relational:ColumnName", "CountryCode");

            builder.Property(e => e.StateCode)
                .HasMaxLength(3)
                .IsUnicode(false)
                .HasAnnotation("Relational:ColumnName", "StateCode");

            builder.Property(e => e.CityName)
                .HasMaxLength(125)
                .IsUnicode(false)
                .HasAnnotation("Relational:ColumnName", "CityName");

            builder.Property(e => e.Neighborhood)
                .HasMaxLength(125)
                .IsUnicode(false)
                .HasAnnotation("Relational:ColumnName", "Neighborhood");

            builder.Property(e => e.StreetName)
                .HasMaxLength(300)
                .IsUnicode(false)
                .HasAnnotation("Relational:ColumnName", "StreetName");

            builder.Property(e => e.StreetNumber)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasAnnotation("Relational:ColumnName", "StreetNumber");

            builder.Property(e => e.PostalCode)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasAnnotation("Relational:ColumnName", "PostalCode");

            builder.Property(e => e.Complement)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasAnnotation("Relational:ColumnName", "Complement");

            builder.Property(e => e.Latitude)
                .IsRequired()
                .HasAnnotation("Relational:ColumnName", "Latitude");

            builder.Property(e => e.Longitue)
                .IsRequired()
                .HasAnnotation("Relational:ColumnName", "Longitue");


            //--------------------

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
        }
    }
}
