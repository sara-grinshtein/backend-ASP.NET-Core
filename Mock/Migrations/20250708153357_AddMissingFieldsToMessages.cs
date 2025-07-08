using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Mock.Migrations
{
    /// <inheritdoc />
    public partial class AddMissingFieldsToMessages : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Response_Messages_message_id",
                table: "Response");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Response",
                table: "Response");

            migrationBuilder.RenameTable(
                name: "Response",
                newName: "Responses");

            migrationBuilder.RenameIndex(
                name: "IX_Response_message_id",
                table: "Responses",
                newName: "IX_Responses_message_id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Responses",
                table: "Responses",
                column: "response_id");

            migrationBuilder.AddForeignKey(
                name: "FK_Responses_Messages_message_id",
                table: "Responses",
                column: "message_id",
                principalTable: "Messages",
                principalColumn: "message_id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Responses_Messages_message_id",
                table: "Responses");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Responses",
                table: "Responses");

            migrationBuilder.RenameTable(
                name: "Responses",
                newName: "Response");

            migrationBuilder.RenameIndex(
                name: "IX_Responses_message_id",
                table: "Response",
                newName: "IX_Response_message_id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Response",
                table: "Response",
                column: "response_id");

            migrationBuilder.AddForeignKey(
                name: "FK_Response_Messages_message_id",
                table: "Response",
                column: "message_id",
                principalTable: "Messages",
                principalColumn: "message_id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
