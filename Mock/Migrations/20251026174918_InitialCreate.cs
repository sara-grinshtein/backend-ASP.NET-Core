using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Mock.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Helpeds",
                columns: table => new
                {
                    helped_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    password = table.Column<string>(type: "text", nullable: false),
                    helped_first_name = table.Column<string>(type: "text", nullable: false),
                    helped_last_name = table.Column<string>(type: "text", nullable: true),
                    tel = table.Column<string>(type: "text", nullable: true),
                    email = table.Column<string>(type: "text", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    Latitude = table.Column<double>(type: "double precision", nullable: true),
                    Longitude = table.Column<double>(type: "double precision", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Helpeds", x => x.helped_id);
                });

            migrationBuilder.CreateTable(
                name: "KnowledgeCategories",
                columns: table => new
                {
                    ID_knowledge = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    describtion = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KnowledgeCategories", x => x.ID_knowledge);
                });

            migrationBuilder.CreateTable(
                name: "Volunteers",
                columns: table => new
                {
                    volunteer_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    password = table.Column<string>(type: "text", nullable: false),
                    volunteer_first_name = table.Column<string>(type: "text", nullable: false),
                    volunteer_last_name = table.Column<string>(type: "text", nullable: true),
                    start_time = table.Column<TimeSpan>(type: "interval", nullable: true),
                    end_time = table.Column<TimeSpan>(type: "interval", nullable: true),
                    tel = table.Column<string>(type: "text", nullable: true),
                    email = table.Column<string>(type: "text", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    Latitude = table.Column<double>(type: "double precision", nullable: true),
                    Longitude = table.Column<double>(type: "double precision", nullable: true),
                    assignment_count = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Volunteers", x => x.volunteer_id);
                });

            migrationBuilder.CreateTable(
                name: "areas_Of_Knowledges",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    volunteer_id = table.Column<int>(type: "integer", nullable: false),
                    ID_knowledge = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_areas_Of_Knowledges", x => x.Id);
                    table.ForeignKey(
                        name: "FK_areas_Of_Knowledges_KnowledgeCategories_ID_knowledge",
                        column: x => x.ID_knowledge,
                        principalTable: "KnowledgeCategories",
                        principalColumn: "ID_knowledge",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_areas_Of_Knowledges_Volunteers_volunteer_id",
                        column: x => x.volunteer_id,
                        principalTable: "Volunteers",
                        principalColumn: "volunteer_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Messages",
                columns: table => new
                {
                    message_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    volunteer_id = table.Column<int>(type: "integer", nullable: true),
                    helped_id = table.Column<int>(type: "integer", nullable: false),
                    isDone = table.Column<bool>(type: "boolean", nullable: false),
                    description = table.Column<string>(type: "text", nullable: false),
                    hasResponse = table.Column<bool>(type: "boolean", nullable: false),
                    ConfirmArrival = table.Column<bool>(type: "boolean", nullable: true),
                    Latitude = table.Column<double>(type: "double precision", nullable: true),
                    Longitude = table.Column<double>(type: "double precision", nullable: true),
                    date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    title = table.Column<string>(type: "text", nullable: true),
                    location = table.Column<string>(type: "text", nullable: true),
                    phone = table.Column<string>(type: "text", nullable: true)
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
                name: "Responses",
                columns: table => new
                {
                    response_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    message_id = table.Column<int>(type: "integer", nullable: false),
                    context = table.Column<string>(type: "text", nullable: false),
                    rating = table.Column<int>(type: "integer", nullable: false),
                    isPublic = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Responses", x => x.response_id);
                    table.ForeignKey(
                        name: "FK_Responses_Messages_message_id",
                        column: x => x.message_id,
                        principalTable: "Messages",
                        principalColumn: "message_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_areas_Of_Knowledges_ID_knowledge",
                table: "areas_Of_Knowledges",
                column: "ID_knowledge");

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
                name: "IX_Responses_message_id",
                table: "Responses",
                column: "message_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "areas_Of_Knowledges");

            migrationBuilder.DropTable(
                name: "Responses");

            migrationBuilder.DropTable(
                name: "KnowledgeCategories");

            migrationBuilder.DropTable(
                name: "Messages");

            migrationBuilder.DropTable(
                name: "Helpeds");

            migrationBuilder.DropTable(
                name: "Volunteers");
        }
    }
}
