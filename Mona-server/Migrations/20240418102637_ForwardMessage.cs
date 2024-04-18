using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Mona.Migrations
{
    /// <inheritdoc />
    public partial class ForwardMessage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsForwarded",
                table: "Messages");

            migrationBuilder.AddColumn<string>(
                name: "ForwardId",
                table: "Messages",
                type: "TEXT",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Messages_ForwardId",
                table: "Messages",
                column: "ForwardId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Messages_Messages_ForwardId",
                table: "Messages",
                column: "ForwardId",
                principalTable: "Messages",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Messages_Messages_ForwardId",
                table: "Messages");

            migrationBuilder.DropIndex(
                name: "IX_Messages_ForwardId",
                table: "Messages");

            migrationBuilder.DropColumn(
                name: "ForwardId",
                table: "Messages");

            migrationBuilder.AddColumn<bool>(
                name: "IsForwarded",
                table: "Messages",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);
        }
    }
}
