using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ClimbingCommunity.Data.Migrations
{
    public partial class AddedTrainingClimberEntityPlusRelation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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

            migrationBuilder.CreateTable(
                name: "TrainingsClimbers",
                columns: table => new
                {
                    ClimberId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    TrainingId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrainingsClimbers", x => new { x.ClimberId, x.TrainingId });
                    table.ForeignKey(
                        name: "FK_TrainingsClimbers_AspNetUsers_ClimberId",
                        column: x => x.ClimberId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TrainingsClimbers_Trainings_TrainingId",
                        column: x => x.TrainingId,
                        principalTable: "Trainings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "ClimbingTrips",
                columns: new[] { "Id", "Destination", "Duration", "IsActive", "OrganizatorId", "PhotoUrl", "Title", "TripTypeId" },
                values: new object[] { new Guid("003551cb-ad78-48f5-bfc3-a014bbf5d629"), "South Afrika, Capetown", 20, true, "930cb0dc-0c2c-4e74-a885-d93f862588fb", "~/images/ClimbingTrips/Rocklands.webp", "Second Climbing Trip", 1 });

            migrationBuilder.InsertData(
                table: "ClimbingTrips",
                columns: new[] { "Id", "Destination", "Duration", "IsActive", "OrganizatorId", "PhotoUrl", "Title", "TripTypeId" },
                values: new object[] { new Guid("d8b4f772-5860-48e5-a4c0-1fbdc1d481f1"), "France, Fontainebleau", 10, true, "930cb0dc-0c2c-4e74-a885-d93f862588fb", "\"~/images/ClimbingTrips/Font.jpg\"", "First Climbing Trip", 1 });

            migrationBuilder.InsertData(
                table: "ClimbingTrips",
                columns: new[] { "Id", "Destination", "Duration", "IsActive", "OrganizatorId", "PhotoUrl", "Title", "TripTypeId" },
                values: new object[] { new Guid("e36e3a78-d700-48d5-bf68-43ca45351cd0"), "Spain, Mallorca", 5, true, "930cb0dc-0c2c-4e74-a885-d93f862588fb", "~/images/ClimbingTrips/Mallorca.jpg", "Third Climbing Trip", 3 });

            migrationBuilder.CreateIndex(
                name: "IX_TrainingsClimbers_TrainingId",
                table: "TrainingsClimbers",
                column: "TrainingId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TrainingsClimbers");

            migrationBuilder.DeleteData(
                table: "ClimbingTrips",
                keyColumn: "Id",
                keyValue: new Guid("003551cb-ad78-48f5-bfc3-a014bbf5d629"));

            migrationBuilder.DeleteData(
                table: "ClimbingTrips",
                keyColumn: "Id",
                keyValue: new Guid("d8b4f772-5860-48e5-a4c0-1fbdc1d481f1"));

            migrationBuilder.DeleteData(
                table: "ClimbingTrips",
                keyColumn: "Id",
                keyValue: new Guid("e36e3a78-d700-48d5-bf68-43ca45351cd0"));

            migrationBuilder.InsertData(
                table: "ClimbingTrips",
                columns: new[] { "Id", "CreatedOn", "Destination", "Duration", "IsActive", "OrganizatorId", "PhotoUrl", "Title", "TripTypeId" },
                values: new object[] { new Guid("03e1a942-7c20-400b-a776-77f45bb0c558"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "South Afrika, Capetown", 20, true, "930cb0dc-0c2c-4e74-a885-d93f862588fb", "~/images/ClimbingTrips/Rocklands.webp", "Second Climbing Trip", 1 });

            migrationBuilder.InsertData(
                table: "ClimbingTrips",
                columns: new[] { "Id", "CreatedOn", "Destination", "Duration", "IsActive", "OrganizatorId", "PhotoUrl", "Title", "TripTypeId" },
                values: new object[] { new Guid("69c4f3b2-2e74-4f3a-a6aa-ffe4b591abe0"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "France, Fontainebleau", 10, true, "930cb0dc-0c2c-4e74-a885-d93f862588fb", "\"~/images/ClimbingTrips/Font.jpg\"", "First Climbing Trip", 1 });

            migrationBuilder.InsertData(
                table: "ClimbingTrips",
                columns: new[] { "Id", "CreatedOn", "Destination", "Duration", "IsActive", "OrganizatorId", "PhotoUrl", "Title", "TripTypeId" },
                values: new object[] { new Guid("709c5e92-5818-4eed-b98e-fb5f1b0876df"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Spain, Mallorca", 5, true, "930cb0dc-0c2c-4e74-a885-d93f862588fb", "~/images/ClimbingTrips/Mallorca.jpg", "Third Climbing Trip", 3 });
        }
    }
}
