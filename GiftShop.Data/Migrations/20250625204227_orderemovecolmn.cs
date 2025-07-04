using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GiftShop.Data.Migrations
{
    /// <inheritdoc />
    public partial class orderemovecolmn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreateDate",
                table: "Orders");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreateDate",
                table: "Orders",
                type: "datetime2",
                nullable: true,
                defaultValueSql: "GETDATE()");
        }
    }
}
