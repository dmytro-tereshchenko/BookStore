using Microsoft.EntityFrameworkCore.Migrations;

namespace BookStore.Migrations
{
    public partial class DellCustomersUpdateConnections : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BookReserves_Customers_CustomerId",
                table: "BookReserves");

            migrationBuilder.DropForeignKey(
                name: "FK_BookSolds_Customers_CustomerId",
                table: "BookSolds");

            migrationBuilder.DropForeignKey(
                name: "FK_Stocks_Books_BookId",
                table: "Stocks");

            migrationBuilder.DropTable(
                name: "Customers");

            migrationBuilder.DropIndex(
                name: "IX_BookSolds_CustomerId",
                table: "BookSolds");

            migrationBuilder.DropIndex(
                name: "IX_BookReserves_CustomerId",
                table: "BookReserves");

            migrationBuilder.DropColumn(
                name: "CustomerId",
                table: "BookSolds");

            migrationBuilder.DropColumn(
                name: "CustomerId",
                table: "BookReserves");

            migrationBuilder.RenameColumn(
                name: "BookId",
                table: "Stocks",
                newName: "BookInStoreId");

            migrationBuilder.RenameIndex(
                name: "IX_Stocks_BookId",
                table: "Stocks",
                newName: "IX_Stocks_BookInStoreId");

            migrationBuilder.AlterColumn<decimal>(
                name: "Discount",
                table: "Stocks",
                type: "decimal(4,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(2,2)");

            migrationBuilder.AlterColumn<decimal>(
                name: "SoldPrice",
                table: "BookSolds",
                type: "decimal(8,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(5,2)");

            migrationBuilder.AddColumn<int>(
                name: "AccountId",
                table: "BookSolds",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "AccountId",
                table: "BookReserves",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "Price",
                table: "BookInStores",
                type: "decimal(8,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(5,2)");

            migrationBuilder.AlterColumn<decimal>(
                name: "CostPrice",
                table: "BookInStores",
                type: "decimal(8,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(5,2)");

            migrationBuilder.AlterColumn<bool>(
                name: "Admin",
                table: "Accounts",
                type: "bit",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.CreateIndex(
                name: "IX_BookSolds_AccountId",
                table: "BookSolds",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_BookReserves_AccountId",
                table: "BookReserves",
                column: "AccountId");

            migrationBuilder.AddForeignKey(
                name: "FK_BookReserves_Accounts_AccountId",
                table: "BookReserves",
                column: "AccountId",
                principalTable: "Accounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_BookSolds_Accounts_AccountId",
                table: "BookSolds",
                column: "AccountId",
                principalTable: "Accounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Stocks_BookInStores_BookInStoreId",
                table: "Stocks",
                column: "BookInStoreId",
                principalTable: "BookInStores",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BookReserves_Accounts_AccountId",
                table: "BookReserves");

            migrationBuilder.DropForeignKey(
                name: "FK_BookSolds_Accounts_AccountId",
                table: "BookSolds");

            migrationBuilder.DropForeignKey(
                name: "FK_Stocks_BookInStores_BookInStoreId",
                table: "Stocks");

            migrationBuilder.DropIndex(
                name: "IX_BookSolds_AccountId",
                table: "BookSolds");

            migrationBuilder.DropIndex(
                name: "IX_BookReserves_AccountId",
                table: "BookReserves");

            migrationBuilder.DropColumn(
                name: "AccountId",
                table: "BookSolds");

            migrationBuilder.DropColumn(
                name: "AccountId",
                table: "BookReserves");

            migrationBuilder.RenameColumn(
                name: "BookInStoreId",
                table: "Stocks",
                newName: "BookId");

            migrationBuilder.RenameIndex(
                name: "IX_Stocks_BookInStoreId",
                table: "Stocks",
                newName: "IX_Stocks_BookId");

            migrationBuilder.AlterColumn<decimal>(
                name: "Discount",
                table: "Stocks",
                type: "decimal(2,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(4,2)");

            migrationBuilder.AlterColumn<decimal>(
                name: "SoldPrice",
                table: "BookSolds",
                type: "decimal(5,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(8,2)");

            migrationBuilder.AddColumn<int>(
                name: "CustomerId",
                table: "BookSolds",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "CustomerId",
                table: "BookReserves",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<decimal>(
                name: "Price",
                table: "BookInStores",
                type: "decimal(5,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(8,2)");

            migrationBuilder.AlterColumn<decimal>(
                name: "CostPrice",
                table: "BookInStores",
                type: "decimal(5,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(8,2)");

            migrationBuilder.AlterColumn<bool>(
                name: "Admin",
                table: "Accounts",
                type: "bit",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: false);

            migrationBuilder.CreateTable(
                name: "Customers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    MiddleName = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customers", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BookSolds_CustomerId",
                table: "BookSolds",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_BookReserves_CustomerId",
                table: "BookReserves",
                column: "CustomerId");

            migrationBuilder.AddForeignKey(
                name: "FK_BookReserves_Customers_CustomerId",
                table: "BookReserves",
                column: "CustomerId",
                principalTable: "Customers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BookSolds_Customers_CustomerId",
                table: "BookSolds",
                column: "CustomerId",
                principalTable: "Customers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Stocks_Books_BookId",
                table: "Stocks",
                column: "BookId",
                principalTable: "Books",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
