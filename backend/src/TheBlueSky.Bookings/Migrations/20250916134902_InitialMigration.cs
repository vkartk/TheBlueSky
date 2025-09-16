using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TheBlueSky.Bookings.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Bookings",
                columns: table => new
                {
                    BookingId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    FlightId = table.Column<int>(type: "int", nullable: false),
                    BookingDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    NumberOfPassengers = table.Column<int>(type: "int", nullable: false),
                    SubtotalAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TaxAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    BookingStatus = table.Column<int>(type: "int", nullable: false),
                    PaymentStatus = table.Column<int>(type: "int", nullable: false),
                    LastUpdated = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bookings", x => x.BookingId);
                });

            migrationBuilder.CreateTable(
                name: "MealPreferences",
                columns: table => new
                {
                    MealPreferenceId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PreferenceName = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    PreferenceDescription = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MealPreferences", x => x.MealPreferenceId);
                });

            migrationBuilder.CreateTable(
                name: "Passengers",
                columns: table => new
                {
                    PassengerId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ManagedByUserId = table.Column<int>(type: "int", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    DateOfBirth = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Gender = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: true),
                    PassportNumber = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: true),
                    NationalityCountryId = table.Column<string>(type: "nvarchar(2)", maxLength: 2, nullable: true),
                    RelationshipToManager = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Passengers", x => x.PassengerId);
                });

            migrationBuilder.CreateTable(
                name: "BookingAudits",
                columns: table => new
                {
                    BookingAuditId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BookingId = table.Column<int>(type: "int", nullable: false),
                    ActionType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OldValuesJson = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NewValuesJson = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PerformedByUserId = table.Column<int>(type: "int", nullable: false),
                    AuditTimestamp = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BookingAudits", x => x.BookingAuditId);
                    table.ForeignKey(
                        name: "FK_BookingAudits_Bookings_BookingId",
                        column: x => x.BookingId,
                        principalTable: "Bookings",
                        principalColumn: "BookingId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BookingCancellations",
                columns: table => new
                {
                    BookingCancellationId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BookingId = table.Column<int>(type: "int", nullable: false),
                    CancellationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CancelledByUserId = table.Column<int>(type: "int", nullable: false),
                    RefundAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    RefundStatus = table.Column<int>(type: "int", nullable: false),
                    RefundDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CancellationReason = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AdminNotes = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BookingCancellations", x => x.BookingCancellationId);
                    table.ForeignKey(
                        name: "FK_BookingCancellations_Bookings_BookingId",
                        column: x => x.BookingId,
                        principalTable: "Bookings",
                        principalColumn: "BookingId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Payments",
                columns: table => new
                {
                    PaymentId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BookingId = table.Column<int>(type: "int", nullable: false),
                    PaymentMethod = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: false),
                    PaymentAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PaymentDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    PaymentStatus = table.Column<int>(type: "int", nullable: false),
                    GatewayTransactionId = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: true),
                    RefundDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RefundAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Payments", x => x.PaymentId);
                    table.ForeignKey(
                        name: "FK_Payments_Bookings_BookingId",
                        column: x => x.BookingId,
                        principalTable: "Bookings",
                        principalColumn: "BookingId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BookingPassengers",
                columns: table => new
                {
                    BookingPassengerId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BookingId = table.Column<int>(type: "int", nullable: false),
                    PassengerId = table.Column<int>(type: "int", nullable: false),
                    FlightSeatStatusId = table.Column<int>(type: "int", nullable: false),
                    TicketNumber = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: false),
                    TicketPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    MealPreferenceId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BookingPassengers", x => x.BookingPassengerId);
                    table.ForeignKey(
                        name: "FK_BookingPassengers_Bookings_BookingId",
                        column: x => x.BookingId,
                        principalTable: "Bookings",
                        principalColumn: "BookingId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BookingPassengers_MealPreferences_MealPreferenceId",
                        column: x => x.MealPreferenceId,
                        principalTable: "MealPreferences",
                        principalColumn: "MealPreferenceId",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_BookingPassengers_Passengers_PassengerId",
                        column: x => x.PassengerId,
                        principalTable: "Passengers",
                        principalColumn: "PassengerId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BookingAudits_BookingId",
                table: "BookingAudits",
                column: "BookingId");

            migrationBuilder.CreateIndex(
                name: "IX_BookingCancellations_BookingId",
                table: "BookingCancellations",
                column: "BookingId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_BookingPassengers_BookingId",
                table: "BookingPassengers",
                column: "BookingId");

            migrationBuilder.CreateIndex(
                name: "IX_BookingPassengers_MealPreferenceId",
                table: "BookingPassengers",
                column: "MealPreferenceId");

            migrationBuilder.CreateIndex(
                name: "IX_BookingPassengers_PassengerId",
                table: "BookingPassengers",
                column: "PassengerId");

            migrationBuilder.CreateIndex(
                name: "IX_Payments_BookingId",
                table: "Payments",
                column: "BookingId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BookingAudits");

            migrationBuilder.DropTable(
                name: "BookingCancellations");

            migrationBuilder.DropTable(
                name: "BookingPassengers");

            migrationBuilder.DropTable(
                name: "Payments");

            migrationBuilder.DropTable(
                name: "MealPreferences");

            migrationBuilder.DropTable(
                name: "Passengers");

            migrationBuilder.DropTable(
                name: "Bookings");
        }
    }
}
