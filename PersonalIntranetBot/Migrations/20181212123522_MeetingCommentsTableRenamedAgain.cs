using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace PersonalIntranetBot.Migrations
{
    public partial class MeetingCommentsTableRenamedAgain : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_MeetingsComments",
                table: "MeetingsComments");

            migrationBuilder.RenameTable(
                name: "MeetingsComments",
                newName: "MeetingComments");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MeetingComments",
                table: "MeetingComments",
                column: "MeetingCommentId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_MeetingComments",
                table: "MeetingComments");

            migrationBuilder.RenameTable(
                name: "MeetingComments",
                newName: "MeetingsComments");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MeetingsComments",
                table: "MeetingsComments",
                column: "MeetingCommentId");
        }
    }
}
