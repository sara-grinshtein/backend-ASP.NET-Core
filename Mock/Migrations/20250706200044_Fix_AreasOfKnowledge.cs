using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Mock.Migrations
{
    /// <inheritdoc />
    public partial class Fix_AreasOfKnowledge : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_responses_Messages_message_id",
                table: "responses");

            migrationBuilder.DropPrimaryKey(
                name: "PK_responses",
                table: "responses");

            migrationBuilder.RenameTable(
                name: "responses",
                newName: "Response");

            migrationBuilder.RenameIndex(
                name: "IX_responses_message_id",
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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Response_Messages_message_id",
                table: "Response");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Response",
                table: "Response");

            migrationBuilder.RenameTable(
                name: "Response",
                newName: "responses");

            migrationBuilder.RenameIndex(
                name: "IX_Response_message_id",
                table: "responses",
                newName: "IX_responses_message_id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_responses",
                table: "responses",
                column: "response_id");

            migrationBuilder.AddForeignKey(
                name: "FK_responses_Messages_message_id",
                table: "responses",
                column: "message_id",
                principalTable: "Messages",
                principalColumn: "message_id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
