using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Mona.Migrations
{
    /// <inheritdoc />
    public partial class MessageDeleteFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IsDeleted",
                table: "Messages",
                newName: "IsSenderDeleted");

            migrationBuilder.AddColumn<bool>(
                name: "IsReceiverDeleted",
                table: "Messages",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsReceiverDeleted",
                table: "Messages");

            migrationBuilder.RenameColumn(
                name: "IsSenderDeleted",
                table: "Messages",
                newName: "IsDeleted");
        }
    }
}
