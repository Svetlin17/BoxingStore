using Microsoft.EntityFrameworkCore.Migrations;

namespace BoxingStore.Data.Migrations
{
    public partial class AddedConvertedNamePropertyToProductsTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ConvertedName",
                table: "Products",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ConvertedName",
                table: "Products");
        }
    }
}
