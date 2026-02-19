using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class DockerInit : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "allergies",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_allergies", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "meals",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_meals", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "weekly_plans",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_weekly_plans", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Calories = table.Column<float>(type: "real", nullable: false),
                    Proteins = table.Column<float>(type: "real", nullable: false),
                    Fats = table.Column<float>(type: "real", nullable: false),
                    Carbs = table.Column<float>(type: "real", nullable: false),
                    AllergyId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Products_allergies_AllergyId",
                        column: x => x.AllergyId,
                        principalTable: "allergies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "substitutions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    OriginalMealId = table.Column<Guid>(type: "uuid", nullable: false),
                    SubstituteMealId = table.Column<Guid>(type: "uuid", nullable: false),
                    Reason = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_substitutions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_substitutions_meals_OriginalMealId",
                        column: x => x.OriginalMealId,
                        principalTable: "meals",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_substitutions_meals_SubstituteMealId",
                        column: x => x.SubstituteMealId,
                        principalTable: "meals",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "daily_meals",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    DayIndex = table.Column<int>(type: "integer", nullable: false),
                    MealType = table.Column<string>(type: "text", nullable: false),
                    MealId = table.Column<Guid>(type: "uuid", nullable: false),
                    WeeklyPlanId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_daily_meals", x => x.Id);
                    table.ForeignKey(
                        name: "FK_daily_meals_meals_MealId",
                        column: x => x.MealId,
                        principalTable: "meals",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_daily_meals_weekly_plans_WeeklyPlanId",
                        column: x => x.WeeklyPlanId,
                        principalTable: "weekly_plans",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    PasswordHash = table.Column<string>(type: "text", nullable: false),
                    Role = table.Column<string>(type: "text", nullable: false),
                    Weight = table.Column<float>(type: "real", nullable: true),
                    Height = table.Column<float>(type: "real", nullable: true),
                    ActivityLevel = table.Column<string>(type: "text", nullable: true),
                    Goal = table.Column<string>(type: "text", nullable: true),
                    WeeklyPlanId = table.Column<Guid>(type: "uuid", nullable: true),
                    CurrentPlanDay = table.Column<int>(type: "integer", nullable: false, defaultValue: 0)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_users", x => x.Id);
                    table.ForeignKey(
                        name: "FK_users_weekly_plans_WeeklyPlanId",
                        column: x => x.WeeklyPlanId,
                        principalTable: "weekly_plans",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "meal_products",
                columns: table => new
                {
                    MealId = table.Column<Guid>(type: "uuid", nullable: false),
                    ProductId = table.Column<Guid>(type: "uuid", nullable: false),
                    AmountInGrams = table.Column<float>(type: "real", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_meal_products", x => new { x.MealId, x.ProductId });
                    table.ForeignKey(
                        name: "FK_meal_products_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_meal_products_meals_MealId",
                        column: x => x.MealId,
                        principalTable: "meals",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

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

            migrationBuilder.CreateTable(
                name: "user_allergies",
                columns: table => new
                {
                    AllergiesId = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_user_allergies", x => new { x.AllergiesId, x.UserId });
                    table.ForeignKey(
                        name: "FK_user_allergies_allergies_AllergiesId",
                        column: x => x.AllergiesId,
                        principalTable: "allergies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_user_allergies_users_UserId",
                        column: x => x.UserId,
                        principalTable: "users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

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

            migrationBuilder.CreateIndex(
                name: "IX_daily_meals_MealId",
                table: "daily_meals",
                column: "MealId");

            migrationBuilder.CreateIndex(
                name: "IX_daily_meals_WeeklyPlanId",
                table: "daily_meals",
                column: "WeeklyPlanId");

            migrationBuilder.CreateIndex(
                name: "IX_meal_products_ProductId",
                table: "meal_products",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_AllergyId",
                table: "Products",
                column: "AllergyId");

            migrationBuilder.CreateIndex(
                name: "IX_substitutions_OriginalMealId",
                table: "substitutions",
                column: "OriginalMealId");

            migrationBuilder.CreateIndex(
                name: "IX_substitutions_SubstituteMealId",
                table: "substitutions",
                column: "SubstituteMealId");

            migrationBuilder.CreateIndex(
                name: "IX_user_allergies_UserId",
                table: "user_allergies",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_users_WeeklyPlanId",
                table: "users",
                column: "WeeklyPlanId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "daily_meal_progress");

            migrationBuilder.DropTable(
                name: "meal_products");

            migrationBuilder.DropTable(
                name: "substitutions");

            migrationBuilder.DropTable(
                name: "user_allergies");

            migrationBuilder.DropTable(
                name: "daily_meals");

            migrationBuilder.DropTable(
                name: "Products");

            migrationBuilder.DropTable(
                name: "users");

            migrationBuilder.DropTable(
                name: "meals");

            migrationBuilder.DropTable(
                name: "allergies");

            migrationBuilder.DropTable(
                name: "weekly_plans");
        }
    }
}
