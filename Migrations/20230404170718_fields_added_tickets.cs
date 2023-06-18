using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HDBackend.Migrations
{
    /// <inheritdoc />
    public partial class fieldsaddedtickets : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ContactName",
                table: "Tickets",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ContactPhoneNumber",
                table: "Tickets",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TicketDescription",
                table: "Tickets",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ContactName",
                table: "Tickets");

            migrationBuilder.DropColumn(
                name: "ContactPhoneNumber",
                table: "Tickets");

            migrationBuilder.DropColumn(
                name: "TicketDescription",
                table: "Tickets");
        }
    }
}
