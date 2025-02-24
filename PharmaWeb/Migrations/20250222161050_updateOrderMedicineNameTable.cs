using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PharmaWeb.Migrations
{
    /// <inheritdoc />
    public partial class updateOrderMedicineNameTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tb_Pharma_OrderMerdicine_Tb_Pharma_Medicine_MedicineId",
                table: "Tb_Pharma_OrderMerdicine");

            migrationBuilder.DropForeignKey(
                name: "FK_Tb_Pharma_OrderMerdicine_Tb_Pharma_Order_OrderId",
                table: "Tb_Pharma_OrderMerdicine");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Tb_Pharma_OrderMerdicine",
                table: "Tb_Pharma_OrderMerdicine");

            migrationBuilder.RenameTable(
                name: "Tb_Pharma_OrderMerdicine",
                newName: "Tb_Pharma_OrderMedicine");

            migrationBuilder.RenameIndex(
                name: "IX_Tb_Pharma_OrderMerdicine_MedicineId",
                table: "Tb_Pharma_OrderMedicine",
                newName: "IX_Tb_Pharma_OrderMedicine_MedicineId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Tb_Pharma_OrderMedicine",
                table: "Tb_Pharma_OrderMedicine",
                columns: new[] { "OrderId", "MedicineId" });

            migrationBuilder.AddForeignKey(
                name: "FK_Tb_Pharma_OrderMedicine_Tb_Pharma_Medicine_MedicineId",
                table: "Tb_Pharma_OrderMedicine",
                column: "MedicineId",
                principalTable: "Tb_Pharma_Medicine",
                principalColumn: "MedicineId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Tb_Pharma_OrderMedicine_Tb_Pharma_Order_OrderId",
                table: "Tb_Pharma_OrderMedicine",
                column: "OrderId",
                principalTable: "Tb_Pharma_Order",
                principalColumn: "OrderId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tb_Pharma_OrderMedicine_Tb_Pharma_Medicine_MedicineId",
                table: "Tb_Pharma_OrderMedicine");

            migrationBuilder.DropForeignKey(
                name: "FK_Tb_Pharma_OrderMedicine_Tb_Pharma_Order_OrderId",
                table: "Tb_Pharma_OrderMedicine");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Tb_Pharma_OrderMedicine",
                table: "Tb_Pharma_OrderMedicine");

            migrationBuilder.RenameTable(
                name: "Tb_Pharma_OrderMedicine",
                newName: "Tb_Pharma_OrderMerdicine");

            migrationBuilder.RenameIndex(
                name: "IX_Tb_Pharma_OrderMedicine_MedicineId",
                table: "Tb_Pharma_OrderMerdicine",
                newName: "IX_Tb_Pharma_OrderMerdicine_MedicineId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Tb_Pharma_OrderMerdicine",
                table: "Tb_Pharma_OrderMerdicine",
                columns: new[] { "OrderId", "MedicineId" });

            migrationBuilder.AddForeignKey(
                name: "FK_Tb_Pharma_OrderMerdicine_Tb_Pharma_Medicine_MedicineId",
                table: "Tb_Pharma_OrderMerdicine",
                column: "MedicineId",
                principalTable: "Tb_Pharma_Medicine",
                principalColumn: "MedicineId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Tb_Pharma_OrderMerdicine_Tb_Pharma_Order_OrderId",
                table: "Tb_Pharma_OrderMerdicine",
                column: "OrderId",
                principalTable: "Tb_Pharma_Order",
                principalColumn: "OrderId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
