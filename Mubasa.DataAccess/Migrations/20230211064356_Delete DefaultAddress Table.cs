using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Mubasa.DataAccess.Migrations
{
    public partial class DeleteDefaultAddressTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Addresses_AspNetUsers_ApplicationUserId",
                schema: "Address",
                table: "Addresses");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_DefaultAddresses_DefaultAddressId",
                table: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "DefaultAddresses",
                schema: "Address");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_DefaultAddressId",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_Addresses_ApplicationUserId",
                schema: "Address",
                table: "Addresses");

            migrationBuilder.AddColumn<int>(
                name: "AddressId",
                table: "AspNetUsers",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ApplicationUserId",
                schema: "Address",
                table: "Addresses",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_DefaultAddressId",
                table: "AspNetUsers",
                column: "DefaultAddressId",
                unique: true,
                filter: "[DefaultAddressId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Addresses_DefaultAddressId",
                table: "AspNetUsers",
                column: "DefaultAddressId",
                principalSchema: "Address",
                principalTable: "Addresses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Addresses_DefaultAddressId",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_DefaultAddressId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "AddressId",
                table: "AspNetUsers");

            migrationBuilder.AlterColumn<string>(
                name: "ApplicationUserId",
                schema: "Address",
                table: "Addresses",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateTable(
                name: "DefaultAddresses",
                schema: "Address",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AddressId = table.Column<int>(type: "int", nullable: false),
                    ApplicationUserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DefaultAddresses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DefaultAddresses_Addresses_AddressId",
                        column: x => x.AddressId,
                        principalSchema: "Address",
                        principalTable: "Addresses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DefaultAddresses_AspNetUsers_ApplicationUserId",
                        column: x => x.ApplicationUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_DefaultAddressId",
                table: "AspNetUsers",
                column: "DefaultAddressId");

            migrationBuilder.CreateIndex(
                name: "IX_Addresses_ApplicationUserId",
                schema: "Address",
                table: "Addresses",
                column: "ApplicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_DefaultAddresses_AddressId",
                schema: "Address",
                table: "DefaultAddresses",
                column: "AddressId");

            migrationBuilder.CreateIndex(
                name: "IX_DefaultAddresses_ApplicationUserId",
                schema: "Address",
                table: "DefaultAddresses",
                column: "ApplicationUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Addresses_AspNetUsers_ApplicationUserId",
                schema: "Address",
                table: "Addresses",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_DefaultAddresses_DefaultAddressId",
                table: "AspNetUsers",
                column: "DefaultAddressId",
                principalSchema: "Address",
                principalTable: "DefaultAddresses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
