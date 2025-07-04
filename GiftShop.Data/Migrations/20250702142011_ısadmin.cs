using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GiftShop.Data.Migrations
{
    /// <inheritdoc />
    public partial class ısadmin : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<bool>(
                name: "IsTopMenu",
                table: "Categories",
                type: "bit",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsAdmin",
                table: "AppUsers",
                type: "bit",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "AppUsers",
                keyColumn: "AppUserID",
                keyValue: 1,
                column: "IsAdmin",
                value: null);

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "CategoryID",
                keyValue: 1,
                column: "IsTopMenu",
                value: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsAdmin",
                table: "AppUsers");

            migrationBuilder.AlterColumn<bool>(
                name: "IsTopMenu",
                table: "Categories",
                type: "bit",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "CategoryID",
                keyValue: 1,
                column: "IsTopMenu",
                value: null);
        }
    }
}
