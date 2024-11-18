using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace RemindWebApp.Migrations
{
    public partial class Blog : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "HeaderBlogs");

            migrationBuilder.DropColumn(
                name: "By",
                table: "Blogs");

            migrationBuilder.AddColumn<string>(
                name: "HeaderBig",
                table: "Blogs",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "HeaderSmall",
                table: "Blogs",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HeaderBig",
                table: "Blogs");

            migrationBuilder.DropColumn(
                name: "HeaderSmall",
                table: "Blogs");

            migrationBuilder.AddColumn<string>(
                name: "By",
                table: "Blogs",
                maxLength: 300,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "HeaderBlogs",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ImagePath = table.Column<string>(maxLength: 300, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HeaderBlogs", x => x.Id);
                });
        }
    }
}
