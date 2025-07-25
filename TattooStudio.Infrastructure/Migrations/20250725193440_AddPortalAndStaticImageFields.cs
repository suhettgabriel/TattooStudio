using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TattooStudio.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddPortalAndStaticImageFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "BodyPartPhotoUrl",
                table: "TattooRequests",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MagicLinkToken",
                table: "TattooRequests",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "MagicLinkTokenExpiration",
                table: "TattooRequests",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ReferenceImageUrls",
                table: "TattooRequests",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BodyPartPhotoUrl",
                table: "TattooRequests");

            migrationBuilder.DropColumn(
                name: "MagicLinkToken",
                table: "TattooRequests");

            migrationBuilder.DropColumn(
                name: "MagicLinkTokenExpiration",
                table: "TattooRequests");

            migrationBuilder.DropColumn(
                name: "ReferenceImageUrls",
                table: "TattooRequests");
        }
    }
}
