using Microsoft.EntityFrameworkCore.Migrations;

namespace Rmit.Asr.Application.Migrations
{
    public partial class AddIdToRoom : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "RoomId",
                table: "Room",
                newName: "Id");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Room",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Room_Name",
                table: "Room",
                column: "Name",
                unique: true,
                filter: "[Name] IS NOT NULL");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Room_Name",
                table: "Room");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "Room");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Room",
                newName: "RoomId");
        }
    }
}
