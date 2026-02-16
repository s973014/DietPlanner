using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddWeeklyPlanRelationToDailyMeal : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_daily_meals_WeeklyPlans_WeeklyPlanId",
                table: "daily_meals");

            migrationBuilder.DropPrimaryKey(
                name: "PK_WeeklyPlans",
                table: "WeeklyPlans");

            migrationBuilder.RenameTable(
                name: "WeeklyPlans",
                newName: "weekly_plans");

            migrationBuilder.AddPrimaryKey(
                name: "PK_weekly_plans",
                table: "weekly_plans",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_daily_meals_weekly_plans_WeeklyPlanId",
                table: "daily_meals",
                column: "WeeklyPlanId",
                principalTable: "weekly_plans",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_daily_meals_weekly_plans_WeeklyPlanId",
                table: "daily_meals");

            migrationBuilder.DropPrimaryKey(
                name: "PK_weekly_plans",
                table: "weekly_plans");

            migrationBuilder.RenameTable(
                name: "weekly_plans",
                newName: "WeeklyPlans");

            migrationBuilder.AddPrimaryKey(
                name: "PK_WeeklyPlans",
                table: "WeeklyPlans",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_daily_meals_WeeklyPlans_WeeklyPlanId",
                table: "daily_meals",
                column: "WeeklyPlanId",
                principalTable: "WeeklyPlans",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
