using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ToyStore.DAL.Migrations
{
    /// <inheritdoc />
    public partial class NewProduct2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductVariantAttribute_ProductAttribute_AttributeId",
                table: "ProductVariantAttribute");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductVariantAttribute_ProductVariants_VariantId",
                table: "ProductVariantAttribute");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProductVariantAttribute",
                table: "ProductVariantAttribute");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProductAttribute",
                table: "ProductAttribute");

            migrationBuilder.RenameTable(
                name: "ProductVariantAttribute",
                newName: "ProductVariantAttributes");

            migrationBuilder.RenameTable(
                name: "ProductAttribute",
                newName: "ProductAttributes");

            migrationBuilder.RenameIndex(
                name: "IX_ProductVariantAttribute_VariantId",
                table: "ProductVariantAttributes",
                newName: "IX_ProductVariantAttributes_VariantId");

            migrationBuilder.RenameIndex(
                name: "IX_ProductVariantAttribute_AttributeId",
                table: "ProductVariantAttributes",
                newName: "IX_ProductVariantAttributes_AttributeId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProductVariantAttributes",
                table: "ProductVariantAttributes",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProductAttributes",
                table: "ProductAttributes",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductVariantAttributes_ProductAttributes_AttributeId",
                table: "ProductVariantAttributes",
                column: "AttributeId",
                principalTable: "ProductAttributes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductVariantAttributes_ProductVariants_VariantId",
                table: "ProductVariantAttributes",
                column: "VariantId",
                principalTable: "ProductVariants",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductVariantAttributes_ProductAttributes_AttributeId",
                table: "ProductVariantAttributes");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductVariantAttributes_ProductVariants_VariantId",
                table: "ProductVariantAttributes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProductVariantAttributes",
                table: "ProductVariantAttributes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProductAttributes",
                table: "ProductAttributes");

            migrationBuilder.RenameTable(
                name: "ProductVariantAttributes",
                newName: "ProductVariantAttribute");

            migrationBuilder.RenameTable(
                name: "ProductAttributes",
                newName: "ProductAttribute");

            migrationBuilder.RenameIndex(
                name: "IX_ProductVariantAttributes_VariantId",
                table: "ProductVariantAttribute",
                newName: "IX_ProductVariantAttribute_VariantId");

            migrationBuilder.RenameIndex(
                name: "IX_ProductVariantAttributes_AttributeId",
                table: "ProductVariantAttribute",
                newName: "IX_ProductVariantAttribute_AttributeId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProductVariantAttribute",
                table: "ProductVariantAttribute",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProductAttribute",
                table: "ProductAttribute",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductVariantAttribute_ProductAttribute_AttributeId",
                table: "ProductVariantAttribute",
                column: "AttributeId",
                principalTable: "ProductAttribute",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductVariantAttribute_ProductVariants_VariantId",
                table: "ProductVariantAttribute",
                column: "VariantId",
                principalTable: "ProductVariants",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
