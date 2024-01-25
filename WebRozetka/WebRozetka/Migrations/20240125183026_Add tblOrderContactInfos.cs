using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebRozetka.Migrations
{
    /// <inheritdoc />
    public partial class AddtblOrderContactInfos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "tblOrderContactInfos",
                columns: table => new
                {
                    OrderId = table.Column<int>(type: "integer", nullable: false),
                    FirstName = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    LastName = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Phone = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblOrderContactInfos", x => x.OrderId);
                    table.ForeignKey(
                        name: "FK_tblOrderContactInfos_tblOrders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "tblOrders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tblOrderContactInfos");
        }
    }
}
