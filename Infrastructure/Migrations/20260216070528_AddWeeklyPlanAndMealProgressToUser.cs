using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddWeeklyPlanAndMealProgressToUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "WeeklyPlanId",
                table: "users",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "daily_meal_progress",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    DailyMealId = table.Column<Guid>(type: "uuid", nullable: false),
                    IsEaten = table.Column<bool>(type: "boolean", nullable: false),
                    ReplacementMealId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_daily_meal_progress", x => x.Id);
                    table.ForeignKey(
                        name: "FK_daily_meal_progress_daily_meals_DailyMealId",
                        column: x => x.DailyMealId,
                        principalTable: "daily_meals",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_daily_meal_progress_meals_ReplacementMealId",
                        column: x => x.ReplacementMealId,
                        principalTable: "meals",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_daily_meal_progress_users_UserId",
                        column: x => x.UserId,
                        principalTable: "users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_users_WeeklyPlanId",
                table: "users",
                column: "WeeklyPlanId");

            migrationBuilder.CreateIndex(
                name: "IX_daily_meal_progress_DailyMealId",
                table: "daily_meal_progress",
                column: "DailyMealId");

            migrationBuilder.CreateIndex(
                name: "IX_daily_meal_progress_ReplacementMealId",
                table: "daily_meal_progress",
                column: "ReplacementMealId");

            migrationBuilder.CreateIndex(
                name: "IX_daily_meal_progress_UserId",
                table: "daily_meal_progress",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_users_weekly_plans_WeeklyPlanId",
                table: "users",
                column: "WeeklyPlanId",
                principalTable: "weekly_plans",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_users_weekly_plans_WeeklyPlanId",
                table: "users");

            migrationBuilder.DropTable(
                name: "daily_meal_progress");

            migrationBuilder.DropIndex(
                name: "IX_users_WeeklyPlanId",
                table: "users");

            migrationBuilder.DropColumn(
                name: "WeeklyPlanId",
                table: "users");
        }
    }
}
