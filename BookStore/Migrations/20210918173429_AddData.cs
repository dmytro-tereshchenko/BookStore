using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BookStore.Migrations
{
    public partial class AddData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Accounts",
                columns: new[] { "Id", "Admin", "Login", "Password" },
                values: new object[] { 1, true, "admin", "admin" });

            migrationBuilder.InsertData(
                table: "Accounts",
                columns: new[] { "Id", "Login", "Password" },
                values: new object[] { 2, "seller1", "seller1" });

            migrationBuilder.InsertData(
                table: "Authors",
                columns: new[] { "Id", "FirstName", "LastName", "MiddleName" },
                values: new object[,]
                {
                    { 1, "Marcel", "Proust", null },
                    { 2, "James", "Joyce", null },
                    { 3, "Miguel", "de Cervantes", null },
                    { 4, "Gabriel", "Márquez", "García" },
                    { 5, "Francis", "Fitzgerald", "Scott" },
                    { 6, "John", "Tolkien", "Ronald Reuel" }
                });

            migrationBuilder.InsertData(
                table: "BookSerieses",
                columns: new[] { "Id", "Name" },
                values: new object[] { 1, "The Lord of the Rings" });

            migrationBuilder.InsertData(
                table: "Genres",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 5, "Fantasy" },
                    { 4, "Tragedy" },
                    { 1, "Modernist" },
                    { 2, "Novel" },
                    { 3, "Magic realism" }
                });

            migrationBuilder.InsertData(
                table: "Publishers",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 5, "Charles Scribner's Sons" },
                    { 1, "Grasset and Gallimard" },
                    { 2, "Shakespeare and Company" },
                    { 3, "Francisco de Robles" },
                    { 4, "Editorial" },
                    { 6, "George Allen & Unwin" }
                });

            migrationBuilder.InsertData(
                table: "Books",
                columns: new[] { "Id", "GenreId", "Name", "Pages", "PublisherId", "YearOfPublished" },
                values: new object[,]
                {
                    { 1, 1, "In Search of Lost Time", 4215, 1, 1922 },
                    { 2, 1, "Ulysses", 730, 2, 1922 },
                    { 3, 2, "Don Quixote", 845, 3, 1620 },
                    { 4, 3, "One Hundred Years of Solitude", 624, 4, 1970 },
                    { 5, 4, "The Great Gatsby", 749, 5, 1925 },
                    { 6, 5, "The Fellowship of the Ring", 423, 6, 1954 },
                    { 7, 5, "The Two Towers", 352, 6, 1954 },
                    { 8, 5, "The Return of the King", 416, 6, 1955 }
                });

            migrationBuilder.InsertData(
                table: "BookAuthors",
                columns: new[] { "AuthorId", "BookId" },
                values: new object[,]
                {
                    { 1, 1 },
                    { 2, 2 },
                    { 3, 3 },
                    { 4, 4 }
                });

            migrationBuilder.InsertData(
                table: "BookInStores",
                columns: new[] { "Id", "Amount", "BookId", "CostPrice", "DateAdded", "Price" },
                values: new object[,]
                {
                    { 1, 5, 1, 254.2m, new DateTime(2018, 9, 18, 20, 34, 28, 990, DateTimeKind.Local).AddTicks(5813), 350m },
                    { 2, 12, 2, 324.7m, new DateTime(2020, 12, 18, 20, 34, 28, 993, DateTimeKind.Local).AddTicks(4883), 400m },
                    { 3, 24, 3, 128.6m, new DateTime(2021, 8, 29, 20, 34, 28, 993, DateTimeKind.Local).AddTicks(4912), 200m },
                    { 4, 7, 4, 742.5m, new DateTime(2021, 9, 14, 20, 34, 28, 993, DateTimeKind.Local).AddTicks(5003), 1000m },
                    { 5, 11, 5, 418.1m, new DateTime(2021, 9, 18, 20, 34, 28, 993, DateTimeKind.Local).AddTicks(5008), 600m },
                    { 6, 14, 6, 251m, new DateTime(2021, 9, 18, 20, 34, 28, 993, DateTimeKind.Local).AddTicks(5012), 325m },
                    { 7, 10, 7, 284m, new DateTime(2021, 9, 10, 20, 34, 28, 993, DateTimeKind.Local).AddTicks(5016), 392m },
                    { 8, 15, 8, 327.6m, new DateTime(2021, 9, 10, 20, 34, 28, 993, DateTimeKind.Local).AddTicks(5020), 450m }
                });

            migrationBuilder.InsertData(
                table: "BookSeriesBooks",
                columns: new[] { "BookId", "BookSeriesId", "Position" },
                values: new object[,]
                {
                    { 6, 1, 1 },
                    { 7, 1, 2 },
                    { 8, 1, 3 }
                });

            migrationBuilder.InsertData(
                table: "BookReserves",
                columns: new[] { "Id", "AccountId", "BookInStoreId", "DateReserve" },
                values: new object[,]
                {
                    { 1, 2, 1, new DateTime(2021, 9, 16, 20, 34, 28, 993, DateTimeKind.Local).AddTicks(6217) },
                    { 2, 2, 3, new DateTime(2021, 9, 17, 20, 34, 28, 993, DateTimeKind.Local).AddTicks(6529) }
                });

            migrationBuilder.InsertData(
                table: "BookSolds",
                columns: new[] { "Id", "AccountId", "BookInStoreId", "DateSold", "SoldPrice" },
                values: new object[,]
                {
                    { 1, 2, 2, new DateTime(2021, 6, 18, 20, 34, 28, 993, DateTimeKind.Local).AddTicks(7925), 400m },
                    { 2, 2, 4, new DateTime(2021, 9, 6, 20, 34, 28, 993, DateTimeKind.Local).AddTicks(8239), 1000m },
                    { 3, 2, 5, new DateTime(2021, 9, 13, 20, 34, 28, 993, DateTimeKind.Local).AddTicks(8250), 600m }
                });

            migrationBuilder.InsertData(
                table: "Stocks",
                columns: new[] { "Id", "BookInStoreId", "DateEnd", "DateStart", "Discount" },
                values: new object[] { 1, 3, new DateTime(2021, 10, 8, 20, 34, 28, 993, DateTimeKind.Local).AddTicks(9731), new DateTime(2021, 9, 13, 20, 34, 28, 993, DateTimeKind.Local).AddTicks(9422), 15m });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Accounts",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Authors",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Authors",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "BookAuthors",
                keyColumns: new[] { "AuthorId", "BookId" },
                keyValues: new object[] { 1, 1 });

            migrationBuilder.DeleteData(
                table: "BookAuthors",
                keyColumns: new[] { "AuthorId", "BookId" },
                keyValues: new object[] { 2, 2 });

            migrationBuilder.DeleteData(
                table: "BookAuthors",
                keyColumns: new[] { "AuthorId", "BookId" },
                keyValues: new object[] { 3, 3 });

            migrationBuilder.DeleteData(
                table: "BookAuthors",
                keyColumns: new[] { "AuthorId", "BookId" },
                keyValues: new object[] { 4, 4 });

            migrationBuilder.DeleteData(
                table: "BookInStores",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "BookInStores",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "BookInStores",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "BookReserves",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "BookReserves",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "BookSeriesBooks",
                keyColumns: new[] { "BookId", "BookSeriesId" },
                keyValues: new object[] { 6, 1 });

            migrationBuilder.DeleteData(
                table: "BookSeriesBooks",
                keyColumns: new[] { "BookId", "BookSeriesId" },
                keyValues: new object[] { 7, 1 });

            migrationBuilder.DeleteData(
                table: "BookSeriesBooks",
                keyColumns: new[] { "BookId", "BookSeriesId" },
                keyValues: new object[] { 8, 1 });

            migrationBuilder.DeleteData(
                table: "BookSolds",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "BookSolds",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "BookSolds",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Stocks",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Accounts",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Authors",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Authors",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Authors",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Authors",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "BookInStores",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "BookInStores",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "BookInStores",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "BookInStores",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "BookInStores",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "BookSerieses",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Genres",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Publishers",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Genres",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Genres",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Genres",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Genres",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Publishers",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Publishers",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Publishers",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Publishers",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Publishers",
                keyColumn: "Id",
                keyValue: 5);
        }
    }
}
