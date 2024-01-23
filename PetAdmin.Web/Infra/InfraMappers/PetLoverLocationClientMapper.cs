using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetAdmin.Web.Models.Domain;

namespace PetAdmin.Web.Infra.InfraMappers
{
    public class PetLoverLocationClientMapper : IEntityTypeConfiguration<PetLoverLocationClient>
    {
        public void Configure(EntityTypeBuilder<PetLoverLocationClient> builder)
        {
            builder.ToTable("PetLoverLocationClient");

            builder.HasAnnotation("Relational:TableName", "PetLoverLocationClient");

            builder.Property(e => e.Id).HasAnnotation("Relational:ColumnName", "Id").ValueGeneratedOnAdd();

            builder.Property(e => e.PetLoverId)
                .HasAnnotation("Relational:ColumnName", "PetLoverId");

            builder.Property(e => e.ClientId)
                .HasAnnotation("Relational:ColumnName", "ClientId");

            builder.Property(e => e.Distance)
                .HasAnnotation("Relational:ColumnName", "Distance");

            //---------------

            builder.HasOne(d => d.PetLover)
                .WithMany(p => p.PetLoverLocationClientList)
                .HasForeignKey(d => d.PetLoverId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PetLoverLocationClient_PetLover");

            builder.HasOne(d => d.Client)
                .WithMany(p => p.PetLoverLocationClientList)
                .HasForeignKey(d => d.ClientId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PetLoverLocationClient_Client");

            //------------

            builder.HasIndex(e => e.PetLoverId)
                     .HasName("x_PetLoverLocationClient_PetLoverId");

            builder.HasIndex(e => e.ClientId)
                     .HasName("x_PetLoverLocationClient_ClientId");
        }
    }
}
