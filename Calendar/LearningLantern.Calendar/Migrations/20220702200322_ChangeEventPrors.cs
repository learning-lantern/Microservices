using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LearningLantern.Calendar.Migrations
{
    public partial class ChangeEventPrors : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "StartTime",
                table: "Events",
                newName: "StartDate");

            migrationBuilder.RenameColumn(
                name: "EndTime",
                table: "Events",
                newName: "DueDate");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "StartDate",
                table: "Events",
                newName: "StartTime");

            migrationBuilder.RenameColumn(
                name: "DueDate",
                table: "Events",
                newName: "EndTime");
        }
    }
}
