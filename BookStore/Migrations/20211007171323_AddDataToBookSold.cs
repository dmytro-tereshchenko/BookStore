using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BookStore.Migrations
{
    public partial class AddDataToBookSold : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "BookInStores",
                keyColumn: "Id",
                keyValue: 1,
                column: "DateAdded",
                value: new DateTime(2018, 10, 7, 20, 13, 22, 494, DateTimeKind.Local).AddTicks(6380));

            migrationBuilder.UpdateData(
                table: "BookInStores",
                keyColumn: "Id",
                keyValue: 2,
                column: "DateAdded",
                value: new DateTime(2021, 1, 7, 20, 13, 22, 497, DateTimeKind.Local).AddTicks(6962));

            migrationBuilder.UpdateData(
                table: "BookInStores",
                keyColumn: "Id",
                keyValue: 3,
                column: "DateAdded",
                value: new DateTime(2021, 9, 17, 20, 13, 22, 497, DateTimeKind.Local).AddTicks(6990));

            migrationBuilder.UpdateData(
                table: "BookInStores",
                keyColumn: "Id",
                keyValue: 4,
                column: "DateAdded",
                value: new DateTime(2021, 10, 3, 20, 13, 22, 497, DateTimeKind.Local).AddTicks(7001));

            migrationBuilder.UpdateData(
                table: "BookInStores",
                keyColumn: "Id",
                keyValue: 5,
                column: "DateAdded",
                value: new DateTime(2021, 10, 7, 20, 13, 22, 497, DateTimeKind.Local).AddTicks(7005));

            migrationBuilder.UpdateData(
                table: "BookInStores",
                keyColumn: "Id",
                keyValue: 6,
                column: "DateAdded",
                value: new DateTime(2021, 10, 7, 20, 13, 22, 497, DateTimeKind.Local).AddTicks(7009));

            migrationBuilder.UpdateData(
                table: "BookInStores",
                keyColumn: "Id",
                keyValue: 7,
                column: "DateAdded",
                value: new DateTime(2021, 9, 29, 20, 13, 22, 497, DateTimeKind.Local).AddTicks(7013));

            migrationBuilder.UpdateData(
                table: "BookInStores",
                keyColumn: "Id",
                keyValue: 8,
                column: "DateAdded",
                value: new DateTime(2021, 9, 29, 20, 13, 22, 497, DateTimeKind.Local).AddTicks(7017));

            migrationBuilder.UpdateData(
                table: "BookReserves",
                keyColumn: "Id",
                keyValue: 1,
                column: "DateReserve",
                value: new DateTime(2021, 10, 5, 20, 13, 22, 497, DateTimeKind.Local).AddTicks(8516));

            migrationBuilder.UpdateData(
                table: "BookReserves",
                keyColumn: "Id",
                keyValue: 2,
                column: "DateReserve",
                value: new DateTime(2021, 10, 6, 20, 13, 22, 497, DateTimeKind.Local).AddTicks(8840));

            migrationBuilder.UpdateData(
                table: "BookSolds",
                keyColumn: "Id",
                keyValue: 1,
                column: "DateSold",
                value: new DateTime(2021, 7, 7, 20, 13, 22, 498, DateTimeKind.Local).AddTicks(261));

            migrationBuilder.UpdateData(
                table: "BookSolds",
                keyColumn: "Id",
                keyValue: 2,
                column: "DateSold",
                value: new DateTime(2021, 9, 25, 20, 13, 22, 498, DateTimeKind.Local).AddTicks(583));

            migrationBuilder.UpdateData(
                table: "BookSolds",
                keyColumn: "Id",
                keyValue: 3,
                column: "DateSold",
                value: new DateTime(2021, 10, 2, 20, 13, 22, 498, DateTimeKind.Local).AddTicks(594));

            migrationBuilder.InsertData(
                table: "BookSolds",
                columns: new[] { "Id", "AccountId", "BookInStoreId", "DateSold", "SoldPrice" },
                values: new object[] { 4, 2, 4, new DateTime(2021, 10, 5, 20, 13, 22, 498, DateTimeKind.Local).AddTicks(598), 1000m });

            migrationBuilder.UpdateData(
                table: "Stocks",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "DateEnd", "DateStart" },
                values: new object[] { new DateTime(2021, 10, 27, 20, 13, 22, 498, DateTimeKind.Local).AddTicks(2074), new DateTime(2021, 10, 2, 20, 13, 22, 498, DateTimeKind.Local).AddTicks(1746) });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "BookSolds",
                keyColumn: "Id",
                keyValue: 4);

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
                column: "DateReserve",
                value: new DateTime(2021, 10, 4, 20, 50, 39, 539, DateTimeKind.Local).AddTicks(1387));

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
    }
}
