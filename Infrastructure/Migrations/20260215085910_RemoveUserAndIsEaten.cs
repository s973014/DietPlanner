using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RemoveUserAndIsEaten : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_weekly_plans_users_UserId",
                table: "weekly_plans");

            migrationBuilder.DropIndex(
                name: "IX_weekly_plans_UserId",
                table: "weekly_plans");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "weekly_plans");

            migrationBuilder.DropColumn(
                name: "IsEaten",
                table: "daily_meals");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "UserId",
                table: "weekly_plans",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<bool>(
                name: "IsEaten",
                table: "daily_meals",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateIndex(
                name: "IX_weekly_plans_UserId",
                table: "weekly_plans",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_weekly_plans_users_UserId",
                table: "weekly_plans",
                column: "UserId",
                principalTable: "users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
