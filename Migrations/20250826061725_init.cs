using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Kakeibo.Migrations
{
    /// <inheritdoc />
    public partial class init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsExpense",
                table: "Transactions");

            migrationBuilder.AddColumn<bool>(
                name: "IsExpense",
                table: "Categorys",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsExpense",
                table: "Categorys");

            migrationBuilder.AddColumn<bool>(
                name: "IsExpense",
                table: "Transactions",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
