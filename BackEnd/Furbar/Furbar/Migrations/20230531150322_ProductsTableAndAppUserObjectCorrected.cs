using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Furbar.Migrations
{
    /// <inheritdoc />
    public partial class ProductsTableAndAppUserObjectCorrected : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Workers",
                newName: "FullName");

            migrationBuilder.AddColumn<bool>(
                name: "IsCompared",
                table: "Products",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "ImageUrl",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsCompared",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "ImageUrl",
                table: "AspNetUsers");

            migrationBuilder.RenameColumn(
                name: "FullName",
                table: "Workers",
                newName: "Name");
        }
    }
}
