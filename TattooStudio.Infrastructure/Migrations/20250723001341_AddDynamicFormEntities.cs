using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TattooStudio.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddDynamicFormEntities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TattooRequests_Studios_StudioId",
                table: "TattooRequests");

            migrationBuilder.DropIndex(
                name: "IX_TattooRequests_StudioId",
                table: "TattooRequests");

            migrationBuilder.DropColumn(
                name: "BodyPart",
                table: "TattooRequests");

            migrationBuilder.DropColumn(
                name: "BodyPartPhotoUrl",
                table: "TattooRequests");

            migrationBuilder.DropColumn(
                name: "PreferredSemester",
                table: "TattooRequests");

            migrationBuilder.DropColumn(
                name: "ReferenceImageUrls",
                table: "TattooRequests");

            migrationBuilder.DropColumn(
                name: "StudioId",
                table: "TattooRequests");

            migrationBuilder.DropColumn(
                name: "TattooDescription",
                table: "TattooRequests");

            migrationBuilder.DropColumn(
                name: "TattooSize",
                table: "TattooRequests");

            migrationBuilder.CreateTable(
                name: "FormFields",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Label = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    FieldType = table.Column<int>(type: "int", nullable: false),
                    Options = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsRequired = table.Column<bool>(type: "bit", nullable: false),
                    Order = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FormFields", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TattooRequestAnswers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TattooRequestId = table.Column<int>(type: "int", nullable: false),
                    FormFieldId = table.Column<int>(type: "int", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TattooRequestAnswers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TattooRequestAnswers_FormFields_FormFieldId",
                        column: x => x.FormFieldId,
                        principalTable: "FormFields",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TattooRequestAnswers_TattooRequests_TattooRequestId",
                        column: x => x.TattooRequestId,
                        principalTable: "TattooRequests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TattooRequestAnswers_FormFieldId",
                table: "TattooRequestAnswers",
                column: "FormFieldId");

            migrationBuilder.CreateIndex(
                name: "IX_TattooRequestAnswers_TattooRequestId",
                table: "TattooRequestAnswers",
                column: "TattooRequestId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TattooRequestAnswers");

            migrationBuilder.DropTable(
                name: "FormFields");

            migrationBuilder.AddColumn<string>(
                name: "BodyPart",
                table: "TattooRequests",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "BodyPartPhotoUrl",
                table: "TattooRequests",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PreferredSemester",
                table: "TattooRequests",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ReferenceImageUrls",
                table: "TattooRequests",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "StudioId",
                table: "TattooRequests",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "TattooDescription",
                table: "TattooRequests",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "TattooSize",
                table: "TattooRequests",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

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
    }
}
