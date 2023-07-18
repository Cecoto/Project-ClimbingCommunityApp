using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ClimbingCommunity.Data.Migrations
{
    public partial class AddedCommentEntityWithRelations : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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

            migrationBuilder.CreateTable(
                name: "Comments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false, comment: "Unique identifier")
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Text = table.Column<string>(type: "nvarchar(400)", maxLength: 400, nullable: false, comment: "Content of the comment"),
                    ClimbingTripId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TrainingId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AuthorId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    isActive = table.Column<bool>(type: "bit", nullable: true, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Comments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Comments_AspNetUsers_AuthorId",
                        column: x => x.AuthorId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Comments_ClimbingTrips_ClimbingTripId",
                        column: x => x.ClimbingTripId,
                        principalTable: "ClimbingTrips",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Comments_Trainings_TrainingId",
                        column: x => x.TrainingId,
                        principalTable: "Trainings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "ClimbingTrips",
                columns: new[] { "Id", "Destination", "Duration", "IsActive", "OrganizatorId", "PhotoUrl", "Title", "TripTypeId" },
                values: new object[] { new Guid("0ca910e0-efd1-4acf-acea-952fcdc9ae25"), "France, Fontainebleau", 10, true, "930cb0dc-0c2c-4e74-a885-d93f862588fb", "\"~/images/ClimbingTrips/Font.jpg\"", "First Climbing Trip", 1 });

            migrationBuilder.InsertData(
                table: "ClimbingTrips",
                columns: new[] { "Id", "Destination", "Duration", "IsActive", "OrganizatorId", "PhotoUrl", "Title", "TripTypeId" },
                values: new object[] { new Guid("485087e1-9e47-45a6-bac1-79876e05f28e"), "South Afrika, Capetown", 20, true, "930cb0dc-0c2c-4e74-a885-d93f862588fb", "~/images/ClimbingTrips/Rocklands.webp", "Second Climbing Trip", 1 });

            migrationBuilder.InsertData(
                table: "ClimbingTrips",
                columns: new[] { "Id", "Destination", "Duration", "IsActive", "OrganizatorId", "PhotoUrl", "Title", "TripTypeId" },
                values: new object[] { new Guid("689ee195-26c9-42d1-8509-3645b248cd94"), "Spain, Mallorca", 5, true, "930cb0dc-0c2c-4e74-a885-d93f862588fb", "~/images/ClimbingTrips/Mallorca.jpg", "Third Climbing Trip", 3 });

            migrationBuilder.CreateIndex(
                name: "IX_Comments_AuthorId",
                table: "Comments",
                column: "AuthorId");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_ClimbingTripId",
                table: "Comments",
                column: "ClimbingTripId");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_TrainingId",
                table: "Comments",
                column: "TrainingId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Comments");

            migrationBuilder.DeleteData(
                table: "ClimbingTrips",
                keyColumn: "Id",
                keyValue: new Guid("0ca910e0-efd1-4acf-acea-952fcdc9ae25"));

            migrationBuilder.DeleteData(
                table: "ClimbingTrips",
                keyColumn: "Id",
                keyValue: new Guid("485087e1-9e47-45a6-bac1-79876e05f28e"));

            migrationBuilder.DeleteData(
                table: "ClimbingTrips",
                keyColumn: "Id",
                keyValue: new Guid("689ee195-26c9-42d1-8509-3645b248cd94"));

            migrationBuilder.InsertData(
                table: "ClimbingTrips",
                columns: new[] { "Id", "CreatedOn", "Destination", "Duration", "IsActive", "OrganizatorId", "PhotoUrl", "Title", "TripTypeId" },
                values: new object[] { new Guid("003551cb-ad78-48f5-bfc3-a014bbf5d629"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "South Afrika, Capetown", 20, true, "930cb0dc-0c2c-4e74-a885-d93f862588fb", "~/images/ClimbingTrips/Rocklands.webp", "Second Climbing Trip", 1 });

            migrationBuilder.InsertData(
                table: "ClimbingTrips",
                columns: new[] { "Id", "CreatedOn", "Destination", "Duration", "IsActive", "OrganizatorId", "PhotoUrl", "Title", "TripTypeId" },
                values: new object[] { new Guid("d8b4f772-5860-48e5-a4c0-1fbdc1d481f1"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "France, Fontainebleau", 10, true, "930cb0dc-0c2c-4e74-a885-d93f862588fb", "\"~/images/ClimbingTrips/Font.jpg\"", "First Climbing Trip", 1 });

            migrationBuilder.InsertData(
                table: "ClimbingTrips",
                columns: new[] { "Id", "CreatedOn", "Destination", "Duration", "IsActive", "OrganizatorId", "PhotoUrl", "Title", "TripTypeId" },
                values: new object[] { new Guid("e36e3a78-d700-48d5-bf68-43ca45351cd0"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Spain, Mallorca", 5, true, "930cb0dc-0c2c-4e74-a885-d93f862588fb", "~/images/ClimbingTrips/Mallorca.jpg", "Third Climbing Trip", 3 });
        }
    }
}
