using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MP_Backend.Migrations
{
    /// <inheritdoc />
    public partial class AddPropsForSoftDeletion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                table: "UserProfiles",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "UserProfiles",
                type: "boolean",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CustomerNameAtBooking",
                table: "Bookings",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EmailAtBooking",
                table: "Bookings",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NationalIdAtBooking",
                table: "Bookings",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DeletedAt",
                table: "UserProfiles");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "UserProfiles");

            migrationBuilder.DropColumn(
                name: "CustomerNameAtBooking",
                table: "Bookings");

            migrationBuilder.DropColumn(
                name: "EmailAtBooking",
                table: "Bookings");

            migrationBuilder.DropColumn(
                name: "NationalIdAtBooking",
                table: "Bookings");
        }
    }
}
