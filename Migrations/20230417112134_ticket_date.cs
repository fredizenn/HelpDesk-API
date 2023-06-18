using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HDBackend.Migrations
{
    /// <inheritdoc />
    public partial class ticketdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "Tickets",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "Tickets");
        }
    }
}
