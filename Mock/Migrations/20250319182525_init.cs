using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Mock.Migrations
{
    /// <inheritdoc />
    public partial class init : Migration 
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Helpeds",
                columns: table => new
                {
                    helpedid = table.Column<int>(name: "helped_id", type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    helpedfirstname = table.Column<string>(name: "helped_first_name", type: "nvarchar(max)", nullable: false),
                    helpedlastname = table.Column<string>(name: "helped_last_name", type: "nvarchar(max)", nullable: true),
                    tel = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    location = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Helpeds", x => x.helpedid);
                });

            migrationBuilder.CreateTable(
                name: "Messages",
                columns: table => new
                {
                    messageid = table.Column<int>(name: "message_id", type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    volunteerfirstname = table.Column<string>(name: "volunteer_first_name", type: "nvarchar(max)", nullable: false),
                    volunteerlastname = table.Column<string>(name: "volunteer_last_name", type: "nvarchar(max)", nullable: true),
                    helpedfirstname = table.Column<string>(name: "helped_first_name", type: "nvarchar(max)", nullable: false),
                    helpedlastname = table.Column<string>(name: "helped_last_name", type: "nvarchar(max)", nullable: true),
                    isDone = table.Column<bool>(type: "bit", nullable: false),
                    description = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Messages", x => x.messageid);
                });

            migrationBuilder.CreateTable(
                name: "Volunteers",
                columns: table => new
                {
                    volunteerid = table.Column<int>(name: "volunteer_id", type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    volunteerfirstname = table.Column<string>(name: "volunteer_first_name", type: "nvarchar(max)", nullable: false),
                    volunteerlastname = table.Column<string>(name: "volunteer_last_name", type: "nvarchar(max)", nullable: true),
                    location = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    starttime = table.Column<TimeSpan>(name: "start_time", type: "time", nullable: false),
                    endtime = table.Column<TimeSpan>(name: "end_time", type: "time", nullable: false),
                    tel = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    email = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Volunteers", x => x.volunteerid);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Helpeds");

            migrationBuilder.DropTable(
                name: "Messages");

            migrationBuilder.DropTable(
                name: "Volunteers");
        }
    }
}
