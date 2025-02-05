using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Furbar.Migrations
{
    /// <inheritdoc />
    public partial class addStarReviewsColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "StarReview",
                table: "Reviews",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "StarReview",
                table: "Reviews");
        }
    }
}
