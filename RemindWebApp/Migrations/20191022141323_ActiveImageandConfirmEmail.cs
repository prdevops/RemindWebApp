using Microsoft.EntityFrameworkCore.Migrations;

namespace RemindWebApp.Migrations
{
    public partial class ActiveImageandConfirmEmail : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Active",
                table: "Images",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "EmaileConfirm",
                table: "AspNetUsers",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Active",
                table: "Images");

            migrationBuilder.DropColumn(
                name: "EmaileConfirm",
                table: "AspNetUsers");
        }
    }
}
