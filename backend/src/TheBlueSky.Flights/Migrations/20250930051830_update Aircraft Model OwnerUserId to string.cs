using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TheBlueSky.Flights.Migrations
{
    /// <inheritdoc />
    public partial class updateAircraftModelOwnerUserIdtostring : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "OwnerUserId",
                table: "Aircrafts",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "OwnerUserId",
                table: "Aircrafts",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");
        }
    }
}
