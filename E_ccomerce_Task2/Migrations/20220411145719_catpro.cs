using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace E_ccomerce_Task2.Migrations
{
    public partial class catpro : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_productCategories_Categories_categorieId",
                table: "productCategories");

            migrationBuilder.DropColumn(
                name: "CategoryId",
                table: "productCategories");

            migrationBuilder.RenameColumn(
                name: "categorieId",
                table: "productCategories",
                newName: "CategorieId");

            migrationBuilder.RenameIndex(
                name: "IX_productCategories_categorieId",
                table: "productCategories",
                newName: "IX_productCategories_CategorieId");

            migrationBuilder.AddForeignKey(
                name: "FK_productCategories_Categories_CategorieId",
                table: "productCategories",
                column: "CategorieId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_productCategories_Categories_CategorieId",
                table: "productCategories");

            migrationBuilder.RenameColumn(
                name: "CategorieId",
                table: "productCategories",
                newName: "categorieId");

            migrationBuilder.RenameIndex(
                name: "IX_productCategories_CategorieId",
                table: "productCategories",
                newName: "IX_productCategories_categorieId");

            migrationBuilder.AddColumn<int>(
                name: "CategoryId",
                table: "productCategories",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddForeignKey(
                name: "FK_productCategories_Categories_categorieId",
                table: "productCategories",
                column: "categorieId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
