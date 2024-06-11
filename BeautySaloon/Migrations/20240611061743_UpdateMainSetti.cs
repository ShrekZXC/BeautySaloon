using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BeautySaloon.Migrations
{
    /// <inheritdoc />
    public partial class UpdateMainSetti : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ColorBackground",
                table: "MainSettings",
                newName: "ColorBackgroundMain");

            migrationBuilder.AddColumn<string>(
                name: "ColorBackgroundFooter",
                table: "MainSettings",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ColorBackgroundFooter",
                table: "MainSettings");

            migrationBuilder.RenameColumn(
                name: "ColorBackgroundMain",
                table: "MainSettings",
                newName: "ColorBackground");
        }
    }
}
