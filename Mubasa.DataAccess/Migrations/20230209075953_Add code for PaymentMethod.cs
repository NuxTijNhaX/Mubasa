using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Mubasa.DataAccess.Migrations
{
    public partial class AddcodeforPaymentMethod : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Code",
                table: "PaymentMethods",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Code",
                table: "PaymentMethods");
        }
    }
}
