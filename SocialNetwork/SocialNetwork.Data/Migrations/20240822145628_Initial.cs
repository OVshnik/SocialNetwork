using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SocialNetwork.Data.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Image",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);
            migrationBuilder.AddColumn<string>(
                 name: "Status",
                 table: "AspNetUsers",
                 type: "nvarchar(max)",
                 nullable: true);
            migrationBuilder.AddColumn<string>(
                 name: "About",
                 table: "AspNetUsers",
                 type: "nvarchar(max)",
                 nullable: true);
        }

    }
}
