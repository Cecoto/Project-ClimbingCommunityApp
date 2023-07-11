using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ClimbingCommunity.Data.Migrations
{
    public partial class RemovedDiscriminatorColumnFromDbAndAddCreatedOn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedOn",
                table: "Trainings",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "GETDATE()",
                comment: "Date and time user creted the entity");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedOn",
                table: "ClimbingTrips",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "GETDATE()",
                comment: "Date and time user creted the entity");

            migrationBuilder.AddColumn<string>(
                name: "UserType",
                table: "AspNetUsers",
                type: "nvarchar(25)",
                maxLength: 25,
                nullable: false,
                defaultValue: "",
                comment: "Here we save the userRole in the application.");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedOn",
                table: "Trainings");

            migrationBuilder.DropColumn(
                name: "CreatedOn",
                table: "ClimbingTrips");

            migrationBuilder.DropColumn(
                name: "UserType",
                table: "AspNetUsers");
        }
    }
}
