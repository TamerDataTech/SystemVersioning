using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DataTech.System.Versioning.Migrations
{
    public partial class Init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AppIndexers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AppSystemId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AppModuleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ReleaseIndex = table.Column<int>(type: "int", nullable: false),
                    VersionIndex = table.Column<int>(type: "int", nullable: false),
                    EnhancementIndex = table.Column<int>(type: "int", nullable: false),
                    FixIndex = table.Column<int>(type: "int", nullable: false),
                    CreationTime = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    CreatorId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Deleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppIndexers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AppSystems",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ReleaseIndex = table.Column<int>(type: "int", nullable: false),
                    CreationTime = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    CreatorId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Deleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppSystems", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AppUsers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Username = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Fullname = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreationTime = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    CreatorId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Deleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AppModules",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AppSystemId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ReleaseIndex = table.Column<int>(type: "int", nullable: false),
                    VersionIndex = table.Column<int>(type: "int", nullable: false),
                    EnhancementIndex = table.Column<int>(type: "int", nullable: false),
                    FixIndex = table.Column<int>(type: "int", nullable: false),
                    CreationTime = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    CreatorId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Deleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppModules", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AppModules_AppSystems_AppSystemId",
                        column: x => x.AppSystemId,
                        principalTable: "AppSystems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AppSystemLogs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AppSystemId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ReleaseIndex = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreationTime = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    CreatorId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Deleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppSystemLogs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AppSystemLogs_AppSystems_AppSystemId",
                        column: x => x.AppSystemId,
                        principalTable: "AppSystems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AppModuleLogs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AppModuleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ReleaseIndex = table.Column<int>(type: "int", nullable: false),
                    VersionIndex = table.Column<int>(type: "int", nullable: false),
                    EnhancementIndex = table.Column<int>(type: "int", nullable: false),
                    FixIndex = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreationTime = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    CreatorId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Deleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppModuleLogs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AppModuleLogs_AppModules_AppModuleId",
                        column: x => x.AppModuleId,
                        principalTable: "AppModules",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AppModuleLogs_AppModuleId",
                table: "AppModuleLogs",
                column: "AppModuleId");

            migrationBuilder.CreateIndex(
                name: "IX_AppModules_AppSystemId",
                table: "AppModules",
                column: "AppSystemId");

            migrationBuilder.CreateIndex(
                name: "IX_AppSystemLogs_AppSystemId",
                table: "AppSystemLogs",
                column: "AppSystemId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AppIndexers");

            migrationBuilder.DropTable(
                name: "AppModuleLogs");

            migrationBuilder.DropTable(
                name: "AppSystemLogs");

            migrationBuilder.DropTable(
                name: "AppUsers");

            migrationBuilder.DropTable(
                name: "AppModules");

            migrationBuilder.DropTable(
                name: "AppSystems");
        }
    }
}
