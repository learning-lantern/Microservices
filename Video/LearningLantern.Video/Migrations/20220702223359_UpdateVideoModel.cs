using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LearningLantern.Video.Migrations
{
    public partial class UpdateVideoModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ClassroomId",
                table: "Videos");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "Videos");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Videos",
                newName: "Path");

            migrationBuilder.CreateTable(
                name: "VideoQuiz",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    QuizId = table.Column<int>(type: "int", nullable: false),
                    Time = table.Column<int>(type: "int", nullable: false),
                    VideoModelId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VideoQuiz", x => x.Id);
                    table.ForeignKey(
                        name: "FK_VideoQuiz_Videos_VideoModelId",
                        column: x => x.VideoModelId,
                        principalTable: "Videos",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_VideoQuiz_VideoModelId",
                table: "VideoQuiz",
                column: "VideoModelId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "VideoQuiz");

            migrationBuilder.RenameColumn(
                name: "Path",
                table: "Videos",
                newName: "UserId");

            migrationBuilder.AddColumn<int>(
                name: "ClassroomId",
                table: "Videos",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Videos",
                type: "nvarchar(450)",
                maxLength: 450,
                nullable: false,
                defaultValue: "");
        }
    }
}
