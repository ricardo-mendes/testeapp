using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetAdmin.Web.Models;

namespace PetAdmin.Web.Infra.InfraMappers
{
    public class UserMapper : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("Users");

            builder.HasAnnotation("Relational:TableName", "Users");

            builder.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasAnnotation("Relational:ColumnName", "Name");

            builder.Property(e => e.UserTypeId)
                .IsRequired()
                .HasAnnotation("Relational:ColumnName", "UserTypeId");

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
        }
    }
}
