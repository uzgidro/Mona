using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Mona.Migrations
{
    /// <inheritdoc />
    public partial class Chat : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Messages_AspNetUsers_SenderId",
                table: "Messages");

            migrationBuilder.AddColumn<string>(
                name: "ChatId",
                table: "Messages",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserModelId",
                table: "Messages",
                type: "TEXT",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Chats",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Chats", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ChatClients",
                columns: table => new
                {
                    ClientId = table.Column<string>(type: "TEXT", nullable: false),
                    ChatId = table.Column<string>(type: "TEXT", nullable: false),
                    UserModelId = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChatClients", x => new { x.ClientId, x.ChatId });
                    table.ForeignKey(
                        name: "FK_ChatClients_AspNetUsers_UserModelId",
                        column: x => x.UserModelId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ChatClients_Chats_ChatId",
                        column: x => x.ChatId,
                        principalTable: "Chats",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Messages_ChatId",
                table: "Messages",
                column: "ChatId");

            migrationBuilder.CreateIndex(
                name: "IX_Messages_UserModelId",
                table: "Messages",
                column: "UserModelId");

            migrationBuilder.CreateIndex(
                name: "IX_ChatClients_ChatId",
                table: "ChatClients",
                column: "ChatId");

            migrationBuilder.CreateIndex(
                name: "IX_ChatClients_UserModelId",
                table: "ChatClients",
                column: "UserModelId");

            migrationBuilder.AddForeignKey(
                name: "FK_Messages_AspNetUsers_SenderId",
                table: "Messages",
                column: "SenderId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Messages_AspNetUsers_UserModelId",
                table: "Messages",
                column: "UserModelId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Messages_Chats_ChatId",
                table: "Messages",
                column: "ChatId",
                principalTable: "Chats",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Messages_AspNetUsers_SenderId",
                table: "Messages");

            migrationBuilder.DropForeignKey(
                name: "FK_Messages_AspNetUsers_UserModelId",
                table: "Messages");

            migrationBuilder.DropForeignKey(
                name: "FK_Messages_Chats_ChatId",
                table: "Messages");

            migrationBuilder.DropTable(
                name: "ChatClients");

            migrationBuilder.DropTable(
                name: "Chats");

            migrationBuilder.DropIndex(
                name: "IX_Messages_ChatId",
                table: "Messages");

            migrationBuilder.DropIndex(
                name: "IX_Messages_UserModelId",
                table: "Messages");

            migrationBuilder.DropColumn(
                name: "ChatId",
                table: "Messages");

            migrationBuilder.DropColumn(
                name: "UserModelId",
                table: "Messages");

            migrationBuilder.AddForeignKey(
                name: "FK_Messages_AspNetUsers_SenderId",
                table: "Messages",
                column: "SenderId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
