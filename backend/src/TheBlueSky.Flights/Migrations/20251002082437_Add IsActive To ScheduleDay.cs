using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TheBlueSky.Flights.Migrations
{
    /// <inheritdoc />
    public partial class AddIsActiveToScheduleDay : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "ScheduleDays",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "ScheduleDays");
        }
    }
}
