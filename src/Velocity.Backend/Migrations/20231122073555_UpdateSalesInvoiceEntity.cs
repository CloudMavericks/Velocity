using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Velocity.Backend.Migrations
{
    /// <inheritdoc />
    public partial class UpdateSalesInvoiceEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SalesInvoices_Customers_CustomerId",
                table: "SalesInvoices");

            migrationBuilder.AlterColumn<Guid>(
                name: "CustomerId",
                table: "SalesInvoices",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddColumn<Guid>(
                name: "PurchaseInvoiceItemId",
                table: "SalesInvoiceItem",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_SalesInvoiceItem_PurchaseInvoiceItemId",
                table: "SalesInvoiceItem",
                column: "PurchaseInvoiceItemId");

            migrationBuilder.AddForeignKey(
                name: "FK_SalesInvoiceItem_PurchaseInvoiceItem_PurchaseInvoiceItemId",
                table: "SalesInvoiceItem",
                column: "PurchaseInvoiceItemId",
                principalTable: "PurchaseInvoiceItem",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_SalesInvoices_Customers_CustomerId",
                table: "SalesInvoices",
                column: "CustomerId",
                principalTable: "Customers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SalesInvoiceItem_PurchaseInvoiceItem_PurchaseInvoiceItemId",
                table: "SalesInvoiceItem");

            migrationBuilder.DropForeignKey(
                name: "FK_SalesInvoices_Customers_CustomerId",
                table: "SalesInvoices");

            migrationBuilder.DropIndex(
                name: "IX_SalesInvoiceItem_PurchaseInvoiceItemId",
                table: "SalesInvoiceItem");

            migrationBuilder.DropColumn(
                name: "PurchaseInvoiceItemId",
                table: "SalesInvoiceItem");

            migrationBuilder.AlterColumn<Guid>(
                name: "CustomerId",
                table: "SalesInvoices",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_SalesInvoices_Customers_CustomerId",
                table: "SalesInvoices",
                column: "CustomerId",
                principalTable: "Customers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
