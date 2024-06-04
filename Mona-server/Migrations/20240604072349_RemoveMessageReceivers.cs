using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Mona.Migrations
{
    /// <inheritdoc />
    public partial class RemoveMessageReceivers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Messages_AspNetUsers_DirectReceiverId",
                table: "Messages");

            migrationBuilder.DropForeignKey(
                name: "FK_Messages_Chats_ChatId",
                table: "Messages");

            migrationBuilder.DropForeignKey(
                name: "FK_Messages_Groups_GroupReceiverId",
                table: "Messages");

            migrationBuilder.DropIndex(
                name: "IX_Messages_DirectReceiverId",
                table: "Messages");

            migrationBuilder.DropIndex(
                name: "IX_Messages_GroupReceiverId",
                table: "Messages");

            migrationBuilder.DropColumn(
                name: "DirectReceiverId",
                table: "Messages");

            migrationBuilder.DropColumn(
                name: "GroupReceiverId",
                table: "Messages");

            migrationBuilder.AlterColumn<string>(
                name: "ChatId",
                table: "Messages",
                type: "TEXT",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Messages_Chats_ChatId",
                table: "Messages",
                column: "ChatId",
                principalTable: "Chats",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Messages_Chats_ChatId",
                table: "Messages");

            migrationBuilder.AlterColumn<string>(
                name: "ChatId",
                table: "Messages",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT");

            migrationBuilder.AddColumn<string>(
                name: "DirectReceiverId",
                table: "Messages",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "GroupReceiverId",
                table: "Messages",
                type: "TEXT",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Messages_DirectReceiverId",
                table: "Messages",
                column: "DirectReceiverId");

            migrationBuilder.CreateIndex(
                name: "IX_Messages_GroupReceiverId",
                table: "Messages",
                column: "GroupReceiverId");

            migrationBuilder.AddForeignKey(
                name: "FK_Messages_AspNetUsers_DirectReceiverId",
                table: "Messages",
                column: "DirectReceiverId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Messages_Chats_ChatId",
                table: "Messages",
                column: "ChatId",
                principalTable: "Chats",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Messages_Groups_GroupReceiverId",
                table: "Messages",
                column: "GroupReceiverId",
                principalTable: "Groups",
                principalColumn: "Id");
        }
    }
}
