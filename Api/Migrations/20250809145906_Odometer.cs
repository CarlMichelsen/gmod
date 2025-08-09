using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Api.Migrations
{
    /// <inheritdoc />
    public partial class Odometer : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "odometer_trip",
                schema: "gmod",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    started_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    map = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    tag = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    ended_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_odometer_trip", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "odometer_data",
                schema: "gmod",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    parent_odometer_trip_id = table.Column<Guid>(type: "uuid", nullable: false),
                    positions = table.Column<string>(type: "text", nullable: false),
                    received_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_odometer_data", x => x.id);
                    table.ForeignKey(
                        name: "fk_odometer_data_odometer_trip_parent_odometer_trip_id",
                        column: x => x.parent_odometer_trip_id,
                        principalSchema: "gmod",
                        principalTable: "odometer_trip",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_odometer_data_parent_odometer_trip_id",
                schema: "gmod",
                table: "odometer_data",
                column: "parent_odometer_trip_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "odometer_data",
                schema: "gmod");

            migrationBuilder.DropTable(
                name: "odometer_trip",
                schema: "gmod");
        }
    }
}
