using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Mubasa.DataAccess.Migrations
{
    public partial class AddschemaforDefaultTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameTable(
                name: "DefaultAddresses",
                newName: "DefaultAddresses",
                newSchema: "Address");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameTable(
                name: "DefaultAddresses",
                schema: "Address",
                newName: "DefaultAddresses");
        }
    }
}
