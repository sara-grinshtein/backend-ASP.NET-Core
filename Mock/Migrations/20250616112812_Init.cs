using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Mock.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Helpeds",
                columns: table => new
                {
                    helped_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    helped_first_name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    helped_last_name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    tel = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    Latitude = table.Column<double>(type: "float", nullable: true),
                    Longitude = table.Column<double>(type: "float", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Helpeds", x => x.helped_id);
                });

            migrationBuilder.CreateTable(
                name: "Volunteers",
                columns: table => new
                {
                    volunteer_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    volunteer_first_name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    volunteer_last_name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    start_time = table.Column<TimeSpan>(type: "time", nullable: true),
                    end_time = table.Column<TimeSpan>(type: "time", nullable: true),
                    tel = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    Latitude = table.Column<double>(type: "float", nullable: true),
                    Longitude = table.Column<double>(type: "float", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Volunteers", x => x.volunteer_id);
                });

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
                name: "Messages",
                columns: table => new
                {
                    message_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    volunteer_id = table.Column<int>(type: "int", nullable: true),
                    helped_id = table.Column<int>(type: "int", nullable: false),
                    isDone = table.Column<bool>(type: "bit", nullable: false),
                    description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    hasResponse = table.Column<bool>(type: "bit", nullable: false),
                    ConfirmArrival = table.Column<bool>(type: "bit", nullable: true),
                    Latitude = table.Column<double>(type: "float", nullable: true),
                    Longitude = table.Column<double>(type: "float", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Messages", x => x.message_id);
                    table.ForeignKey(
                        name: "FK_Messages_Helpeds_helped_id",
                        column: x => x.helped_id,
                        principalTable: "Helpeds",
                        principalColumn: "helped_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Messages_Volunteers_volunteer_id",
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
                    message_id = table.Column<int>(type: "int", nullable: false),
                    context = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    rating = table.Column<int>(type: "int", nullable: false),
                    isPublic = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_responses", x => x.response_id);
                    table.ForeignKey(
                        name: "FK_responses_Messages_message_id",
                        column: x => x.message_id,
                        principalTable: "Messages",
                        principalColumn: "message_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_areas_Of_Knowledges_volunteer_id",
                table: "areas_Of_Knowledges",
                column: "volunteer_id");

            migrationBuilder.CreateIndex(
                name: "IX_Messages_helped_id",
                table: "Messages",
                column: "helped_id");

            migrationBuilder.CreateIndex(
                name: "IX_Messages_volunteer_id",
                table: "Messages",
                column: "volunteer_id");

            migrationBuilder.CreateIndex(
                name: "IX_responses_message_id",
                table: "responses",
                column: "message_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "areas_Of_Knowledges");

            migrationBuilder.DropTable(
                name: "responses");

            migrationBuilder.DropTable(
                name: "Messages");

            migrationBuilder.DropTable(
                name: "Helpeds");

            migrationBuilder.DropTable(
                name: "Volunteers");
        }
    }
}
