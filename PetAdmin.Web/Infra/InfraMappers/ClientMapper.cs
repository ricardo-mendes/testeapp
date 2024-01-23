using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetAdmin.Web.Models.Domain;

namespace PetAdmin.Web.Infra.InfraMappers
{
    public class ClientMapper : IEntityTypeConfiguration<Client>
    {
        public void Configure(EntityTypeBuilder<Client> builder)
        {
            builder.ToTable("Client");

            builder.HasAnnotation("Relational:TableName", "Client");

            builder.Property(e => e.Id).HasAnnotation("Relational:ColumnName", "ClientId").ValueGeneratedOnAdd();

            builder.Property(e => e.Uid)
                .IsRequired()
                .HasAnnotation("Relational:ColumnName", "ClientUid");

            builder.Property(e => e.UserId).HasAnnotation("Relational:ColumnName", "UserId");

            builder.Property(e => e.ProfileTypeId)
                .HasAnnotation("Relational:ColumnName", "ProfileTypeId");

            builder.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasAnnotation("Relational:ColumnName", "Name");

            builder.Property(e => e.Email)
                .IsRequired()
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasAnnotation("Relational:ColumnName", "Email");

            builder.Property(e => e.PhoneNumber)
                .IsRequired()
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasAnnotation("Relational:ColumnName", "PhoneNumber");

            builder.Property(e => e.LocationId)
                .IsRequired()
                .HasAnnotation("Relational:ColumnName", "LocationId");

            builder.Property(e => e.DocumentInformation)
                .HasAnnotation("Relational:ColumnName", "DocumentInformation");

            builder.Property(e => e.DocumentTypeId)
                .HasAnnotation("Relational:ColumnName", "DocumentTypeId");

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

            //--------------

            builder.HasOne(d => d.User)
                .WithMany(p => p.ClientList)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Client_User");

            builder.HasOne(d => d.Location)
                .WithMany(p => p.ClientList)
                .HasForeignKey(d => d.LocationId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Client_Location");

            //------------

            builder.HasIndex(e => e.UserId)
                     .HasName("x_Client_UserId");

            builder.HasIndex(e => e.LocationId)
                     .HasName("x_Client_LocationId");
        }
    }
}
