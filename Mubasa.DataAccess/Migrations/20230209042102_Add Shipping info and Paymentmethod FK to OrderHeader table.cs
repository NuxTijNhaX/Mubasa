using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Mubasa.DataAccess.Migrations
{
    public partial class AddShippinginfoandPaymentmethodFKtoOrderHeadertable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "District",
                table: "OrderHeaders");

            migrationBuilder.DropColumn(
                name: "Province",
                table: "OrderHeaders");

            migrationBuilder.DropColumn(
                name: "Ward",
                table: "OrderHeaders");

            migrationBuilder.AddColumn<double>(
                name: "Discount",
                table: "OrderHeaders",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<int>(
                name: "DistrictId",
                table: "OrderHeaders",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "PaymentMethodId",
                table: "OrderHeaders",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ProvinceId",
                table: "OrderHeaders",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<double>(
                name: "ShippingCost",
                table: "OrderHeaders",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<string>(
                name: "ShippingInfo",
                table: "OrderHeaders",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "WardId",
                table: "OrderHeaders",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_OrderHeaders_DistrictId",
                table: "OrderHeaders",
                column: "DistrictId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderHeaders_PaymentMethodId",
                table: "OrderHeaders",
                column: "PaymentMethodId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderHeaders_ProvinceId",
                table: "OrderHeaders",
                column: "ProvinceId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderHeaders_WardId",
                table: "OrderHeaders",
                column: "WardId");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderHeaders_Districts_DistrictId",
                table: "OrderHeaders",
                column: "DistrictId",
                principalSchema: "Address",
                principalTable: "Districts",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_OrderHeaders_PaymentMethods_PaymentMethodId",
                table: "OrderHeaders",
                column: "PaymentMethodId",
                principalTable: "PaymentMethods",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_OrderHeaders_Provinces_ProvinceId",
                table: "OrderHeaders",
                column: "ProvinceId",
                principalSchema: "Address",
                principalTable: "Provinces",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_OrderHeaders_Wards_WardId",
                table: "OrderHeaders",
                column: "WardId",
                principalSchema: "Address",
                principalTable: "Wards",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderHeaders_Districts_DistrictId",
                table: "OrderHeaders");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderHeaders_PaymentMethods_PaymentMethodId",
                table: "OrderHeaders");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderHeaders_Provinces_ProvinceId",
                table: "OrderHeaders");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderHeaders_Wards_WardId",
                table: "OrderHeaders");

            migrationBuilder.DropIndex(
                name: "IX_OrderHeaders_DistrictId",
                table: "OrderHeaders");

            migrationBuilder.DropIndex(
                name: "IX_OrderHeaders_PaymentMethodId",
                table: "OrderHeaders");

            migrationBuilder.DropIndex(
                name: "IX_OrderHeaders_ProvinceId",
                table: "OrderHeaders");

            migrationBuilder.DropIndex(
                name: "IX_OrderHeaders_WardId",
                table: "OrderHeaders");

            migrationBuilder.DropColumn(
                name: "Discount",
                table: "OrderHeaders");

            migrationBuilder.DropColumn(
                name: "DistrictId",
                table: "OrderHeaders");

            migrationBuilder.DropColumn(
                name: "PaymentMethodId",
                table: "OrderHeaders");

            migrationBuilder.DropColumn(
                name: "ProvinceId",
                table: "OrderHeaders");

            migrationBuilder.DropColumn(
                name: "ShippingCost",
                table: "OrderHeaders");

            migrationBuilder.DropColumn(
                name: "ShippingInfo",
                table: "OrderHeaders");

            migrationBuilder.DropColumn(
                name: "WardId",
                table: "OrderHeaders");

            migrationBuilder.AddColumn<string>(
                name: "District",
                table: "OrderHeaders",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Province",
                table: "OrderHeaders",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Ward",
                table: "OrderHeaders",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
