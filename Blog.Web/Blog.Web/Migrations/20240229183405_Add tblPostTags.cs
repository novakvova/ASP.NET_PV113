using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Blog.Web.Migrations
{
    /// <inheritdoc />
    public partial class AddtblPostTags : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "tblPostTags",
                columns: table => new
                {
                    PostId = table.Column<int>(type: "integer", nullable: false),
                    TagId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblPostTags", x => new { x.PostId, x.TagId });
                    table.ForeignKey(
                        name: "FK_tblPostTags_tblPosts_PostId",
                        column: x => x.PostId,
                        principalTable: "tblPosts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_tblPostTags_tblTags_TagId",
                        column: x => x.TagId,
                        principalTable: "tblTags",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_tblPostTags_TagId",
                table: "tblPostTags",
                column: "TagId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tblPostTags");
        }
    }
}
