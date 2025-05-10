using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Mock.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "helped_first_name",
                table: "Messages");

            migrationBuilder.DropColumn(
                name: "helped_last_name",
                table: "Messages");

            migrationBuilder.DropColumn(
                name: "volunteer_first_name",
                table: "Messages");

            migrationBuilder.DropColumn(
                name: "volunteer_last_name",
                table: "Messages");

            migrationBuilder.AddColumn<int>(
                name: "helped_id",
                table: "Messages",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "volunteer_id",
                table: "Messages",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "areas_Of_Knowledges",
                columns: table => new
                {
                    ID_knowledge = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    describtion = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    volunteer_id = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_areas_Of_Knowledges", x => x.ID_knowledge);
                    table.ForeignKey(
                        name: "FK_areas_Of_Knowledges_Volunteers_volunteer_id",
                        column: x => x.volunteer_id,
                        principalTable: "Volunteers",
                        principalColumn: "volunteer_id");
                });

            migrationBuilder.CreateTable(
                name: "responses",
                columns: table => new
                {
                    response_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    helped_id = table.Column<int>(type: "int", nullable: false),
                    context = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    rating = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_responses", x => x.response_id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_areas_Of_Knowledges_volunteer_id",
                table: "areas_Of_Knowledges",
                column: "volunteer_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "areas_Of_Knowledges");

            migrationBuilder.DropTable(
                name: "responses");

            migrationBuilder.DropColumn(
                name: "helped_id",
                table: "Messages");

            migrationBuilder.DropColumn(
                name: "volunteer_id",
                table: "Messages");

            migrationBuilder.AddColumn<string>(
                name: "helped_first_name",
                table: "Messages",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "helped_last_name",
                table: "Messages",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "volunteer_first_name",
                table: "Messages",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "volunteer_last_name",
                table: "Messages",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
