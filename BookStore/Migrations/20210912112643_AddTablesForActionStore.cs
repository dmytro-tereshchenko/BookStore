using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BookStore.Migrations
{
    public partial class AddTablesForActionStore : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropCheckConstraint(
                name: "CK_Book_YearOfPublished",
                table: "Books");

            migrationBuilder.CreateTable(
                name: "BookInStores",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BookId = table.Column<int>(type: "int", nullable: false),
                    CostPrice = table.Column<decimal>(type: "decimal(5,2)", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(5,2)", nullable: false),
                    Amount = table.Column<int>(type: "int", nullable: false),
                    DateAdded = table.Column<DateTime>(type: "date", nullable: false, defaultValueSql: "getdate()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BookInStores", x => x.Id);
                    table.CheckConstraint("CK_BookInStore_CostPrice", "[CostPrice] >= 0");
                    table.CheckConstraint("CK_BookInStore_Price", "[Price] >= 0");
                    table.CheckConstraint("CK_BookInStore_Amount", "[Amount] >= 0");
                    table.ForeignKey(
                        name: "FK_BookInStores_Books_BookId",
                        column: x => x.BookId,
                        principalTable: "Books",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Customers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    MiddleName = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    LastName = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Stocks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BookId = table.Column<int>(type: "int", nullable: false),
                    Discount = table.Column<decimal>(type: "decimal(2,2)", nullable: false),
                    DateStart = table.Column<DateTime>(type: "date", nullable: false),
                    DateEnd = table.Column<DateTime>(type: "date", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Stocks", x => x.Id);
                    table.CheckConstraint("CK_Stock_Discount", "[Discount] > 0 AND [Discount] <= 100.0");
                    table.ForeignKey(
                        name: "FK_Stocks_Books_BookId",
                        column: x => x.BookId,
                        principalTable: "Books",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BookReserves",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BookInStoreId = table.Column<int>(type: "int", nullable: false),
                    CustomerId = table.Column<int>(type: "int", nullable: false),
                    DateReserve = table.Column<DateTime>(type: "date", nullable: false, defaultValueSql: "getdate()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BookReserves", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BookReserves_BookInStores_BookInStoreId",
                        column: x => x.BookInStoreId,
                        principalTable: "BookInStores",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BookReserves_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BookSolds",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BookInStoreId = table.Column<int>(type: "int", nullable: false),
                    CustomerId = table.Column<int>(type: "int", nullable: false),
                    DateSold = table.Column<DateTime>(type: "date", nullable: false, defaultValueSql: "getdate()"),
                    SoldPrice = table.Column<decimal>(type: "decimal(5,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BookSolds", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BookSolds_BookInStores_BookInStoreId",
                        column: x => x.BookInStoreId,
                        principalTable: "BookInStores",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BookSolds_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.AddCheckConstraint(
                name: "CK_Book_YearOfPublished",
                table: "Books",
                sql: "[YearOfPublished] >= 0 AND [YearOfPublished] <= YEAR(GETDATE())");

            migrationBuilder.CreateIndex(
                name: "IX_BookInStores_BookId",
                table: "BookInStores",
                column: "BookId");

            migrationBuilder.CreateIndex(
                name: "IX_BookReserves_BookInStoreId",
                table: "BookReserves",
                column: "BookInStoreId");

            migrationBuilder.CreateIndex(
                name: "IX_BookReserves_CustomerId",
                table: "BookReserves",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_BookSolds_BookInStoreId",
                table: "BookSolds",
                column: "BookInStoreId");

            migrationBuilder.CreateIndex(
                name: "IX_BookSolds_CustomerId",
                table: "BookSolds",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_Stocks_BookId",
                table: "Stocks",
                column: "BookId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BookReserves");

            migrationBuilder.DropTable(
                name: "BookSolds");

            migrationBuilder.DropTable(
                name: "Stocks");

            migrationBuilder.DropTable(
                name: "BookInStores");

            migrationBuilder.DropTable(
                name: "Customers");

            migrationBuilder.DropCheckConstraint(
                name: "CK_Book_YearOfPublished",
                table: "Books");

            migrationBuilder.AddCheckConstraint(
                name: "CK_Book_YearOfPublished",
                table: "Books",
                sql: "[YearOfPublished] >=0 AND [YearOfPublished] <= YEAR(GETDATE())");
        }
    }
}
