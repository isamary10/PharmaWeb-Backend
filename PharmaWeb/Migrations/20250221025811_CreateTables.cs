using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PharmaWeb.Migrations
{
    /// <inheritdoc />
    public partial class CreateTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Tb_Pharma_Medicine",
                columns: table => new
                {
                    MedicineId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,6)", precision: 18, scale: 6, nullable: false),
                    StockQuantity = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tb_Pharma_Medicine", x => x.MedicineId);
                });

            migrationBuilder.CreateTable(
                name: "Tb_Pharma_RawMaterial",
                columns: table => new
                {
                    RawMaterialId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Supplier = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    StockQuantity = table.Column<int>(type: "int", nullable: false),
                    ExpirationDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tb_Pharma_RawMaterial", x => x.RawMaterialId);
                });

            migrationBuilder.CreateTable(
                name: "Tb_Pharma_MedicineRawMaterial",
                columns: table => new
                {
                    MedicineId = table.Column<int>(type: "int", nullable: false),
                    RawMaterialId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tb_Pharma_MedicineRawMaterial", x => new { x.MedicineId, x.RawMaterialId });
                    table.ForeignKey(
                        name: "FK_Tb_Pharma_MedicineRawMaterial_Tb_Pharma_Medicine_MedicineId",
                        column: x => x.MedicineId,
                        principalTable: "Tb_Pharma_Medicine",
                        principalColumn: "MedicineId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Tb_Pharma_MedicineRawMaterial_Tb_Pharma_RawMaterial_RawMaterialId",
                        column: x => x.RawMaterialId,
                        principalTable: "Tb_Pharma_RawMaterial",
                        principalColumn: "RawMaterialId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Tb_Pharma_MedicineRawMaterial_RawMaterialId",
                table: "Tb_Pharma_MedicineRawMaterial",
                column: "RawMaterialId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Tb_Pharma_MedicineRawMaterial");

            migrationBuilder.DropTable(
                name: "Tb_Pharma_Medicine");

            migrationBuilder.DropTable(
                name: "Tb_Pharma_RawMaterial");
        }
    }
}
