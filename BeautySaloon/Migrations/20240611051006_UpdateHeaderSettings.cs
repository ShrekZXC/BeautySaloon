using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BeautySaloon.Migrations
{
    /// <inheritdoc />
    public partial class UpdateHeaderSettings : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ColorHeader",
                table: "HeaderSettings",
                newName: "ColorText");

            migrationBuilder.AddColumn<string>(
                name: "ColorBackground",
                table: "HeaderSettings",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ColorBackground",
                table: "HeaderSettings");

            migrationBuilder.RenameColumn(
                name: "ColorText",
                table: "HeaderSettings",
                newName: "ColorHeader");
        }
    }
}
