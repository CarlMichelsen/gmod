using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Api.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreateGmod : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "gmod");

            migrationBuilder.CreateTable(
                name: "image",
                schema: "gmod",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    content_id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    size_x = table.Column<int>(type: "integer", nullable: false),
                    size_y = table.Column<int>(type: "integer", nullable: false),
                    creator_steam_id64 = table.Column<decimal>(type: "numeric(20,0)", nullable: true),
                    source = table.Column<string>(type: "text", nullable: false),
                    content_hash_code = table.Column<int>(type: "integer", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_image", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "image_content",
                schema: "gmod",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    data = table.Column<byte[]>(type: "bytea", nullable: false),
                    image_id = table.Column<Guid>(type: "uuid", nullable: false),
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_image_content", x => x.id);
                    table.ForeignKey(
                        name: "fk_image_content_image_image_id",
                        column: x => x.image_id,
                        principalSchema: "gmod",
                        principalTable: "image",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_image_content_hash_code",
                schema: "gmod",
                table: "image",
                column: "content_hash_code");

            migrationBuilder.CreateIndex(
                name: "ix_image_content_image_id",
                schema: "gmod",
                table: "image_content",
                column: "image_id",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "image_content",
                schema: "gmod");

            migrationBuilder.DropTable(
                name: "image",
                schema: "gmod");
        }
    }
}
