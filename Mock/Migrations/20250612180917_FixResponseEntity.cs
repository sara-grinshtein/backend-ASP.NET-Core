using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Mock.Migrations
{
    /// <inheritdoc />
    public partial class FixResponseEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "helped_id1",
                table: "responses",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "isPublic",
                table: "responses",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "message_id1",
                table: "responses",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_responses_helped_id1",
                table: "responses",
                column: "helped_id1");

            migrationBuilder.CreateIndex(
                name: "IX_responses_message_id1",
                table: "responses",
                column: "message_id1");

            migrationBuilder.AddForeignKey(
                name: "FK_responses_Helpeds_helped_id1",
                table: "responses",
                column: "helped_id1",
                principalTable: "Helpeds",
                principalColumn: "helped_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_responses_Messages_message_id1",
                table: "responses",
                column: "message_id1",
                principalTable: "Messages",
                principalColumn: "message_id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_responses_Helpeds_helped_id1",
                table: "responses");

            migrationBuilder.DropForeignKey(
                name: "FK_responses_Messages_message_id1",
                table: "responses");

            migrationBuilder.DropIndex(
                name: "IX_responses_helped_id1",
                table: "responses");

            migrationBuilder.DropIndex(
                name: "IX_responses_message_id1",
                table: "responses");

            migrationBuilder.DropColumn(
                name: "helped_id1",
                table: "responses");

            migrationBuilder.DropColumn(
                name: "isPublic",
                table: "responses");

            migrationBuilder.DropColumn(
                name: "message_id1",
                table: "responses");
        }
    }
}
