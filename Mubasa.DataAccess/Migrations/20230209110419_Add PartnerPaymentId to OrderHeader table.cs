using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Mubasa.DataAccess.Migrations
{
    public partial class AddPartnerPaymentIdtoOrderHeadertable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PartnerPaymentId",
                table: "OrderHeaders",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PartnerPaymentId",
                table: "OrderHeaders");
        }
    }
}
