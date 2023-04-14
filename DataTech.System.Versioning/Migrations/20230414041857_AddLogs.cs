using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DataTech.System.Versioning.Migrations
{
    public partial class AddLogs : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AppModuleLogs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AppModuleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    VersionIndex = table.Column<int>(type: "int", nullable: false),
                    UpdateIndex = table.Column<int>(type: "int", nullable: false),
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

            migrationBuilder.CreateIndex(
                name: "IX_AppModuleLogs_AppModuleId",
                table: "AppModuleLogs",
                column: "AppModuleId");

            migrationBuilder.CreateIndex(
                name: "IX_AppSystemLogs_AppSystemId",
                table: "AppSystemLogs",
                column: "AppSystemId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AppModuleLogs");

            migrationBuilder.DropTable(
                name: "AppSystemLogs");
        }
    }
}
