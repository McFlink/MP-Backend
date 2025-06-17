using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MP_Backend.Migrations
{
    /// <inheritdoc />
    public partial class AddOrderConfirmEmaailPropAndRemoveIsRetailerFlag : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsRetailer",
                table: "UserProfiles");

            migrationBuilder.AddColumn<bool>(
                name: "OrderConfirmationEmailSent",
                table: "Orders",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OrderConfirmationEmailSent",
                table: "Orders");

            migrationBuilder.AddColumn<bool>(
                name: "IsRetailer",
                table: "UserProfiles",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }
    }
}
