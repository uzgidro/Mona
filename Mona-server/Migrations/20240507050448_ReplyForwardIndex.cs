using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Mona.Migrations
{
    /// <inheritdoc />
    public partial class ReplyForwardIndex : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Messages_ForwardId",
                table: "Messages");

            migrationBuilder.DropIndex(
                name: "IX_Messages_ReplyId",
                table: "Messages");

            migrationBuilder.CreateIndex(
                name: "IX_Messages_ForwardId",
                table: "Messages",
                column: "ForwardId");

            migrationBuilder.CreateIndex(
                name: "IX_Messages_ReplyId",
                table: "Messages",
                column: "ReplyId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Messages_ForwardId",
                table: "Messages");

            migrationBuilder.DropIndex(
                name: "IX_Messages_ReplyId",
                table: "Messages");

            migrationBuilder.CreateIndex(
                name: "IX_Messages_ForwardId",
                table: "Messages",
                column: "ForwardId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Messages_ReplyId",
                table: "Messages",
                column: "ReplyId",
                unique: true);
        }
    }
}
