using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace RemindWebApp.Migrations
{
    public partial class CreateProductTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(maxLength: 300, nullable: false),
                    Title = table.Column<string>(maxLength: 300, nullable: false),
                    Description = table.Column<string>(maxLength: 5000, nullable: false),
                    Price = table.Column<double>(nullable: false),
                    Count = table.Column<int>(nullable: false),
                    Image = table.Column<string>(maxLength: 255, nullable: false),
                    DiscountProduct = table.Column<int>(nullable: false),
                    ProductDedline = table.Column<DateTime>(nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    Active = table.Column<string>(nullable: true),
                    CategoryMarkaId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Products_CategoryMarkas_CategoryMarkaId",
                        column: x => x.CategoryMarkaId,
                        principalTable: "CategoryMarkas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Products_CategoryMarkaId",
                table: "Products",
                column: "CategoryMarkaId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Products");
        }
    }
}
