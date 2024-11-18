using Microsoft.EntityFrameworkCore.Migrations;

namespace RemindWebApp.Migrations
{
    public partial class remtest : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Test",
                table: "Orders");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Test",
                table: "Orders",
                nullable: true);
        }
    }
}
