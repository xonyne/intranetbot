using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace PersonalIntranetBot.Migrations
{
    public partial class MeetingCommentsTableRenamed : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_MeetingsCommments",
                table: "MeetingsCommments");

            migrationBuilder.RenameTable(
                name: "MeetingsCommments",
                newName: "MeetingsComments");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MeetingsComments",
                table: "MeetingsComments",
                column: "MeetingCommentId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_MeetingsComments",
                table: "MeetingsComments");

            migrationBuilder.RenameTable(
                name: "MeetingsComments",
                newName: "MeetingsCommments");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MeetingsCommments",
                table: "MeetingsCommments",
                column: "MeetingCommentId");
        }
    }
}
