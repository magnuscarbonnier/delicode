using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DeliCode.ProductAPI.Migrations
{
    public partial class SeedProductsToDb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("4df795cf-ea1c-47c1-a4e0-f20742cfe359"));

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("7ec6222e-32d6-4293-8d23-85504851156c"));

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("891ceb12-e067-4596-a8a2-4e0a4b9eef6e"));

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("bd8f361d-e5e3-4f33-82cf-2594368d78c3"));

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("c64c3fc5-3e3f-4c5a-9baf-e0e58541ab07"));

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "AmountInStorage", "Description", "ImageUrl", "Name", "Price" },
                values: new object[,]
                {
                    { new Guid("8b942715-1c92-48d7-b32b-7de961872d71"), 3, "Varma, frysta", "img/bullar-min.jpg", "Kanelbulle", 9.50m },
                    { new Guid("a215f4e8-fc3c-4b32-95ec-93598b3517ec"), 0, "Extra florsocker, extra kall", "img/kladdkaka-min.jpg", "Kladdkaka", 50.00m },
                    { new Guid("c273dbe9-06bd-4907-bda7-440baa3fb136"), 3, "Innehåller grädde och is", "img/tarta-min.jpg", "Tårta", 79.90m },
                    { new Guid("17bf3cf4-5d40-40a5-b169-b5feea2564eb"), 1, "En vanlig cheesecake, mellanfryst", "img/ostkaka-min.jpg", "Cheesecake", 29.90m },
                    { new Guid("cb6745ff-1656-4451-b81c-af1b04a394b7"), 4, "Stora, tinade", "img/muffin-min.jpg", "Muffin", 19.90m }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("17bf3cf4-5d40-40a5-b169-b5feea2564eb"));

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("8b942715-1c92-48d7-b32b-7de961872d71"));

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("a215f4e8-fc3c-4b32-95ec-93598b3517ec"));

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("c273dbe9-06bd-4907-bda7-440baa3fb136"));

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("cb6745ff-1656-4451-b81c-af1b04a394b7"));

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "AmountInStorage", "Description", "ImageUrl", "Name", "Price" },
                values: new object[,]
                {
                    { new Guid("891ceb12-e067-4596-a8a2-4e0a4b9eef6e"), 3, "Varma", "https://picsum.photos/id/133/286/180", "Kanelbulle", 9.50m },
                    { new Guid("bd8f361d-e5e3-4f33-82cf-2594368d78c3"), 0, "Extra florsocker", "https://picsum.photos/id/106/286/180", "Kladdkarta", 50.00m },
                    { new Guid("4df795cf-ea1c-47c1-a4e0-f20742cfe359"), 3, "Innehåller grädde", "https://picsum.photos/id/292/286/180", "Tårta", 79.90m },
                    { new Guid("7ec6222e-32d6-4293-8d23-85504851156c"), 1, "En vanlig cheesecake", "https://picsum.photos/id/104/286/180", "Cheesecake", 29.90m },
                    { new Guid("c64c3fc5-3e3f-4c5a-9baf-e0e58541ab07"), 4, "Stora", "https://picsum.photos/id/143/286/180", "Muffin", 19.90m }
                });
        }
    }
}
