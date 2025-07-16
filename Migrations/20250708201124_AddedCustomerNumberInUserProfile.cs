using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MP_Backend.Migrations
{
    /// <inheritdoc />
    public partial class AddedCustomerNumberInUserProfile : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CustomerNumber",
                table: "UserProfiles",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CustomerNumber",
                table: "UserProfiles");
        }
    }
}
