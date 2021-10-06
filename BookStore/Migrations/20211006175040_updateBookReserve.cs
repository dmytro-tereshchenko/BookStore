using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BookStore.Migrations
{
    public partial class updateBookReserve : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "BookReserves",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "BookInStores",
                keyColumn: "Id",
                keyValue: 1,
                column: "DateAdded",
                value: new DateTime(2018, 10, 6, 20, 50, 39, 536, DateTimeKind.Local).AddTicks(1021));

            migrationBuilder.UpdateData(
                table: "BookInStores",
                keyColumn: "Id",
                keyValue: 2,
                column: "DateAdded",
                value: new DateTime(2021, 1, 6, 20, 50, 39, 538, DateTimeKind.Local).AddTicks(9838));

            migrationBuilder.UpdateData(
                table: "BookInStores",
                keyColumn: "Id",
                keyValue: 3,
                column: "DateAdded",
                value: new DateTime(2021, 9, 16, 20, 50, 39, 538, DateTimeKind.Local).AddTicks(9865));

            migrationBuilder.UpdateData(
                table: "BookInStores",
                keyColumn: "Id",
                keyValue: 4,
                column: "DateAdded",
                value: new DateTime(2021, 10, 2, 20, 50, 39, 538, DateTimeKind.Local).AddTicks(9876));

            migrationBuilder.UpdateData(
                table: "BookInStores",
                keyColumn: "Id",
                keyValue: 5,
                column: "DateAdded",
                value: new DateTime(2021, 10, 6, 20, 50, 39, 538, DateTimeKind.Local).AddTicks(9880));

            migrationBuilder.UpdateData(
                table: "BookInStores",
                keyColumn: "Id",
                keyValue: 6,
                column: "DateAdded",
                value: new DateTime(2021, 10, 6, 20, 50, 39, 538, DateTimeKind.Local).AddTicks(9884));

            migrationBuilder.UpdateData(
                table: "BookInStores",
                keyColumn: "Id",
                keyValue: 7,
                column: "DateAdded",
                value: new DateTime(2021, 9, 28, 20, 50, 39, 538, DateTimeKind.Local).AddTicks(9888));

            migrationBuilder.UpdateData(
                table: "BookInStores",
                keyColumn: "Id",
                keyValue: 8,
                column: "DateAdded",
                value: new DateTime(2021, 9, 28, 20, 50, 39, 538, DateTimeKind.Local).AddTicks(9892));

            migrationBuilder.UpdateData(
                table: "BookReserves",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "DateReserve", "Description" },
                values: new object[] { new DateTime(2021, 10, 4, 20, 50, 39, 539, DateTimeKind.Local).AddTicks(1387), "Book for client with telephone 0984512574" });

            migrationBuilder.UpdateData(
                table: "BookReserves",
                keyColumn: "Id",
                keyValue: 2,
                column: "DateReserve",
                value: new DateTime(2021, 10, 5, 20, 50, 39, 539, DateTimeKind.Local).AddTicks(1709));

            migrationBuilder.UpdateData(
                table: "BookSolds",
                keyColumn: "Id",
                keyValue: 1,
                column: "DateSold",
                value: new DateTime(2021, 7, 6, 20, 50, 39, 539, DateTimeKind.Local).AddTicks(3172));

            migrationBuilder.UpdateData(
                table: "BookSolds",
                keyColumn: "Id",
                keyValue: 2,
                column: "DateSold",
                value: new DateTime(2021, 9, 24, 20, 50, 39, 539, DateTimeKind.Local).AddTicks(3494));

            migrationBuilder.UpdateData(
                table: "BookSolds",
                keyColumn: "Id",
                keyValue: 3,
                column: "DateSold",
                value: new DateTime(2021, 10, 1, 20, 50, 39, 539, DateTimeKind.Local).AddTicks(3504));

            migrationBuilder.UpdateData(
                table: "Stocks",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "DateEnd", "DateStart" },
                values: new object[] { new DateTime(2021, 10, 26, 20, 50, 39, 539, DateTimeKind.Local).AddTicks(4899), new DateTime(2021, 10, 1, 20, 50, 39, 539, DateTimeKind.Local).AddTicks(4586) });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "BookReserves");

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
    }
}
