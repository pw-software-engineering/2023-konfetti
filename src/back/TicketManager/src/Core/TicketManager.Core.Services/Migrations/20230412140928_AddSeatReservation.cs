using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TicketManager.Core.Services.Migrations
{
    /// <inheritdoc />
    public partial class AddSeatReservation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SeatReservations",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    EventId = table.Column<Guid>(type: "uuid", nullable: false),
                    SectorName = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: false),
                    ReservedSeats = table.Column<int>(type: "integer", nullable: false),
                    DateModified = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    RowVersion = table.Column<byte[]>(type: "bytea", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SeatReservations", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SeatReservations");
        }
    }
}
