using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApiTIenda.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Lots",
                columns: table => new
                {
                    IdLot = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CodeLote = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    DateOfAdmission = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ExpDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Lots", x => x.IdLot);
                });

            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    IdProduct = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Category = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.IdProduct);
                });

            migrationBuilder.CreateTable(
                name: "Suppliers",
                columns: table => new
                {
                    IdSuPlier = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Contact = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Address = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Suppliers", x => x.IdSuPlier);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    IdUser = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Password = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Rol = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.IdUser);
                });

            migrationBuilder.CreateTable(
                name: "ProductSuppliers",
                columns: table => new
                {
                    IdProduct = table.Column<int>(type: "int", nullable: false),
                    IdSupplier = table.Column<int>(type: "int", nullable: false),
                    IdLot = table.Column<int>(type: "int", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    Stock = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductSuppliers", x => new { x.IdProduct, x.IdSupplier, x.IdLot });
                    table.ForeignKey(
                        name: "FK_ProductSuppliers_Lots_IdLot",
                        column: x => x.IdLot,
                        principalTable: "Lots",
                        principalColumn: "IdLot",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductSuppliers_Products_IdProduct",
                        column: x => x.IdProduct,
                        principalTable: "Products",
                        principalColumn: "IdProduct",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductSuppliers_Suppliers_IdSupplier",
                        column: x => x.IdSupplier,
                        principalTable: "Suppliers",
                        principalColumn: "IdSuPlier",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OrderRs",
                columns: table => new
                {
                    IdOrder = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdUser = table.Column<int>(type: "int", nullable: false),
                    OrderDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    State = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Total = table.Column<decimal>(type: "decimal(10,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderRs", x => x.IdOrder);
                    table.ForeignKey(
                        name: "FK_OrderRs_Users_IdUser",
                        column: x => x.IdUser,
                        principalTable: "Users",
                        principalColumn: "IdUser",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WishLists",
                columns: table => new
                {
                    IdList = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdUser = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WishLists", x => x.IdList);
                    table.ForeignKey(
                        name: "FK_WishLists_Users_IdUser",
                        column: x => x.IdUser,
                        principalTable: "Users",
                        principalColumn: "IdUser",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OrderRDetails",
                columns: table => new
                {
                    IdOrder = table.Column<int>(type: "int", nullable: false),
                    IdProduct = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    UnitPrice = table.Column<decimal>(type: "decimal(10,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderRDetails", x => new { x.IdOrder, x.IdProduct });
                    table.ForeignKey(
                        name: "FK_OrderRDetails_OrderRs_IdOrder",
                        column: x => x.IdOrder,
                        principalTable: "OrderRs",
                        principalColumn: "IdOrder",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OrderRDetails_Products_IdProduct",
                        column: x => x.IdProduct,
                        principalTable: "Products",
                        principalColumn: "IdProduct",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WishListDetails",
                columns: table => new
                {
                    IdList = table.Column<int>(type: "int", nullable: false),
                    IdProduct = table.Column<int>(type: "int", nullable: false),
                    DateAdded = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WishListDetails", x => new { x.IdList, x.IdProduct });
                    table.ForeignKey(
                        name: "FK_WishListDetails_Products_IdProduct",
                        column: x => x.IdProduct,
                        principalTable: "Products",
                        principalColumn: "IdProduct",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_WishListDetails_WishLists_IdList",
                        column: x => x.IdList,
                        principalTable: "WishLists",
                        principalColumn: "IdList",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_OrderRDetails_IdProduct",
                table: "OrderRDetails",
                column: "IdProduct");

            migrationBuilder.CreateIndex(
                name: "IX_OrderRs_IdUser",
                table: "OrderRs",
                column: "IdUser");

            migrationBuilder.CreateIndex(
                name: "IX_ProductSuppliers_IdLot",
                table: "ProductSuppliers",
                column: "IdLot");

            migrationBuilder.CreateIndex(
                name: "IX_ProductSuppliers_IdSupplier",
                table: "ProductSuppliers",
                column: "IdSupplier");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Email",
                table: "Users",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_WishListDetails_IdProduct",
                table: "WishListDetails",
                column: "IdProduct");

            migrationBuilder.CreateIndex(
                name: "IX_WishLists_IdUser",
                table: "WishLists",
                column: "IdUser");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OrderRDetails");

            migrationBuilder.DropTable(
                name: "ProductSuppliers");

            migrationBuilder.DropTable(
                name: "WishListDetails");

            migrationBuilder.DropTable(
                name: "OrderRs");

            migrationBuilder.DropTable(
                name: "Lots");

            migrationBuilder.DropTable(
                name: "Suppliers");

            migrationBuilder.DropTable(
                name: "Products");

            migrationBuilder.DropTable(
                name: "WishLists");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
