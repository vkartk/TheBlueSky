using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TheBlueSky.Flights.Migrations
{
    /// <inheritdoc />
    public partial class RemoveSeatClass : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AircraftSeats_SeatClasses_SeatClassId",
                table: "AircraftSeats");

            migrationBuilder.DropTable(
                name: "SeatClasses");

            migrationBuilder.DropIndex(
                name: "IX_AircraftSeats_SeatClassId",
                table: "AircraftSeats");

            migrationBuilder.RenameColumn(
                name: "SeatClassId",
                table: "AircraftSeats",
                newName: "SeatClass");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "SeatClass",
                table: "AircraftSeats",
                newName: "SeatClassId");

            migrationBuilder.CreateTable(
                name: "SeatClasses",
                columns: table => new
                {
                    SeatClassId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ClassDescription = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    ClassName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    PriorityOrder = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SeatClasses", x => x.SeatClassId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AircraftSeats_SeatClassId",
                table: "AircraftSeats",
                column: "SeatClassId");

            migrationBuilder.AddForeignKey(
                name: "FK_AircraftSeats_SeatClasses_SeatClassId",
                table: "AircraftSeats",
                column: "SeatClassId",
                principalTable: "SeatClasses",
                principalColumn: "SeatClassId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
