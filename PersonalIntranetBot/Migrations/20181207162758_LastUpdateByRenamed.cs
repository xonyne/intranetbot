using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace PersonalIntranetBot.Migrations
{
    public partial class LastUpdateByRenamed : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "Attendees");

            migrationBuilder.AddColumn<string>(
                name: "LastUpdatedBy",
                table: "Attendees",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LastUpdatedBy",
                table: "Attendees");

            migrationBuilder.AddColumn<string>(
                name: "UpdatedBy",
                table: "Attendees",
                nullable: true);
        }
    }
}
