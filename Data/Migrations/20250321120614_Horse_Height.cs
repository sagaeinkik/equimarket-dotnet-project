using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EquiMarket.Data.Migrations
{
    /// <inheritdoc />
    public partial class Horse_Height : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Height",
                table: "Horses",
                type: "INTEGER",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Height",
                table: "Horses");
        }
    }
}
