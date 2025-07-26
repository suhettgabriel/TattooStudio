using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TattooStudio.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddSharedDocumentsAndConsent : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "ConsentDate",
                table: "TattooRequests",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "ConsentGiven",
                table: "TattooRequests",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "ConsentFormText",
                table: "SystemSettings",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsConsentFormEnabled",
                table: "SystemSettings",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "SharedDocuments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FileName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FileUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UploadedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SharedDocuments", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SharedDocuments");

            migrationBuilder.DropColumn(
                name: "ConsentDate",
                table: "TattooRequests");

            migrationBuilder.DropColumn(
                name: "ConsentGiven",
                table: "TattooRequests");

            migrationBuilder.DropColumn(
                name: "ConsentFormText",
                table: "SystemSettings");

            migrationBuilder.DropColumn(
                name: "IsConsentFormEnabled",
                table: "SystemSettings");
        }
    }
}
