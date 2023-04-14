using Microsoft.EntityFrameworkCore.Migrations;

namespace DataTech.System.Versioning.Migrations
{
    public partial class second : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Deleted",
                table: "AppUsers",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Deleted",
                table: "AppSystems",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Deleted",
                table: "AppModules",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateIndex(
                name: "IX_AppModules_AppSystemId",
                table: "AppModules",
                column: "AppSystemId");

            migrationBuilder.AddForeignKey(
                name: "FK_AppModules_AppSystems_AppSystemId",
                table: "AppModules",
                column: "AppSystemId",
                principalTable: "AppSystems",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AppModules_AppSystems_AppSystemId",
                table: "AppModules");

            migrationBuilder.DropIndex(
                name: "IX_AppModules_AppSystemId",
                table: "AppModules");

            migrationBuilder.DropColumn(
                name: "Deleted",
                table: "AppUsers");

            migrationBuilder.DropColumn(
                name: "Deleted",
                table: "AppSystems");

            migrationBuilder.DropColumn(
                name: "Deleted",
                table: "AppModules");
        }
    }
}
