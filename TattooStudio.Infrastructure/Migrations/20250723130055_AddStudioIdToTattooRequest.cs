using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TattooStudio.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddStudioIdToTattooRequest : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "StudioId",
                table: "TattooRequests",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_TattooRequests_StudioId",
                table: "TattooRequests",
                column: "StudioId");

            migrationBuilder.AddForeignKey(
                name: "FK_TattooRequests_Studios_StudioId",
                table: "TattooRequests",
                column: "StudioId",
                principalTable: "Studios",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TattooRequests_Studios_StudioId",
                table: "TattooRequests");

            migrationBuilder.DropIndex(
                name: "IX_TattooRequests_StudioId",
                table: "TattooRequests");

            migrationBuilder.DropColumn(
                name: "StudioId",
                table: "TattooRequests");
        }
    }
}
