using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EquiMarket.Data.Migrations
{
    /// <inheritdoc />
    public partial class Horse_Created_Date : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "Horses",
                type: "TEXT",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "Horses");
        }
    }
}
