using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Mona.Migrations
{
    /// <inheritdoc />
    public partial class AddChatClient : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddForeignKey(
                name: "FK_ChatClients_AspNetUsers_ClientId",
                table: "ChatClients",
                column: "ClientId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ChatClients_AspNetUsers_ClientId",
                table: "ChatClients");
        }
    }
}
