using Microsoft.EntityFrameworkCore.Migrations;

namespace EFCore_sample.Migrations
{
    public partial class ItemGrade : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ItemGrade",
                table: "Item",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ItemGrade",
                table: "Item");
        }
    }
}
