using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
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
                name: "products",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    calories = table.Column<float>(type: "real", nullable: false),
                    proteins = table.Column<float>(type: "real", nullable: false),
                    fats = table.Column<float>(type: "real", nullable: false),
                    carbs = table.Column<float>(type: "real", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_products", x => x.Id);
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
                    Goal = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_users", x => x.Id);
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
                        name: "FK_meal_products_meals_MealId",
                        column: x => x.MealId,
                        principalTable: "meals",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_meal_products_products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "substitutions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ProductId = table.Column<Guid>(type: "uuid", nullable: false),
                    SubstituteId = table.Column<Guid>(type: "uuid", nullable: false),
                    Reason = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_substitutions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_substitutions_products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_substitutions_products_SubstituteId",
                        column: x => x.SubstituteId,
                        principalTable: "products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
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

            migrationBuilder.CreateTable(
                name: "weekly_plans",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IsCompleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_weekly_plans", x => x.Id);
                    table.ForeignKey(
                        name: "FK_weekly_plans_users_UserId",
                        column: x => x.UserId,
                        principalTable: "users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "daily_meals",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    DayIndex = table.Column<int>(type: "integer", nullable: false),
                    MealType = table.Column<string>(type: "text", nullable: false),
                    MealId = table.Column<Guid>(type: "uuid", nullable: false),
                    IsEaten = table.Column<bool>(type: "boolean", nullable: false),
                    WeeklyPlanId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_daily_meals", x => x.Id);
                    table.ForeignKey(
                        name: "FK_daily_meals_meals_MealId",
                        column: x => x.MealId,
                        principalTable: "meals",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_daily_meals_weekly_plans_WeeklyPlanId",
                        column: x => x.WeeklyPlanId,
                        principalTable: "weekly_plans",
                        principalColumn: "Id");
                });

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
                name: "IX_substitutions_ProductId",
                table: "substitutions",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_substitutions_SubstituteId",
                table: "substitutions",
                column: "SubstituteId");

            migrationBuilder.CreateIndex(
                name: "IX_user_allergies_UserId",
                table: "user_allergies",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_weekly_plans_UserId",
                table: "weekly_plans",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "daily_meals");

            migrationBuilder.DropTable(
                name: "meal_products");

            migrationBuilder.DropTable(
                name: "substitutions");

            migrationBuilder.DropTable(
                name: "user_allergies");

            migrationBuilder.DropTable(
                name: "weekly_plans");

            migrationBuilder.DropTable(
                name: "meals");

            migrationBuilder.DropTable(
                name: "products");

            migrationBuilder.DropTable(
                name: "allergies");

            migrationBuilder.DropTable(
                name: "users");
        }
    }
}
