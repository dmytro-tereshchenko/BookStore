using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BookStore.Migrations
{
    public partial class AddDataBookAuthors : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "BookAuthors",
                columns: new[] { "AuthorId", "BookId" },
                values: new object[,]
                {
                    { 5, 5 },
                    { 6, 6 },
                    { 6, 7 },
                    { 6, 8 }
                });

            migrationBuilder.UpdateData(
                table: "BookInStores",
                keyColumn: "Id",
                keyValue: 1,
                column: "DateAdded",
                value: new DateTime(2018, 9, 18, 22, 15, 44, 686, DateTimeKind.Local).AddTicks(3565));

            migrationBuilder.UpdateData(
                table: "BookInStores",
                keyColumn: "Id",
                keyValue: 2,
                column: "DateAdded",
                value: new DateTime(2020, 12, 18, 22, 15, 44, 689, DateTimeKind.Local).AddTicks(5604));

            migrationBuilder.UpdateData(
                table: "BookInStores",
                keyColumn: "Id",
                keyValue: 3,
                column: "DateAdded",
                value: new DateTime(2021, 8, 29, 22, 15, 44, 689, DateTimeKind.Local).AddTicks(5634));

            migrationBuilder.UpdateData(
                table: "BookInStores",
                keyColumn: "Id",
                keyValue: 4,
                column: "DateAdded",
                value: new DateTime(2021, 9, 14, 22, 15, 44, 689, DateTimeKind.Local).AddTicks(5646));

            migrationBuilder.UpdateData(
                table: "BookInStores",
                keyColumn: "Id",
                keyValue: 5,
                column: "DateAdded",
                value: new DateTime(2021, 9, 18, 22, 15, 44, 689, DateTimeKind.Local).AddTicks(5651));

            migrationBuilder.UpdateData(
                table: "BookInStores",
                keyColumn: "Id",
                keyValue: 6,
                column: "DateAdded",
                value: new DateTime(2021, 9, 18, 22, 15, 44, 689, DateTimeKind.Local).AddTicks(5655));

            migrationBuilder.UpdateData(
                table: "BookInStores",
                keyColumn: "Id",
                keyValue: 7,
                column: "DateAdded",
                value: new DateTime(2021, 9, 10, 22, 15, 44, 689, DateTimeKind.Local).AddTicks(5659));

            migrationBuilder.UpdateData(
                table: "BookInStores",
                keyColumn: "Id",
                keyValue: 8,
                column: "DateAdded",
                value: new DateTime(2021, 9, 10, 22, 15, 44, 689, DateTimeKind.Local).AddTicks(5663));

            migrationBuilder.UpdateData(
                table: "BookReserves",
                keyColumn: "Id",
                keyValue: 1,
                column: "DateReserve",
                value: new DateTime(2021, 9, 16, 22, 15, 44, 689, DateTimeKind.Local).AddTicks(6913));

            migrationBuilder.UpdateData(
                table: "BookReserves",
                keyColumn: "Id",
                keyValue: 2,
                column: "DateReserve",
                value: new DateTime(2021, 9, 17, 22, 15, 44, 689, DateTimeKind.Local).AddTicks(7243));

            migrationBuilder.UpdateData(
                table: "BookSolds",
                keyColumn: "Id",
                keyValue: 1,
                column: "DateSold",
                value: new DateTime(2021, 6, 18, 22, 15, 44, 689, DateTimeKind.Local).AddTicks(8712));

            migrationBuilder.UpdateData(
                table: "BookSolds",
                keyColumn: "Id",
                keyValue: 2,
                column: "DateSold",
                value: new DateTime(2021, 9, 6, 22, 15, 44, 689, DateTimeKind.Local).AddTicks(9043));

            migrationBuilder.UpdateData(
                table: "BookSolds",
                keyColumn: "Id",
                keyValue: 3,
                column: "DateSold",
                value: new DateTime(2021, 9, 13, 22, 15, 44, 689, DateTimeKind.Local).AddTicks(9055));

            migrationBuilder.UpdateData(
                table: "Stocks",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "DateEnd", "DateStart" },
                values: new object[] { new DateTime(2021, 10, 8, 22, 15, 44, 690, DateTimeKind.Local).AddTicks(519), new DateTime(2021, 9, 13, 22, 15, 44, 690, DateTimeKind.Local).AddTicks(193) });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "BookAuthors",
                keyColumns: new[] { "AuthorId", "BookId" },
                keyValues: new object[] { 5, 5 });

            migrationBuilder.DeleteData(
                table: "BookAuthors",
                keyColumns: new[] { "AuthorId", "BookId" },
                keyValues: new object[] { 6, 6 });

            migrationBuilder.DeleteData(
                table: "BookAuthors",
                keyColumns: new[] { "AuthorId", "BookId" },
                keyValues: new object[] { 6, 7 });

            migrationBuilder.DeleteData(
                table: "BookAuthors",
                keyColumns: new[] { "AuthorId", "BookId" },
                keyValues: new object[] { 6, 8 });

            migrationBuilder.UpdateData(
                table: "BookInStores",
                keyColumn: "Id",
                keyValue: 1,
                column: "DateAdded",
                value: new DateTime(2018, 9, 18, 20, 34, 28, 990, DateTimeKind.Local).AddTicks(5813));

            migrationBuilder.UpdateData(
                table: "BookInStores",
                keyColumn: "Id",
                keyValue: 2,
                column: "DateAdded",
                value: new DateTime(2020, 12, 18, 20, 34, 28, 993, DateTimeKind.Local).AddTicks(4883));

            migrationBuilder.UpdateData(
                table: "BookInStores",
                keyColumn: "Id",
                keyValue: 3,
                column: "DateAdded",
                value: new DateTime(2021, 8, 29, 20, 34, 28, 993, DateTimeKind.Local).AddTicks(4912));

            migrationBuilder.UpdateData(
                table: "BookInStores",
                keyColumn: "Id",
                keyValue: 4,
                column: "DateAdded",
                value: new DateTime(2021, 9, 14, 20, 34, 28, 993, DateTimeKind.Local).AddTicks(5003));

            migrationBuilder.UpdateData(
                table: "BookInStores",
                keyColumn: "Id",
                keyValue: 5,
                column: "DateAdded",
                value: new DateTime(2021, 9, 18, 20, 34, 28, 993, DateTimeKind.Local).AddTicks(5008));

            migrationBuilder.UpdateData(
                table: "BookInStores",
                keyColumn: "Id",
                keyValue: 6,
                column: "DateAdded",
                value: new DateTime(2021, 9, 18, 20, 34, 28, 993, DateTimeKind.Local).AddTicks(5012));

            migrationBuilder.UpdateData(
                table: "BookInStores",
                keyColumn: "Id",
                keyValue: 7,
                column: "DateAdded",
                value: new DateTime(2021, 9, 10, 20, 34, 28, 993, DateTimeKind.Local).AddTicks(5016));

            migrationBuilder.UpdateData(
                table: "BookInStores",
                keyColumn: "Id",
                keyValue: 8,
                column: "DateAdded",
                value: new DateTime(2021, 9, 10, 20, 34, 28, 993, DateTimeKind.Local).AddTicks(5020));

            migrationBuilder.UpdateData(
                table: "BookReserves",
                keyColumn: "Id",
                keyValue: 1,
                column: "DateReserve",
                value: new DateTime(2021, 9, 16, 20, 34, 28, 993, DateTimeKind.Local).AddTicks(6217));

            migrationBuilder.UpdateData(
                table: "BookReserves",
                keyColumn: "Id",
                keyValue: 2,
                column: "DateReserve",
                value: new DateTime(2021, 9, 17, 20, 34, 28, 993, DateTimeKind.Local).AddTicks(6529));

            migrationBuilder.UpdateData(
                table: "BookSolds",
                keyColumn: "Id",
                keyValue: 1,
                column: "DateSold",
                value: new DateTime(2021, 6, 18, 20, 34, 28, 993, DateTimeKind.Local).AddTicks(7925));

            migrationBuilder.UpdateData(
                table: "BookSolds",
                keyColumn: "Id",
                keyValue: 2,
                column: "DateSold",
                value: new DateTime(2021, 9, 6, 20, 34, 28, 993, DateTimeKind.Local).AddTicks(8239));

            migrationBuilder.UpdateData(
                table: "BookSolds",
                keyColumn: "Id",
                keyValue: 3,
                column: "DateSold",
                value: new DateTime(2021, 9, 13, 20, 34, 28, 993, DateTimeKind.Local).AddTicks(8250));

            migrationBuilder.UpdateData(
                table: "Stocks",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "DateEnd", "DateStart" },
                values: new object[] { new DateTime(2021, 10, 8, 20, 34, 28, 993, DateTimeKind.Local).AddTicks(9731), new DateTime(2021, 9, 13, 20, 34, 28, 993, DateTimeKind.Local).AddTicks(9422) });
        }
    }
}
