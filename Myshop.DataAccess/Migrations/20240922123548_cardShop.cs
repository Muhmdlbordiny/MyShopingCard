using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Myshop.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class cardShop : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ShopingCards",
                columns: table => new
                {
                    ShopingCardId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    Count = table.Column<int>(type: "int", nullable: false),
                    ApplicationUserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShopingCards", x => x.ShopingCardId);
                    table.ForeignKey(
                        name: "FK_ShopingCards_AspNetUsers_ApplicationUserId",
                        column: x => x.ApplicationUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ShopingCards_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ShopingCards_ApplicationUserId",
                table: "ShopingCards",
                column: "ApplicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_ShopingCards_ProductId",
                table: "ShopingCards",
                column: "ProductId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ShopingCards");
        }
    }
}
