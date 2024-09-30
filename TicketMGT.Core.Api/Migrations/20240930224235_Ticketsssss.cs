using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TicketMGT.Core.Api.Migrations
{
    /// <inheritdoc />
    public partial class Ticketsssss : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ParentTicketId",
                table: "Tickets");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ParentTicketId",
                table: "Tickets",
                type: "uniqueidentifier",
                nullable: true);
        }
    }
}
