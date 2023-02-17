using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Mubasa.DataAccess.Migrations
{
    public partial class Addnamespaces : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "Production");

            migrationBuilder.EnsureSchema(
                name: "Sales");

            migrationBuilder.RenameTable(
                name: "Suppliers",
                newName: "Suppliers",
                newSchema: "Production");

            migrationBuilder.RenameTable(
                name: "ShoppingItems",
                newName: "ShoppingItems",
                newSchema: "Sales");

            migrationBuilder.RenameTable(
                name: "Publishers",
                newName: "Publishers",
                newSchema: "Production");

            migrationBuilder.RenameTable(
                name: "Products",
                newName: "Products",
                newSchema: "Production");

            migrationBuilder.RenameTable(
                name: "PaymentMethods",
                newName: "PaymentMethods",
                newSchema: "Sales");

            migrationBuilder.RenameTable(
                name: "OrderHeaders",
                newName: "OrderHeaders",
                newSchema: "Sales");

            migrationBuilder.RenameTable(
                name: "OrderDetails",
                newName: "OrderDetails",
                newSchema: "Sales");

            migrationBuilder.RenameTable(
                name: "CoverTypes",
                newName: "CoverTypes",
                newSchema: "Production");

            migrationBuilder.RenameTable(
                name: "Categories",
                newName: "Categories",
                newSchema: "Production");

            migrationBuilder.RenameTable(
                name: "Authors",
                newName: "Authors",
                newSchema: "Production");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameTable(
                name: "Suppliers",
                schema: "Production",
                newName: "Suppliers");

            migrationBuilder.RenameTable(
                name: "ShoppingItems",
                schema: "Sales",
                newName: "ShoppingItems");

            migrationBuilder.RenameTable(
                name: "Publishers",
                schema: "Production",
                newName: "Publishers");

            migrationBuilder.RenameTable(
                name: "Products",
                schema: "Production",
                newName: "Products");

            migrationBuilder.RenameTable(
                name: "PaymentMethods",
                schema: "Sales",
                newName: "PaymentMethods");

            migrationBuilder.RenameTable(
                name: "OrderHeaders",
                schema: "Sales",
                newName: "OrderHeaders");

            migrationBuilder.RenameTable(
                name: "OrderDetails",
                schema: "Sales",
                newName: "OrderDetails");

            migrationBuilder.RenameTable(
                name: "CoverTypes",
                schema: "Production",
                newName: "CoverTypes");

            migrationBuilder.RenameTable(
                name: "Categories",
                schema: "Production",
                newName: "Categories");

            migrationBuilder.RenameTable(
                name: "Authors",
                schema: "Production",
                newName: "Authors");
        }
    }
}
