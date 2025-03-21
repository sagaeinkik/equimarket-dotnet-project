using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EquiMarket.Data.Migrations
{
    /// <inheritdoc />
    public partial class Horse_User_Relations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Horses",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Horses_UserId",
                table: "Horses",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Horses_AspNetUsers_UserId",
                table: "Horses",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Horses_AspNetUsers_UserId",
                table: "Horses");

            migrationBuilder.DropIndex(
                name: "IX_Horses_UserId",
                table: "Horses");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Horses");
        }
    }
}
