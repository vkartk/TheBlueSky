using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TheBlueSky.Flights.Migrations
{
    /// <inheritdoc />
    public partial class AddBaseFaretoFlight : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "BaseFare",
                table: "Flights",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BaseFare",
                table: "Flights");
        }
    }
}
