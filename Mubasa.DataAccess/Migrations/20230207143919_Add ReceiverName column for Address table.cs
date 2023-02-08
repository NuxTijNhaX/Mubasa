using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Mubasa.DataAccess.Migrations
{
    public partial class AddReceiverNamecolumnforAddresstable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "RecevierPhoneNumber",
                table: "OrderHeaders",
                newName: "ReceiverPhoneNumber");

            migrationBuilder.RenameColumn(
                name: "RecevierName",
                table: "OrderHeaders",
                newName: "ReceiverName");

            migrationBuilder.AddColumn<string>(
                name: "ReceiverName",
                schema: "Address",
                table: "Addresses",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ReceiverName",
                schema: "Address",
                table: "Addresses");

            migrationBuilder.RenameColumn(
                name: "ReceiverPhoneNumber",
                table: "OrderHeaders",
                newName: "RecevierPhoneNumber");

            migrationBuilder.RenameColumn(
                name: "ReceiverName",
                table: "OrderHeaders",
                newName: "RecevierName");
        }
    }
}
