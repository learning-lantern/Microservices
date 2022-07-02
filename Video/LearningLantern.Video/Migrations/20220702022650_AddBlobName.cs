using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LearningLantern.Video.Migrations
{
    public partial class AddBlobName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "BlobName",
                table: "Videos",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BlobName",
                table: "Videos");
        }
    }
}
