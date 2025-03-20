using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EquiMarket.Data.Migrations
{
    /// <inheritdoc />
    public partial class Uhm : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ImageFile",
                table: "Horses",
                newName: "ImagePath");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ImagePath",
                table: "Horses",
                newName: "ImageFile");
        }
    }
}
