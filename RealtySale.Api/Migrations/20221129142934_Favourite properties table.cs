using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RealtySale.Api.Migrations
{
    /// <inheritdoc />
    public partial class Favouritepropertiestable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FavouriteProperties",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false),
                    PropertyId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FavouriteProperties", x => new { x.UserId, x.PropertyId });
                    table.ForeignKey(
                        name: "FK_FavouriteProperties_Properties_PropertyId",
                        column: x => x.PropertyId,
                        principalTable: "Properties",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_FavouriteProperties_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FavouriteProperties_PropertyId",
                table: "FavouriteProperties",
                column: "PropertyId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FavouriteProperties");
        }
    }
}
