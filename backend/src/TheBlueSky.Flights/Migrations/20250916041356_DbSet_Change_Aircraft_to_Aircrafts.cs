using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TheBlueSky.Flights.Migrations
{
    /// <inheritdoc />
    public partial class DbSet_Change_Aircraft_to_Aircrafts : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AircraftSeats_Aircraft_AircraftId",
                table: "AircraftSeats");

            migrationBuilder.DropForeignKey(
                name: "FK_FlightSchedules_Aircraft_AircraftId",
                table: "FlightSchedules");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Aircraft",
                table: "Aircraft");

            migrationBuilder.RenameTable(
                name: "Aircraft",
                newName: "Aircrafts");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Aircrafts",
                table: "Aircrafts",
                column: "AircraftId");

            migrationBuilder.AddForeignKey(
                name: "FK_AircraftSeats_Aircrafts_AircraftId",
                table: "AircraftSeats",
                column: "AircraftId",
                principalTable: "Aircrafts",
                principalColumn: "AircraftId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_FlightSchedules_Aircrafts_AircraftId",
                table: "FlightSchedules",
                column: "AircraftId",
                principalTable: "Aircrafts",
                principalColumn: "AircraftId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AircraftSeats_Aircrafts_AircraftId",
                table: "AircraftSeats");

            migrationBuilder.DropForeignKey(
                name: "FK_FlightSchedules_Aircrafts_AircraftId",
                table: "FlightSchedules");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Aircrafts",
                table: "Aircrafts");

            migrationBuilder.RenameTable(
                name: "Aircrafts",
                newName: "Aircraft");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Aircraft",
                table: "Aircraft",
                column: "AircraftId");

            migrationBuilder.AddForeignKey(
                name: "FK_AircraftSeats_Aircraft_AircraftId",
                table: "AircraftSeats",
                column: "AircraftId",
                principalTable: "Aircraft",
                principalColumn: "AircraftId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_FlightSchedules_Aircraft_AircraftId",
                table: "FlightSchedules",
                column: "AircraftId",
                principalTable: "Aircraft",
                principalColumn: "AircraftId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
