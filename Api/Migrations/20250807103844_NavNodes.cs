using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Api.Migrations
{
    /// <inheritdoc />
    public partial class NavNodes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "nav_node",
                schema: "gmod",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    tag = table.Column<string>(type: "character varying(34)", maxLength: 34, nullable: false),
                    map = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    x = table.Column<int>(type: "integer", nullable: false),
                    y = table.Column<int>(type: "integer", nullable: false),
                    z = table.Column<int>(type: "integer", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_nav_node", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "nav_node_link",
                schema: "gmod",
                columns: table => new
                {
                    to_id = table.Column<Guid>(type: "uuid", nullable: false),
                    from_id = table.Column<Guid>(type: "uuid", nullable: false),
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_nav_node_link", x => new { x.to_id, x.from_id });
                    table.ForeignKey(
                        name: "fk_nav_node_link_nav_node_from_id",
                        column: x => x.from_id,
                        principalSchema: "gmod",
                        principalTable: "nav_node",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_nav_node_link_nav_node_to_id",
                        column: x => x.to_id,
                        principalSchema: "gmod",
                        principalTable: "nav_node",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_nav_node_map",
                schema: "gmod",
                table: "nav_node",
                column: "map");

            migrationBuilder.CreateIndex(
                name: "ix_nav_node_tag",
                schema: "gmod",
                table: "nav_node",
                column: "tag");

            migrationBuilder.CreateIndex(
                name: "ix_nav_node_link_from_id",
                schema: "gmod",
                table: "nav_node_link",
                column: "from_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "nav_node_link",
                schema: "gmod");

            migrationBuilder.DropTable(
                name: "nav_node",
                schema: "gmod");
        }
    }
}
