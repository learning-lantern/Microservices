using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LearningLantern.TextLesson.Migrations
{
    public partial class v2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Path",
                table: "TextLessons",
                newName: "Title");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Title",
                table: "TextLessons",
                newName: "Path");
        }
    }
}
