using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GiftShop.Data.Migrations
{
    /// <inheritdoc />
    public partial class categoryparentadd : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsTopMenu",
                table: "Categories",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ParentCategoryID",
                table: "Categories",
                type: "int",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "CategoryID",
                keyValue: 1,
                columns: new[] { "IsTopMenu", "ParentCategoryID" },
                values: new object[] { null, null });

            migrationBuilder.CreateIndex(
                name: "IX_Categories_ParentCategoryID",
                table: "Categories",
                column: "ParentCategoryID");

            migrationBuilder.AddForeignKey(
                name: "FK_Categories_Categories_ParentCategoryID",
                table: "Categories",
                column: "ParentCategoryID",
                principalTable: "Categories",
                principalColumn: "CategoryID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Categories_Categories_ParentCategoryID",
                table: "Categories");

            migrationBuilder.DropIndex(
                name: "IX_Categories_ParentCategoryID",
                table: "Categories");

            migrationBuilder.DropColumn(
                name: "IsTopMenu",
                table: "Categories");

            migrationBuilder.DropColumn(
                name: "ParentCategoryID",
                table: "Categories");
        }
    }
}
