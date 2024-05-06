using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Mona.Migrations
{
    /// <inheritdoc />
    public partial class GroupMessaging : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Messages_AspNetUsers_ReceiverId",
                table: "Messages");

            migrationBuilder.RenameColumn(
                name: "ReceiverId",
                table: "Messages",
                newName: "GroupReceiverId");

            migrationBuilder.RenameIndex(
                name: "IX_Messages_ReceiverId",
                table: "Messages",
                newName: "IX_Messages_GroupReceiverId");

            migrationBuilder.AddColumn<string>(
                name: "DirectReceiverId",
                table: "Messages",
                type: "TEXT",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Messages_DirectReceiverId",
                table: "Messages",
                column: "DirectReceiverId");

            migrationBuilder.AddForeignKey(
                name: "FK_Messages_AspNetUsers_DirectReceiverId",
                table: "Messages",
                column: "DirectReceiverId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Messages_Groups_GroupReceiverId",
                table: "Messages",
                column: "GroupReceiverId",
                principalTable: "Groups",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Messages_AspNetUsers_DirectReceiverId",
                table: "Messages");

            migrationBuilder.DropForeignKey(
                name: "FK_Messages_Groups_GroupReceiverId",
                table: "Messages");

            migrationBuilder.DropIndex(
                name: "IX_Messages_DirectReceiverId",
                table: "Messages");

            migrationBuilder.DropColumn(
                name: "DirectReceiverId",
                table: "Messages");

            migrationBuilder.RenameColumn(
                name: "GroupReceiverId",
                table: "Messages",
                newName: "ReceiverId");

            migrationBuilder.RenameIndex(
                name: "IX_Messages_GroupReceiverId",
                table: "Messages",
                newName: "IX_Messages_ReceiverId");

            migrationBuilder.AddForeignKey(
                name: "FK_Messages_AspNetUsers_ReceiverId",
                table: "Messages",
                column: "ReceiverId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
