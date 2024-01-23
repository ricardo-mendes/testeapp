using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace PetAdmin.Web.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Location",
                columns: table => new
                {
                    LocationId = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    LocationUid = table.Column<Guid>(nullable: false),
                    CountryCode = table.Column<string>(unicode: false, maxLength: 3, nullable: true),
                    StateCode = table.Column<string>(unicode: false, maxLength: 3, nullable: true),
                    CityName = table.Column<string>(unicode: false, maxLength: 125, nullable: true),
                    Neighborhood = table.Column<string>(unicode: false, maxLength: 125, nullable: true),
                    StreetName = table.Column<string>(unicode: false, maxLength: 300, nullable: true),
                    StreetNumber = table.Column<string>(unicode: false, maxLength: 10, nullable: true),
                    PostalCode = table.Column<string>(unicode: false, maxLength: 20, nullable: true),
                    Complement = table.Column<string>(unicode: false, maxLength: 200, nullable: true),
                    Latitude = table.Column<decimal>(type: "decimal(10,6)", nullable: false),
                    Longitue = table.Column<decimal>(type: "decimal(10,6)", nullable: false),
                    IsActive = table.Column<bool>(nullable: false),
                    CreationTime = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getutcdate())"),
                    CreatedUserId = table.Column<Guid>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    LastModificationTime = table.Column<DateTime>(type: "datetime", nullable: true),
                    LastModificationUserId = table.Column<Guid>(nullable: true),
                    DeletionTime = table.Column<DateTime>(type: "datetime", nullable: true),
                    DeletedUserId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Location", x => x.LocationId);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(nullable: true),
                    CreationTime = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getutcdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ScheduleItem",
                columns: table => new
                {
                    ScheduleItemId = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ScheduleItemUid = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(unicode: false, maxLength: 300, nullable: false),
                    IsActive = table.Column<bool>(nullable: false),
                    CreationTime = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getutcdate())"),
                    CreatedUserId = table.Column<Guid>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    LastModificationTime = table.Column<DateTime>(type: "datetime", nullable: true),
                    LastModificationUserId = table.Column<Guid>(nullable: true),
                    DeletionTime = table.Column<DateTime>(type: "datetime", nullable: true),
                    DeletedUserId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ScheduleItem", x => x.ScheduleItemId);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    UserName = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(maxLength: 256, nullable: true),
                    Email = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(nullable: false),
                    PasswordHash = table.Column<string>(nullable: true),
                    SecurityStamp = table.Column<string>(nullable: true),
                    ConcurrencyStamp = table.Column<string>(nullable: true),
                    PhoneNumber = table.Column<string>(nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(nullable: false),
                    TwoFactorEnabled = table.Column<bool>(nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(nullable: true),
                    LockoutEnabled = table.Column<bool>(nullable: false),
                    AccessFailedCount = table.Column<int>(nullable: false),
                    Name = table.Column<string>(unicode: false, maxLength: 500, nullable: false),
                    UserTypeId = table.Column<int>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false),
                    CreationTime = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getutcdate())"),
                    CreatedUserId = table.Column<Guid>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    LastModificationTime = table.Column<DateTime>(type: "datetime", nullable: true),
                    LastModificationUserId = table.Column<Guid>(nullable: true),
                    DeletionTime = table.Column<DateTime>(type: "datetime", nullable: true),
                    DeletedUserId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    RoleId = table.Column<Guid>(nullable: false),
                    ClaimType = table.Column<string>(nullable: true),
                    ClaimValue = table.Column<string>(nullable: true),
                    CreationTime = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getutcdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RoleClaims_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Client",
                columns: table => new
                {
                    ClientId = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ClientUid = table.Column<Guid>(nullable: false),
                    UserId = table.Column<Guid>(nullable: false),
                    ProfileTypeId = table.Column<int>(nullable: false),
                    Name = table.Column<string>(unicode: false, maxLength: 500, nullable: false),
                    Email = table.Column<string>(unicode: false, maxLength: 200, nullable: false),
                    PhoneNumber = table.Column<string>(unicode: false, maxLength: 20, nullable: false),
                    LocationId = table.Column<long>(nullable: false),
                    DocumentInformation = table.Column<string>(nullable: true),
                    DocumentTypeId = table.Column<int>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false),
                    CreationTime = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getutcdate())"),
                    CreatedUserId = table.Column<Guid>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    LastModificationTime = table.Column<DateTime>(type: "datetime", nullable: true),
                    LastModificationUserId = table.Column<Guid>(nullable: true),
                    DeletionTime = table.Column<DateTime>(type: "datetime", nullable: true),
                    DeletedUserId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Client", x => x.ClientId);
                    table.ForeignKey(
                        name: "FK_Client_Location",
                        column: x => x.LocationId,
                        principalTable: "Location",
                        principalColumn: "LocationId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Client_User",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    UserId = table.Column<Guid>(nullable: false),
                    ClaimType = table.Column<string>(nullable: true),
                    ClaimValue = table.Column<string>(nullable: true),
                    CreationTime = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getutcdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserClaims_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(maxLength: 128, nullable: false),
                    ProviderKey = table.Column<string>(maxLength: 128, nullable: false),
                    ProviderDisplayName = table.Column<string>(nullable: true),
                    UserId = table.Column<Guid>(nullable: false),
                    CreationTime = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getutcdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_UserLogins_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserRoles",
                columns: table => new
                {
                    UserId = table.Column<Guid>(nullable: false),
                    RoleId = table.Column<Guid>(nullable: false),
                    CreationTime = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getutcdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_UserRoles_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserRoles_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserTokens",
                columns: table => new
                {
                    UserId = table.Column<Guid>(nullable: false),
                    LoginProvider = table.Column<string>(maxLength: 128, nullable: false),
                    Name = table.Column<string>(maxLength: 128, nullable: false),
                    Value = table.Column<string>(nullable: true),
                    CreationTime = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getutcdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_UserTokens_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Employee",
                columns: table => new
                {
                    EmployeeId = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    EmployeeUid = table.Column<Guid>(nullable: false),
                    ClientId = table.Column<long>(nullable: false),
                    Name = table.Column<string>(unicode: false, maxLength: 250, nullable: false),
                    Email = table.Column<string>(unicode: false, maxLength: 200, nullable: true),
                    PhoneNumber = table.Column<string>(unicode: false, maxLength: 20, nullable: true),
                    IsActive = table.Column<bool>(nullable: false),
                    CreationTime = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getutcdate())"),
                    CreatedUserId = table.Column<Guid>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    LastModificationTime = table.Column<DateTime>(type: "datetime", nullable: true),
                    LastModificationUserId = table.Column<Guid>(nullable: true),
                    DeletionTime = table.Column<DateTime>(type: "datetime", nullable: true),
                    DeletedUserId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Employee", x => x.EmployeeId);
                    table.ForeignKey(
                        name: "FK_Employee_Client",
                        column: x => x.ClientId,
                        principalTable: "Client",
                        principalColumn: "ClientId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PetLover",
                columns: table => new
                {
                    PetLoverId = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    PetLoverUid = table.Column<Guid>(nullable: false),
                    UserId = table.Column<Guid>(nullable: true),
                    ClientId = table.Column<long>(nullable: true),
                    Name = table.Column<string>(unicode: false, maxLength: 500, nullable: false),
                    Email = table.Column<string>(unicode: false, maxLength: 200, nullable: true),
                    PhoneNumber = table.Column<string>(unicode: false, maxLength: 20, nullable: true),
                    LocationId = table.Column<long>(nullable: false),
                    Gender = table.Column<int>(nullable: false),
                    IsClub = table.Column<bool>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false),
                    CreationTime = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getutcdate())"),
                    CreatedUserId = table.Column<Guid>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    LastModificationTime = table.Column<DateTime>(type: "datetime", nullable: true),
                    LastModificationUserId = table.Column<Guid>(nullable: true),
                    DeletionTime = table.Column<DateTime>(type: "datetime", nullable: true),
                    DeletedUserId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PetLover", x => x.PetLoverId);
                    table.ForeignKey(
                        name: "FK_PetLover_Client",
                        column: x => x.ClientId,
                        principalTable: "Client",
                        principalColumn: "ClientId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PetLover_Location",
                        column: x => x.LocationId,
                        principalTable: "Location",
                        principalColumn: "LocationId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PetLover_User",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ScheduleItemClient",
                columns: table => new
                {
                    ScheduleItemClientId = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ScheduleItemClientUid = table.Column<Guid>(nullable: false),
                    ScheduleItemId = table.Column<long>(nullable: false),
                    ClientId = table.Column<long>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false),
                    CreationTime = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getutcdate())"),
                    CreatedUserId = table.Column<Guid>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    LastModificationTime = table.Column<DateTime>(type: "datetime", nullable: true),
                    LastModificationUserId = table.Column<Guid>(nullable: true),
                    DeletionTime = table.Column<DateTime>(type: "datetime", nullable: true),
                    DeletedUserId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ScheduleItemClient", x => x.ScheduleItemClientId);
                    table.ForeignKey(
                        name: "FK_ScheduleItemClient_Client",
                        column: x => x.ClientId,
                        principalTable: "Client",
                        principalColumn: "ClientId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ScheduleItemClient_ScheduleItem",
                        column: x => x.ScheduleItemId,
                        principalTable: "ScheduleItem",
                        principalColumn: "ScheduleItemId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Schedule",
                columns: table => new
                {
                    ScheduleId = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ScheduleUid = table.Column<Guid>(nullable: false),
                    Period = table.Column<int>(nullable: true),
                    Date = table.Column<DateTime>(type: "datetime", nullable: false),
                    DateWithHour = table.Column<DateTime>(nullable: false),
                    Status = table.Column<int>(nullable: false),
                    EmployeeId = table.Column<long>(nullable: false),
                    QuantityAllowed = table.Column<int>(nullable: false),
                    QuantityOccupied = table.Column<int>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false),
                    CreationTime = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getutcdate())"),
                    CreatedUserId = table.Column<Guid>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    LastModificationTime = table.Column<DateTime>(type: "datetime", nullable: true),
                    LastModificationUserId = table.Column<Guid>(nullable: true),
                    DeletionTime = table.Column<DateTime>(type: "datetime", nullable: true),
                    DeletedUserId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Schedule", x => x.ScheduleId);
                    table.UniqueConstraint("UC_Employee_DateWithHour", x => new { x.EmployeeId, x.DateWithHour });
                    table.ForeignKey(
                        name: "FK_Schedule_Employee",
                        column: x => x.EmployeeId,
                        principalTable: "Employee",
                        principalColumn: "EmployeeId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ScheduleItemEmployee",
                columns: table => new
                {
                    ScheduleItemEmployeeId = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ScheduleItemEmployeeUid = table.Column<Guid>(nullable: false),
                    ScheduleItemId = table.Column<long>(nullable: false),
                    EmployeeId = table.Column<long>(nullable: false),
                    Name = table.Column<string>(unicode: false, maxLength: 300, nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Hour = table.Column<int>(nullable: true),
                    Minutes = table.Column<int>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false),
                    CreationTime = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getutcdate())"),
                    CreatedUserId = table.Column<Guid>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    LastModificationTime = table.Column<DateTime>(type: "datetime", nullable: true),
                    LastModificationUserId = table.Column<Guid>(nullable: true),
                    DeletionTime = table.Column<DateTime>(type: "datetime", nullable: true),
                    DeletedUserId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ScheduleItemEmployee", x => x.ScheduleItemEmployeeId);
                    table.ForeignKey(
                        name: "FK_ScheduleItemEmployee_Employee",
                        column: x => x.EmployeeId,
                        principalTable: "Employee",
                        principalColumn: "EmployeeId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ScheduleItemEmployee_ScheduleItem",
                        column: x => x.ScheduleItemId,
                        principalTable: "ScheduleItem",
                        principalColumn: "ScheduleItemId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Pet",
                columns: table => new
                {
                    PetId = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    PetUid = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(unicode: false, maxLength: 200, nullable: false),
                    PetLoverId = table.Column<long>(nullable: false),
                    PetTypeId = table.Column<int>(nullable: false),
                    Breed = table.Column<string>(unicode: false, maxLength: 100, nullable: true),
                    BirthDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    Gender = table.Column<int>(nullable: false),
                    Size = table.Column<int>(nullable: false),
                    Coat = table.Column<int>(nullable: false),
                    Note = table.Column<string>(unicode: false, maxLength: 400, nullable: true),
                    IsClub = table.Column<bool>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false),
                    CreationTime = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getutcdate())"),
                    CreatedUserId = table.Column<Guid>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    LastModificationTime = table.Column<DateTime>(type: "datetime", nullable: true),
                    LastModificationUserId = table.Column<Guid>(nullable: true),
                    DeletionTime = table.Column<DateTime>(type: "datetime", nullable: true),
                    DeletedUserId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pet", x => x.PetId);
                    table.ForeignKey(
                        name: "FK_Pet_PetLover",
                        column: x => x.PetLoverId,
                        principalTable: "PetLover",
                        principalColumn: "PetLoverId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SchedulePet",
                columns: table => new
                {
                    SchedulePetId = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    SchedulePetUid = table.Column<Guid>(nullable: false),
                    Note = table.Column<string>(unicode: false, maxLength: 500, nullable: true),
                    Status = table.Column<int>(nullable: false),
                    PetLoverId = table.Column<long>(nullable: true),
                    PetId = table.Column<long>(nullable: true),
                    ScheduleId = table.Column<long>(nullable: true),
                    ScheduleItemEmployeeId = table.Column<long>(nullable: true),
                    CreationTime = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getutcdate())"),
                    CreatedUserId = table.Column<Guid>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    LastModificationTime = table.Column<DateTime>(type: "datetime", nullable: true),
                    LastModificationUserId = table.Column<Guid>(nullable: true),
                    DeletionTime = table.Column<DateTime>(type: "datetime", nullable: true),
                    DeletedUserId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SchedulePet", x => x.SchedulePetId);
                    table.ForeignKey(
                        name: "FK_SchedulePet_Pet",
                        column: x => x.PetId,
                        principalTable: "Pet",
                        principalColumn: "PetId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SchedulePet_PetLover",
                        column: x => x.PetLoverId,
                        principalTable: "PetLover",
                        principalColumn: "PetLoverId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SchedulePet_Schedule",
                        column: x => x.ScheduleId,
                        principalTable: "Schedule",
                        principalColumn: "ScheduleId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SchedulePet_ScheduleItemEmployee",
                        column: x => x.ScheduleItemEmployeeId,
                        principalTable: "ScheduleItemEmployee",
                        principalColumn: "ScheduleItemEmployeeId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Vaccine",
                columns: table => new
                {
                    VaccineId = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    VaccineUid = table.Column<Guid>(nullable: false),
                    PetId = table.Column<long>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    Date = table.Column<DateTime>(nullable: false),
                    RevaccineDate = table.Column<DateTime>(nullable: true),
                    ClinicName = table.Column<string>(nullable: true),
                    Note = table.Column<string>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false),
                    CreationTime = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getutcdate())"),
                    CreatedUserId = table.Column<Guid>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    LastModificationTime = table.Column<DateTime>(type: "datetime", nullable: true),
                    LastModificationUserId = table.Column<Guid>(nullable: true),
                    DeletionTime = table.Column<DateTime>(type: "datetime", nullable: true),
                    DeletedUserId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vaccine", x => x.VaccineId);
                    table.ForeignKey(
                        name: "FK_Vaccine_Pet",
                        column: x => x.PetId,
                        principalTable: "Pet",
                        principalColumn: "PetId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "x_Client_LocationId",
                table: "Client",
                column: "LocationId");

            migrationBuilder.CreateIndex(
                name: "x_Client_UserId",
                table: "Client",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "x_Employee_ClientId",
                table: "Employee",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "x_Pet_PetLoverId",
                table: "Pet",
                column: "PetLoverId");

            migrationBuilder.CreateIndex(
                name: "x_PetLover_ClientId",
                table: "PetLover",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "x_PetLover_LocationId",
                table: "PetLover",
                column: "LocationId");

            migrationBuilder.CreateIndex(
                name: "x_PetLover_UserId",
                table: "PetLover",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_RoleClaims_RoleId",
                table: "RoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "Roles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "x_Schedule_EmployeeId",
                table: "Schedule",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "x_ScheduleItemClient_ClientId",
                table: "ScheduleItemClient",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "x_ScheduleItemClient_ScheduleItemId",
                table: "ScheduleItemClient",
                column: "ScheduleItemId");

            migrationBuilder.CreateIndex(
                name: "x_ScheduleItemEmployee_EmployeeId",
                table: "ScheduleItemEmployee",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "x_ScheduleItemEmployee_ScheduleItemId",
                table: "ScheduleItemEmployee",
                column: "ScheduleItemId");

            migrationBuilder.CreateIndex(
                name: "x_SchedulePet_PetId",
                table: "SchedulePet",
                column: "PetId");

            migrationBuilder.CreateIndex(
                name: "x_SchedulePet_PetLoverId",
                table: "SchedulePet",
                column: "PetLoverId");

            migrationBuilder.CreateIndex(
                name: "x_SchedulePet_ScheduleId",
                table: "SchedulePet",
                column: "ScheduleId");

            migrationBuilder.CreateIndex(
                name: "x_SchedulePet_ScheduleItemEmployeeId",
                table: "SchedulePet",
                column: "ScheduleItemEmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_UserClaims_UserId",
                table: "UserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserLogins_UserId",
                table: "UserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserRoles_RoleId",
                table: "UserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "Users",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "Users",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "x_Vaccine_PetId",
                table: "Vaccine",
                column: "PetId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RoleClaims");

            migrationBuilder.DropTable(
                name: "ScheduleItemClient");

            migrationBuilder.DropTable(
                name: "SchedulePet");

            migrationBuilder.DropTable(
                name: "UserClaims");

            migrationBuilder.DropTable(
                name: "UserLogins");

            migrationBuilder.DropTable(
                name: "UserRoles");

            migrationBuilder.DropTable(
                name: "UserTokens");

            migrationBuilder.DropTable(
                name: "Vaccine");

            migrationBuilder.DropTable(
                name: "Schedule");

            migrationBuilder.DropTable(
                name: "ScheduleItemEmployee");

            migrationBuilder.DropTable(
                name: "Roles");

            migrationBuilder.DropTable(
                name: "Pet");

            migrationBuilder.DropTable(
                name: "Employee");

            migrationBuilder.DropTable(
                name: "ScheduleItem");

            migrationBuilder.DropTable(
                name: "PetLover");

            migrationBuilder.DropTable(
                name: "Client");

            migrationBuilder.DropTable(
                name: "Location");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
