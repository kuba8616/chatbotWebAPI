using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ChatbotAI.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddIsCanceledToChatMessage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsCancelled",
                table: "ChatResponses",
                type: "bit",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsCancelled",
                table: "ChatResponses");
        }
    }
}
