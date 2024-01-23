using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PetAdmin.Web.Infra.InfraMappers;
using PetAdmin.Web.Models;
using PetAdmin.Web.Models.Domain;
using System;

namespace PetAdmin.Web.Infra
{
    public class PetContext : IdentityDbContext<User, Roles, Guid, UserClaim, UserRole, UserLogin, RoleClaim, UserToken>
    {
        public virtual DbSet<PetLover> PetLovers { get; set; }
        public virtual DbSet<Client> Clients { get; set; }
        public virtual DbSet<Pet> Pets { get; set; }
        public virtual DbSet<Employee> Employees { get; set; }
        public virtual DbSet<ScheduleItem> ScheduleItems { get; set; }
        public virtual DbSet<ScheduleItemEmployee> ScheduleItemEmployees { get; set; }
        public virtual DbSet<ScheduleItemClient> ScheduleItemClients { get; set; }
        public virtual DbSet<Schedule> Schedules { get; set; }
        public virtual DbSet<SchedulePet> SchedulePets { get; set; }
        public virtual DbSet<Location> Locations { get; set; }

        public PetContext(DbContextOptions<PetContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new LocationMapper());
            modelBuilder.ApplyConfiguration(new PetLoverMapper());
            modelBuilder.ApplyConfiguration(new ClientMapper());
            modelBuilder.ApplyConfiguration(new PetMapper());
            modelBuilder.ApplyConfiguration(new ScheduleItemMapper());
            modelBuilder.ApplyConfiguration(new ScheduleItemEmployeeMapper());
            modelBuilder.ApplyConfiguration(new ScheduleItemClientMapper());
            modelBuilder.ApplyConfiguration(new EmployeeMapper());
            modelBuilder.ApplyConfiguration(new ScheduleMapper());
            modelBuilder.ApplyConfiguration(new SchedulePetMapper());
            modelBuilder.ApplyConfiguration(new VaccineMapper());

            //-----------------------------

            modelBuilder.ApplyConfiguration(new UserMapper());

            modelBuilder.Entity<UserClaim>(b =>
            {
                b.ToTable("UserClaims");

                b.Property(e => e.CreationTime)
                .HasColumnType("datetime")
                .HasDefaultValueSql("(getutcdate())")
                .HasAnnotation("Relational:ColumnName", "CreationTime");
            });

            modelBuilder.Entity<UserLogin>(b =>
            {
                b.ToTable("UserLogins");

                b.Property(e => e.CreationTime)
                .HasColumnType("datetime")
                .HasDefaultValueSql("(getutcdate())")
                .HasAnnotation("Relational:ColumnName", "CreationTime");
            });

            modelBuilder.Entity<UserToken>(b =>
            {
                b.ToTable("UserTokens");

                b.Property(e => e.CreationTime)
                .HasColumnType("datetime")
                .HasDefaultValueSql("(getutcdate())")
                .HasAnnotation("Relational:ColumnName", "CreationTime");
            });

            modelBuilder.Entity<Roles>(b =>
            {
                b.ToTable("Roles");

                b.Property(e => e.CreationTime)
                .HasColumnType("datetime")
                .HasDefaultValueSql("(getutcdate())")
                .HasAnnotation("Relational:ColumnName", "CreationTime");
            });

            modelBuilder.Entity<RoleClaim>(b =>
            {
                b.ToTable("RoleClaims");

                b.Property(e => e.CreationTime)
                .HasColumnType("datetime")
                .HasDefaultValueSql("(getutcdate())")
                .HasAnnotation("Relational:ColumnName", "CreationTime");
            });

            modelBuilder.Entity<UserRole>(b =>
            {
                b.ToTable("UserRoles");

                b.Property(e => e.CreationTime)
                .HasColumnType("datetime")
                .HasDefaultValueSql("(getutcdate())")
                .HasAnnotation("Relational:ColumnName", "CreationTime");
            });
        }
    }
}
