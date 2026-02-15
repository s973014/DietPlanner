using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class SubstitutionToMeal : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_substitutions_Products_ProductId",
                table: "substitutions");

            migrationBuilder.DropForeignKey(
                name: "FK_substitutions_Products_SubstituteId",
                table: "substitutions");

            migrationBuilder.RenameColumn(
                name: "SubstituteId",
                table: "substitutions",
                newName: "SubstituteMealId");

            migrationBuilder.RenameColumn(
                name: "ProductId",
                table: "substitutions",
                newName: "OriginalMealId");

            migrationBuilder.RenameIndex(
                name: "IX_substitutions_SubstituteId",
                table: "substitutions",
                newName: "IX_substitutions_SubstituteMealId");

            migrationBuilder.RenameIndex(
                name: "IX_substitutions_ProductId",
                table: "substitutions",
                newName: "IX_substitutions_OriginalMealId");

            migrationBuilder.AddForeignKey(
                name: "FK_substitutions_meals_OriginalMealId",
                table: "substitutions",
                column: "OriginalMealId",
                principalTable: "meals",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_substitutions_meals_SubstituteMealId",
                table: "substitutions",
                column: "SubstituteMealId",
                principalTable: "meals",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_substitutions_meals_OriginalMealId",
                table: "substitutions");

            migrationBuilder.DropForeignKey(
                name: "FK_substitutions_meals_SubstituteMealId",
                table: "substitutions");

            migrationBuilder.RenameColumn(
                name: "SubstituteMealId",
                table: "substitutions",
                newName: "SubstituteId");

            migrationBuilder.RenameColumn(
                name: "OriginalMealId",
                table: "substitutions",
                newName: "ProductId");

            migrationBuilder.RenameIndex(
                name: "IX_substitutions_SubstituteMealId",
                table: "substitutions",
                newName: "IX_substitutions_SubstituteId");

            migrationBuilder.RenameIndex(
                name: "IX_substitutions_OriginalMealId",
                table: "substitutions",
                newName: "IX_substitutions_ProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_substitutions_Products_ProductId",
                table: "substitutions",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_substitutions_Products_SubstituteId",
                table: "substitutions",
                column: "SubstituteId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
