using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Mubasa.DataAccess.Migrations
{
    public partial class UpdatePublisherSupplierTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn("LogoUrl", "Publishers");
            migrationBuilder.DropColumn("Description", "Suppliers");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
