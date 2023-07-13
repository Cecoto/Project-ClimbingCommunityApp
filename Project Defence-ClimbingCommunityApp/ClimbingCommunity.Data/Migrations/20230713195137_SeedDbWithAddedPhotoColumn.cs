using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ClimbingCommunity.Data.Migrations
{
    public partial class SeedDbWithAddedPhotoColumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<bool>(
                name: "isActive",
                table: "Trainings",
                type: "bit",
                nullable: true,
                defaultValue: true,
                comment: "Property for soft delete action.",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: true,
                oldComment: "Property for soft delete action.");

            migrationBuilder.AddColumn<string>(
                name: "PhotoUrl",
                table: "Trainings",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                comment: "Photo of the climbing training willbe/Gym picture");

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "ClimbingTrips",
                type: "bit",
                nullable: true,
                defaultValue: true,
                comment: "Property for soft delete.",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: true,
                oldComment: "Property for soft delete.");

            migrationBuilder.AddColumn<string>(
                name: "PhotoUrl",
                table: "ClimbingTrips",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                comment: "Photo of the climbing trip place");

            migrationBuilder.InsertData(
                table: "ClimbingTrips",
                columns: new[] { "Id", "Destination", "Duration", "IsActive", "OrganizatorId", "PhotoUrl", "Title", "TripTypeId" },
                values: new object[,]
                {
                    { new Guid("5e181fb3-1a5d-4095-9465-1bc6eec010ab"), "Spain, Mallorca", 5, true, "930cb0dc-0c2c-4e74-a885-d93f862588fb", "~/images/ClimbingTrips/Mallorca.jpg", "Third Climbing Trip", 3 },
                    { new Guid("75f3cc84-1b84-4fa2-9c27-2fb0dc36dd4a"), "France, Fontainebleau", 10, true, "930cb0dc-0c2c-4e74-a885-d93f862588fb", "\"~/images/ClimbingTrips/Font.jpg\"", "First Climbing Trip", 1 },
                    { new Guid("d87c23ce-7738-4ce2-9cc5-60d1c3024bfc"), "South Afrika, Capetown", 20, true, "930cb0dc-0c2c-4e74-a885-d93f862588fb", "~/images/ClimbingTrips/Rocklands.webp", "Second Climbing Trip", 1 }
                });

            migrationBuilder.InsertData(
                table: "Trainings",
                columns: new[] { "Id", "CoachId", "Duration", "Location", "PhotoUrl", "Price", "TargetId", "Title" },
                values: new object[,]
                {
                    { new Guid("4f9e7b2f-c085-4fea-b064-3efbbf6beab2"), "3c0bd0ac-9f64-4444-ab43-a31a1886462b", 2, "Bulgaria, Sofia", "~/images/Traingings/Sofia.jpg", 25.00m, 2, "First training" },
                    { new Guid("558d2f08-7cbd-4b95-a661-cd6c8320cf35"), "3c0bd0ac-9f64-4444-ab43-a31a1886462b", 3, "Austria, Innsbruck", "~/images/Traingings/Innsbruck.jpg", 25.00m, 2, "Second training" },
                    { new Guid("7e614d87-fe30-40d2-9214-4aea1ce7e98f"), "749a78a0-ce39-4a27-b9f7-e83ec7642768", 1, "Spain, Madrid", "~/images/Traingings/Madrid.jpg", 25.00m, 2, "Third training" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
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

            migrationBuilder.DeleteData(
                table: "Trainings",
                keyColumn: "Id",
                keyValue: new Guid("4f9e7b2f-c085-4fea-b064-3efbbf6beab2"));

            migrationBuilder.DeleteData(
                table: "Trainings",
                keyColumn: "Id",
                keyValue: new Guid("558d2f08-7cbd-4b95-a661-cd6c8320cf35"));

            migrationBuilder.DeleteData(
                table: "Trainings",
                keyColumn: "Id",
                keyValue: new Guid("7e614d87-fe30-40d2-9214-4aea1ce7e98f"));

            migrationBuilder.DropColumn(
                name: "PhotoUrl",
                table: "Trainings");

            migrationBuilder.DropColumn(
                name: "PhotoUrl",
                table: "ClimbingTrips");

            migrationBuilder.AlterColumn<bool>(
                name: "isActive",
                table: "Trainings",
                type: "bit",
                nullable: false,
                defaultValue: true,
                comment: "Property for soft delete action.",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true,
                oldDefaultValue: true,
                oldComment: "Property for soft delete action.");

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "ClimbingTrips",
                type: "bit",
                nullable: false,
                defaultValue: true,
                comment: "Property for soft delete.",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true,
                oldDefaultValue: true,
                oldComment: "Property for soft delete.");
        }
    }
}
