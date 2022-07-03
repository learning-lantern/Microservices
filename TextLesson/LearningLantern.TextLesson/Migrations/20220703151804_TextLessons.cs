using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LearningLantern.TextLesson.Migrations
{
    public partial class TextLessons : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TextLessons",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BlobName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Path = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TextLessons", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TextLessonQuiz",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    QuizId = table.Column<int>(type: "int", nullable: false),
                    Time = table.Column<int>(type: "int", nullable: false),
                    TextLessonModelId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TextLessonQuiz", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TextLessonQuiz_TextLessons_TextLessonModelId",
                        column: x => x.TextLessonModelId,
                        principalTable: "TextLessons",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_TextLessonQuiz_TextLessonModelId",
                table: "TextLessonQuiz",
                column: "TextLessonModelId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TextLessonQuiz");

            migrationBuilder.DropTable(
                name: "TextLessons");
        }
    }
}
