using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ClimbingCommunity.Data.Migrations
{
    public partial class AddPhotosCollectionToTheUserAndAddPhotoEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "ClimbingTrips",
                keyColumn: "Id",
                keyValue: new Guid("5e181fb3-1a5d-4095-9465-1bc6eec010ab"));

            migrationBuilder.DeleteData(
                table: "ClimbingTrips",
                keyColumn: "Id",
                keyValue: new Guid("75f3cc84-1b84-4fa2-9c27-2fb0dc36dd4a"));

            migrationBuilder.DeleteData(
                table: "ClimbingTrips",
                keyColumn: "Id",
                keyValue: new Guid("d87c23ce-7738-4ce2-9cc5-60d1c3024bfc"));

            migrationBuilder.CreateTable(
                name: "Photo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ImageUrl = table.Column<string>(type: "nvarchar(2048)", maxLength: 2048, nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Photo", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Photo_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "ClimbingTrips",
                columns: new[] { "Id", "Destination", "Duration", "IsActive", "OrganizatorId", "PhotoUrl", "Title", "TripTypeId" },
                values: new object[] { new Guid("402144f9-97b1-4347-83b8-5bccda0ab294"), "Spain, Mallorca", 5, true, "930cb0dc-0c2c-4e74-a885-d93f862588fb", "~/images/ClimbingTrips/Mallorca.jpg", "Third Climbing Trip", 3 });

            migrationBuilder.InsertData(
                table: "ClimbingTrips",
                columns: new[] { "Id", "Destination", "Duration", "IsActive", "OrganizatorId", "PhotoUrl", "Title", "TripTypeId" },
                values: new object[] { new Guid("6f3e3d07-217d-4c6c-970a-13b30abbe2cb"), "France, Fontainebleau", 10, true, "930cb0dc-0c2c-4e74-a885-d93f862588fb", "\"~/images/ClimbingTrips/Font.jpg\"", "First Climbing Trip", 1 });

            migrationBuilder.InsertData(
                table: "ClimbingTrips",
                columns: new[] { "Id", "Destination", "Duration", "IsActive", "OrganizatorId", "PhotoUrl", "Title", "TripTypeId" },
                values: new object[] { new Guid("97ca0296-0d4c-4233-95be-cbdcda3fdef6"), "South Afrika, Capetown", 20, true, "930cb0dc-0c2c-4e74-a885-d93f862588fb", "~/images/ClimbingTrips/Rocklands.webp", "Second Climbing Trip", 1 });

            migrationBuilder.CreateIndex(
                name: "IX_Photo_UserId",
                table: "Photo",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Photo");

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

            migrationBuilder.InsertData(
                table: "ClimbingTrips",
                columns: new[] { "Id", "CreatedOn", "Destination", "Duration", "IsActive", "OrganizatorId", "PhotoUrl", "Title", "TripTypeId" },
                values: new object[] { new Guid("5e181fb3-1a5d-4095-9465-1bc6eec010ab"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Spain, Mallorca", 5, true, "930cb0dc-0c2c-4e74-a885-d93f862588fb", "~/images/ClimbingTrips/Mallorca.jpg", "Third Climbing Trip", 3 });

            migrationBuilder.InsertData(
                table: "ClimbingTrips",
                columns: new[] { "Id", "CreatedOn", "Destination", "Duration", "IsActive", "OrganizatorId", "PhotoUrl", "Title", "TripTypeId" },
                values: new object[] { new Guid("75f3cc84-1b84-4fa2-9c27-2fb0dc36dd4a"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "France, Fontainebleau", 10, true, "930cb0dc-0c2c-4e74-a885-d93f862588fb", "\"~/images/ClimbingTrips/Font.jpg\"", "First Climbing Trip", 1 });

            migrationBuilder.InsertData(
                table: "ClimbingTrips",
                columns: new[] { "Id", "CreatedOn", "Destination", "Duration", "IsActive", "OrganizatorId", "PhotoUrl", "Title", "TripTypeId" },
                values: new object[] { new Guid("d87c23ce-7738-4ce2-9cc5-60d1c3024bfc"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "South Afrika, Capetown", 20, true, "930cb0dc-0c2c-4e74-a885-d93f862588fb", "~/images/ClimbingTrips/Rocklands.webp", "Second Climbing Trip", 1 });
        }
    }
}
