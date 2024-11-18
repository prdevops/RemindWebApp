using Microsoft.EntityFrameworkCore.Migrations;

namespace RemindWebApp.Migrations
{
    public partial class delcontentblog : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Content",
                table: "Blogs");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Content",
                table: "Blogs",
                maxLength: 1000,
                nullable: false,
                defaultValue: "");
        }
    }
}
