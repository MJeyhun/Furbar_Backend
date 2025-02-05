using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Furbar.Migrations
{
    /// <inheritdoc />
    public partial class deleteSocialMediaTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_SocialMedias_SocialMediaId",
                table: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "SocialMedias");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_SocialMediaId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "SocialMediaId",
                table: "AspNetUsers");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SocialMediaId",
                table: "AspNetUsers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "SocialMedias",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AppUserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Facebook = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Instagram = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    Twitter = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SocialMedias", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SocialMedias_AspNetUsers_AppUserId",
                        column: x => x.AppUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_SocialMediaId",
                table: "AspNetUsers",
                column: "SocialMediaId");

            migrationBuilder.CreateIndex(
                name: "IX_SocialMedias_AppUserId",
                table: "SocialMedias",
                column: "AppUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_SocialMedias_SocialMediaId",
                table: "AspNetUsers",
                column: "SocialMediaId",
                principalTable: "SocialMedias",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
