using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HDBackend.Migrations
{
    /// <inheritdoc />
    public partial class TicketStatus : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsCancelled",
                table: "Tickets",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsOpen",
                table: "Tickets",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsResolved",
                table: "Tickets",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "OnHold",
                table: "Tickets",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsCancelled",
                table: "Tickets");

            migrationBuilder.DropColumn(
                name: "IsOpen",
                table: "Tickets");

            migrationBuilder.DropColumn(
                name: "IsResolved",
                table: "Tickets");

            migrationBuilder.DropColumn(
                name: "OnHold",
                table: "Tickets");
        }
    }
}
