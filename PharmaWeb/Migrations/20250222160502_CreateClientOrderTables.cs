using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PharmaWeb.Migrations
{
    /// <inheritdoc />
    public partial class CreateClientOrderTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Tb_Pharma_Client",
                columns: table => new
                {
                    ClientId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Cpf = table.Column<string>(type: "nvarchar(11)", maxLength: 11, nullable: false),
                    Address = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Celphone = table.Column<string>(type: "nvarchar(11)", maxLength: 11, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tb_Pharma_Client", x => x.ClientId);
                });

            migrationBuilder.CreateTable(
                name: "Tb_Pharma_Order",
                columns: table => new
                {
                    OrderId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    OrderDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    OrderTotal = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    ClientId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tb_Pharma_Order", x => x.OrderId);
                    table.ForeignKey(
                        name: "FK_Tb_Pharma_Order_Tb_Pharma_Client_ClientId",
                        column: x => x.ClientId,
                        principalTable: "Tb_Pharma_Client",
                        principalColumn: "ClientId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Tb_Pharma_OrderMerdicine",
                columns: table => new
                {
                    OrderId = table.Column<int>(type: "int", nullable: false),
                    MedicineId = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tb_Pharma_OrderMerdicine", x => new { x.OrderId, x.MedicineId });
                    table.ForeignKey(
                        name: "FK_Tb_Pharma_OrderMerdicine_Tb_Pharma_Medicine_MedicineId",
                        column: x => x.MedicineId,
                        principalTable: "Tb_Pharma_Medicine",
                        principalColumn: "MedicineId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Tb_Pharma_OrderMerdicine_Tb_Pharma_Order_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Tb_Pharma_Order",
                        principalColumn: "OrderId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Tb_Pharma_Order_ClientId",
                table: "Tb_Pharma_Order",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_Tb_Pharma_OrderMerdicine_MedicineId",
                table: "Tb_Pharma_OrderMerdicine",
                column: "MedicineId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Tb_Pharma_OrderMerdicine");

            migrationBuilder.DropTable(
                name: "Tb_Pharma_Order");

            migrationBuilder.DropTable(
                name: "Tb_Pharma_Client");
        }
    }
}
