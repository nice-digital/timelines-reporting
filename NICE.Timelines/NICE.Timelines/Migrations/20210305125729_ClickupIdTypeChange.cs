using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace NICE.Timelines.Migrations
{
    public partial class ClickupIdTypeChange : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TimelineTask",
                columns: table => new
                {
                    TimelineTaskId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ACID = table.Column<int>(type: "int", nullable: false),
                    ClickUpId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateTypeId = table.Column<int>(type: "int", nullable: false),
                    DateTypeDescription = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    DueDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ActualDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TimelineTask", x => x.TimelineTaskId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TimelineTask");
        }
    }
}
