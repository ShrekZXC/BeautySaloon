using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BeautySaloon.Migrations
{
    /// <inheritdoc />
    public partial class UpdateScheduleAddClientAndService : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ClientId",
                table: "WorkSchedule",
                type: "char(36)",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                collation: "ascii_general_ci");

            migrationBuilder.AddColumn<Guid>(
                name: "ServiceId",
                table: "WorkSchedule",
                type: "char(36)",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                collation: "ascii_general_ci");

            migrationBuilder.CreateIndex(
                name: "IX_WorkSchedule_ClientId",
                table: "WorkSchedule",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkSchedule_ServiceId",
                table: "WorkSchedule",
                column: "ServiceId");

            migrationBuilder.AddForeignKey(
                name: "FK_WorkSchedule_AspNetUsers_ClientId",
                table: "WorkSchedule",
                column: "ClientId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_WorkSchedule_Service_ServiceId",
                table: "WorkSchedule",
                column: "ServiceId",
                principalTable: "Service",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WorkSchedule_AspNetUsers_ClientId",
                table: "WorkSchedule");

            migrationBuilder.DropForeignKey(
                name: "FK_WorkSchedule_Service_ServiceId",
                table: "WorkSchedule");

            migrationBuilder.DropIndex(
                name: "IX_WorkSchedule_ClientId",
                table: "WorkSchedule");

            migrationBuilder.DropIndex(
                name: "IX_WorkSchedule_ServiceId",
                table: "WorkSchedule");

            migrationBuilder.DropColumn(
                name: "ClientId",
                table: "WorkSchedule");

            migrationBuilder.DropColumn(
                name: "ServiceId",
                table: "WorkSchedule");
        }
    }
}
