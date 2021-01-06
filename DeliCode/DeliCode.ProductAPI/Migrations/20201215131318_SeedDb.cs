using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DeliCode.ProductAPI.Migrations
{
    public partial class SeedDb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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

        protected override void Down(MigrationBuilder migrationBuilder)
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
        }
    }
}
