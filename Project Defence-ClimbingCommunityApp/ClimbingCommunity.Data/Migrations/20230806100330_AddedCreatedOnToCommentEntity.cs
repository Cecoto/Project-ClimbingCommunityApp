using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ClimbingCommunity.Data.Migrations
{
    public partial class AddedCreatedOnToCommentEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "PhotoUrl",
                table: "Trainings",
                type: "nvarchar(max)",
                nullable: false,
                comment: "Photo of the climbing training will be/Gym picture",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldComment: "Photo of the climbing training willbe/Gym picture");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedOn",
                table: "Comments",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "GETDATE()");

            migrationBuilder.AlterColumn<string>(
                name: "UserType",
                table: "AspNetUsers",
                type: "nvarchar(25)",
                maxLength: 25,
                nullable: false,
                comment: "Here we save the user role in the application.",
                oldClrType: typeof(string),
                oldType: "nvarchar(25)",
                oldMaxLength: 25,
                oldComment: "Here we save the userRole in the application.");

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedOn",
                table: "Comments");

            migrationBuilder.AlterColumn<string>(
                name: "PhotoUrl",
                table: "Trainings",
                type: "nvarchar(max)",
                nullable: false,
                comment: "Photo of the climbing training willbe/Gym picture",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldComment: "Photo of the climbing training will be/Gym picture");

            migrationBuilder.AlterColumn<string>(
                name: "UserType",
                table: "AspNetUsers",
                type: "nvarchar(25)",
                maxLength: 25,
                nullable: false,
                comment: "Here we save the userRole in the application.",
                oldClrType: typeof(string),
                oldType: "nvarchar(25)",
                oldMaxLength: 25,
                oldComment: "Here we save the user role in the application.");

        }
    }
}
