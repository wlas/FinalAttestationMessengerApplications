using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LibraryApiMessenger.Migrations
{
    /// <inheritdoc />
    public partial class Initialnew : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Messages_RecipientEmail",
                table: "Messages");

            migrationBuilder.DropIndex(
                name: "IX_Messages_SenderEmail",
                table: "Messages");

            migrationBuilder.CreateIndex(
                name: "IX_Messages_RecipientEmail",
                table: "Messages",
                column: "RecipientEmail");

            migrationBuilder.CreateIndex(
                name: "IX_Messages_SenderEmail",
                table: "Messages",
                column: "SenderEmail");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Messages_RecipientEmail",
                table: "Messages");

            migrationBuilder.DropIndex(
                name: "IX_Messages_SenderEmail",
                table: "Messages");

            migrationBuilder.CreateIndex(
                name: "IX_Messages_RecipientEmail",
                table: "Messages",
                column: "RecipientEmail",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Messages_SenderEmail",
                table: "Messages",
                column: "SenderEmail",
                unique: true);
        }
    }
}
