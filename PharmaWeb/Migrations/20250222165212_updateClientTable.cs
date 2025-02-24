using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PharmaWeb.Migrations
{
    /// <inheritdoc />
    public partial class updateClientTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Celphone",
                table: "Tb_Pharma_Client",
                newName: "Cellphone");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Cellphone",
                table: "Tb_Pharma_Client",
                newName: "Celphone");
        }
    }
}
