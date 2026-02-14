using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddProductAllergyRelation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_meal_products_products_ProductId",
                table: "meal_products");

            migrationBuilder.DropForeignKey(
                name: "FK_substitutions_products_ProductId",
                table: "substitutions");

            migrationBuilder.DropForeignKey(
                name: "FK_substitutions_products_SubstituteId",
                table: "substitutions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_products",
                table: "products");

            migrationBuilder.RenameTable(
                name: "products",
                newName: "Products");

            migrationBuilder.RenameColumn(
                name: "proteins",
                table: "Products",
                newName: "Proteins");

            migrationBuilder.RenameColumn(
                name: "fats",
                table: "Products",
                newName: "Fats");

            migrationBuilder.RenameColumn(
                name: "carbs",
                table: "Products",
                newName: "Carbs");

            migrationBuilder.RenameColumn(
                name: "calories",
                table: "Products",
                newName: "Calories");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Products",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddColumn<Guid>(
                name: "AllergyId",
                table: "Products",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Products",
                table: "Products",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Products_AllergyId",
                table: "Products",
                column: "AllergyId");

            migrationBuilder.AddForeignKey(
                name: "FK_meal_products_Products_ProductId",
                table: "meal_products",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Products_allergies_AllergyId",
                table: "Products",
                column: "AllergyId",
                principalTable: "allergies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_meal_products_Products_ProductId",
                table: "meal_products");

            migrationBuilder.DropForeignKey(
                name: "FK_Products_allergies_AllergyId",
                table: "Products");

            migrationBuilder.DropForeignKey(
                name: "FK_substitutions_Products_ProductId",
                table: "substitutions");

            migrationBuilder.DropForeignKey(
                name: "FK_substitutions_Products_SubstituteId",
                table: "substitutions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Products",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_Products_AllergyId",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "AllergyId",
                table: "Products");

            migrationBuilder.RenameTable(
                name: "Products",
                newName: "products");

            migrationBuilder.RenameColumn(
                name: "Proteins",
                table: "products",
                newName: "proteins");

            migrationBuilder.RenameColumn(
                name: "Fats",
                table: "products",
                newName: "fats");

            migrationBuilder.RenameColumn(
                name: "Carbs",
                table: "products",
                newName: "carbs");

            migrationBuilder.RenameColumn(
                name: "Calories",
                table: "products",
                newName: "calories");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "products",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100);

            migrationBuilder.AddPrimaryKey(
                name: "PK_products",
                table: "products",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_meal_products_products_ProductId",
                table: "meal_products",
                column: "ProductId",
                principalTable: "products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_substitutions_products_ProductId",
                table: "substitutions",
                column: "ProductId",
                principalTable: "products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_substitutions_products_SubstituteId",
                table: "substitutions",
                column: "SubstituteId",
                principalTable: "products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
