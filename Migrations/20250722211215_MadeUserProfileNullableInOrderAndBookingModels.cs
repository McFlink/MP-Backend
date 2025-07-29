using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MP_Backend.Migrations
{
    /// <inheritdoc />
    public partial class MadeUserProfileNullableInOrderAndBookingModels : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bookings_UserProfiles_UserProfileId",
                table: "Bookings");

            migrationBuilder.DropForeignKey(
                name: "FK_Orders_UserProfiles_UserProfileId",
                table: "Orders");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "UserProfiles",
                type: "boolean",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "UserProfileId",
                table: "Orders",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AddColumn<string>(
                name: "CustomerEmailAtOrder",
                table: "Orders",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CustomerNameAtOrder",
                table: "Orders",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OrganizationNumberAtOrder",
                table: "Orders",
                type: "text",
                nullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "UserProfileId",
                table: "Bookings",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AddForeignKey(
                name: "FK_Bookings_UserProfiles_UserProfileId",
                table: "Bookings",
                column: "UserProfileId",
                principalTable: "UserProfiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_UserProfiles_UserProfileId",
                table: "Orders",
                column: "UserProfileId",
                principalTable: "UserProfiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bookings_UserProfiles_UserProfileId",
                table: "Bookings");

            migrationBuilder.DropForeignKey(
                name: "FK_Orders_UserProfiles_UserProfileId",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "CustomerEmailAtOrder",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "CustomerNameAtOrder",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "OrganizationNumberAtOrder",
                table: "Orders");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "UserProfiles",
                type: "boolean",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "boolean");

            migrationBuilder.AlterColumn<Guid>(
                name: "UserProfileId",
                table: "Orders",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "UserProfileId",
                table: "Bookings",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Bookings_UserProfiles_UserProfileId",
                table: "Bookings",
                column: "UserProfileId",
                principalTable: "UserProfiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_UserProfiles_UserProfileId",
                table: "Orders",
                column: "UserProfileId",
                principalTable: "UserProfiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
