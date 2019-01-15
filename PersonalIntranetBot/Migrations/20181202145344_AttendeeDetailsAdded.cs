using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace PersonalIntranetBot.Migrations
{
    public partial class AttendeeDetailsAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SocialLinks_Attendees_AttendeeId",
                table: "SocialLinks");

            migrationBuilder.DropColumn(
                name: "IsAsPerson",
                table: "Attendees");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "Attendees");

            migrationBuilder.AlterColumn<int>(
                name: "AttendeeId",
                table: "SocialLinks",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CurrentJobCompany",
                table: "Attendees",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CurrentJobTitle",
                table: "Attendees",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DisplayName",
                table: "Attendees",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EducationLocation",
                table: "Attendees",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ImageURL",
                table: "Attendees",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsPerson",
                table: "Attendees",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddForeignKey(
                name: "FK_SocialLinks_Attendees_AttendeeId",
                table: "SocialLinks",
                column: "AttendeeId",
                principalTable: "Attendees",
                principalColumn: "AttendeeId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SocialLinks_Attendees_AttendeeId",
                table: "SocialLinks");

            migrationBuilder.DropColumn(
                name: "CurrentJobCompany",
                table: "Attendees");

            migrationBuilder.DropColumn(
                name: "CurrentJobTitle",
                table: "Attendees");

            migrationBuilder.DropColumn(
                name: "DisplayName",
                table: "Attendees");

            migrationBuilder.DropColumn(
                name: "EducationLocation",
                table: "Attendees");

            migrationBuilder.DropColumn(
                name: "ImageURL",
                table: "Attendees");

            migrationBuilder.DropColumn(
                name: "IsPerson",
                table: "Attendees");

            migrationBuilder.AlterColumn<int>(
                name: "AttendeeId",
                table: "SocialLinks",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<bool>(
                name: "IsAsPerson",
                table: "Attendees",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Attendees",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_SocialLinks_Attendees_AttendeeId",
                table: "SocialLinks",
                column: "AttendeeId",
                principalTable: "Attendees",
                principalColumn: "AttendeeId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
