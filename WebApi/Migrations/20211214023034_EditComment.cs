using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApi.Migrations
{
    public partial class EditComment : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CommentParent",
                table: "Comments",
                newName: "CommentParentId");

            migrationBuilder.AddColumn<DateTime>(
                name: "DateCreate",
                table: "Comments",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DateCreate",
                table: "Comments");

            migrationBuilder.RenameColumn(
                name: "CommentParentId",
                table: "Comments",
                newName: "CommentParent");
        }
    }
}
