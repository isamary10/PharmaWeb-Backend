using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PharmaWeb.Migrations
{
    /// <inheritdoc />
    public partial class updateOrderTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Quantity",
                table: "Tb_Pharma_Order");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Quantity",
                table: "Tb_Pharma_Order",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
