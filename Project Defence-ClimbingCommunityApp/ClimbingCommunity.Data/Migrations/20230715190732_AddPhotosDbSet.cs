using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ClimbingCommunity.Data.Migrations
{
    public partial class AddPhotosDbSet : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Photo_AspNetUsers_UserId",
                table: "Photo");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Photo",
                table: "Photo");

            migrationBuilder.DeleteData(
                table: "ClimbingTrips",
                keyColumn: "Id",
                keyValue: new Guid("402144f9-97b1-4347-83b8-5bccda0ab294"));

            migrationBuilder.DeleteData(
                table: "ClimbingTrips",
                keyColumn: "Id",
                keyValue: new Guid("6f3e3d07-217d-4c6c-970a-13b30abbe2cb"));

            migrationBuilder.DeleteData(
                table: "ClimbingTrips",
                keyColumn: "Id",
                keyValue: new Guid("97ca0296-0d4c-4233-95be-cbdcda3fdef6"));

            migrationBuilder.RenameTable(
                name: "Photo",
                newName: "Photos");

            migrationBuilder.RenameIndex(
                name: "IX_Photo_UserId",
                table: "Photos",
                newName: "IX_Photos_UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Photos",
                table: "Photos",
                column: "Id");

            migrationBuilder.InsertData(
                table: "ClimbingTrips",
                columns: new[] { "Id", "Destination", "Duration", "IsActive", "OrganizatorId", "PhotoUrl", "Title", "TripTypeId" },
                values: new object[] { new Guid("03e1a942-7c20-400b-a776-77f45bb0c558"), "South Afrika, Capetown", 20, true, "930cb0dc-0c2c-4e74-a885-d93f862588fb", "~/images/ClimbingTrips/Rocklands.webp", "Second Climbing Trip", 1 });

            migrationBuilder.InsertData(
                table: "ClimbingTrips",
                columns: new[] { "Id", "Destination", "Duration", "IsActive", "OrganizatorId", "PhotoUrl", "Title", "TripTypeId" },
                values: new object[] { new Guid("69c4f3b2-2e74-4f3a-a6aa-ffe4b591abe0"), "France, Fontainebleau", 10, true, "930cb0dc-0c2c-4e74-a885-d93f862588fb", "\"~/images/ClimbingTrips/Font.jpg\"", "First Climbing Trip", 1 });

            migrationBuilder.InsertData(
                table: "ClimbingTrips",
                columns: new[] { "Id", "Destination", "Duration", "IsActive", "OrganizatorId", "PhotoUrl", "Title", "TripTypeId" },
                values: new object[] { new Guid("709c5e92-5818-4eed-b98e-fb5f1b0876df"), "Spain, Mallorca", 5, true, "930cb0dc-0c2c-4e74-a885-d93f862588fb", "~/images/ClimbingTrips/Mallorca.jpg", "Third Climbing Trip", 3 });

            migrationBuilder.AddForeignKey(
                name: "FK_Photos_AspNetUsers_UserId",
                table: "Photos",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Photos_AspNetUsers_UserId",
                table: "Photos");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Photos",
                table: "Photos");

            migrationBuilder.DeleteData(
                table: "ClimbingTrips",
                keyColumn: "Id",
                keyValue: new Guid("03e1a942-7c20-400b-a776-77f45bb0c558"));

            migrationBuilder.DeleteData(
                table: "ClimbingTrips",
                keyColumn: "Id",
                keyValue: new Guid("69c4f3b2-2e74-4f3a-a6aa-ffe4b591abe0"));

            migrationBuilder.DeleteData(
                table: "ClimbingTrips",
                keyColumn: "Id",
                keyValue: new Guid("709c5e92-5818-4eed-b98e-fb5f1b0876df"));

            migrationBuilder.RenameTable(
                name: "Photos",
                newName: "Photo");

            migrationBuilder.RenameIndex(
                name: "IX_Photos_UserId",
                table: "Photo",
                newName: "IX_Photo_UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Photo",
                table: "Photo",
                column: "Id");

            migrationBuilder.InsertData(
                table: "ClimbingTrips",
                columns: new[] { "Id", "CreatedOn", "Destination", "Duration", "IsActive", "OrganizatorId", "PhotoUrl", "Title", "TripTypeId" },
                values: new object[] { new Guid("402144f9-97b1-4347-83b8-5bccda0ab294"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Spain, Mallorca", 5, true, "930cb0dc-0c2c-4e74-a885-d93f862588fb", "~/images/ClimbingTrips/Mallorca.jpg", "Third Climbing Trip", 3 });

            migrationBuilder.InsertData(
                table: "ClimbingTrips",
                columns: new[] { "Id", "CreatedOn", "Destination", "Duration", "IsActive", "OrganizatorId", "PhotoUrl", "Title", "TripTypeId" },
                values: new object[] { new Guid("6f3e3d07-217d-4c6c-970a-13b30abbe2cb"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "France, Fontainebleau", 10, true, "930cb0dc-0c2c-4e74-a885-d93f862588fb", "\"~/images/ClimbingTrips/Font.jpg\"", "First Climbing Trip", 1 });

            migrationBuilder.InsertData(
                table: "ClimbingTrips",
                columns: new[] { "Id", "CreatedOn", "Destination", "Duration", "IsActive", "OrganizatorId", "PhotoUrl", "Title", "TripTypeId" },
                values: new object[] { new Guid("97ca0296-0d4c-4233-95be-cbdcda3fdef6"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "South Afrika, Capetown", 20, true, "930cb0dc-0c2c-4e74-a885-d93f862588fb", "~/images/ClimbingTrips/Rocklands.webp", "Second Climbing Trip", 1 });

            migrationBuilder.AddForeignKey(
                name: "FK_Photo_AspNetUsers_UserId",
                table: "Photo",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
