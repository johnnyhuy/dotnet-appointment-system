using Microsoft.EntityFrameworkCore.Migrations;

namespace Rmit.Asr.Application.Migrations
{
    public partial class UpdatedModelProperties : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Slot_Room_RoomID",
                table: "Slot");

            migrationBuilder.DropForeignKey(
                name: "FK_Slot_AspNetUsers_StaffID",
                table: "Slot");

            migrationBuilder.DropForeignKey(
                name: "FK_Slot_AspNetUsers_StudentID",
                table: "Slot");

            migrationBuilder.RenameColumn(
                name: "StudentID",
                table: "Slot",
                newName: "StudentId");

            migrationBuilder.RenameColumn(
                name: "StaffID",
                table: "Slot",
                newName: "StaffId");

            migrationBuilder.RenameColumn(
                name: "RoomID",
                table: "Slot",
                newName: "RoomId");

            migrationBuilder.RenameIndex(
                name: "IX_Slot_StudentID",
                table: "Slot",
                newName: "IX_Slot_StudentId");

            migrationBuilder.RenameIndex(
                name: "IX_Slot_StaffID",
                table: "Slot",
                newName: "IX_Slot_StaffId");

            migrationBuilder.RenameColumn(
                name: "RoomID",
                table: "Room",
                newName: "RoomId");

            migrationBuilder.AddForeignKey(
                name: "FK_Slot_Room_RoomId",
                table: "Slot",
                column: "RoomId",
                principalTable: "Room",
                principalColumn: "RoomId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Slot_AspNetUsers_StaffId",
                table: "Slot",
                column: "StaffId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Slot_AspNetUsers_StudentId",
                table: "Slot",
                column: "StudentId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Slot_Room_RoomId",
                table: "Slot");

            migrationBuilder.DropForeignKey(
                name: "FK_Slot_AspNetUsers_StaffId",
                table: "Slot");

            migrationBuilder.DropForeignKey(
                name: "FK_Slot_AspNetUsers_StudentId",
                table: "Slot");

            migrationBuilder.RenameColumn(
                name: "StudentId",
                table: "Slot",
                newName: "StudentID");

            migrationBuilder.RenameColumn(
                name: "StaffId",
                table: "Slot",
                newName: "StaffID");

            migrationBuilder.RenameColumn(
                name: "RoomId",
                table: "Slot",
                newName: "RoomID");

            migrationBuilder.RenameIndex(
                name: "IX_Slot_StudentId",
                table: "Slot",
                newName: "IX_Slot_StudentID");

            migrationBuilder.RenameIndex(
                name: "IX_Slot_StaffId",
                table: "Slot",
                newName: "IX_Slot_StaffID");

            migrationBuilder.RenameColumn(
                name: "RoomId",
                table: "Room",
                newName: "RoomID");

            migrationBuilder.AddForeignKey(
                name: "FK_Slot_Room_RoomID",
                table: "Slot",
                column: "RoomID",
                principalTable: "Room",
                principalColumn: "RoomID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Slot_AspNetUsers_StaffID",
                table: "Slot",
                column: "StaffID",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Slot_AspNetUsers_StudentID",
                table: "Slot",
                column: "StudentID",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
