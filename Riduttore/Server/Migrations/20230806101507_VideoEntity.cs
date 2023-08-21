using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Riduttore.Server.Migrations
{
    /// <inheritdoc />
    public partial class VideoEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Videos",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false, defaultValueSql: "NEWID()"),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Extension = table.Column<string>(type: "TEXT", nullable: false),
                    NativeVaultPath = table.Column<string>(type: "TEXT", nullable: true),
                    NativePhysicalVaultPath = table.Column<string>(type: "TEXT", nullable: true),
                    NativeSizeReadable = table.Column<string>(type: "TEXT", nullable: true),
                    ProcessedVaultPath = table.Column<string>(type: "TEXT", nullable: true),
                    ProcessedPhysicalVaultPath = table.Column<string>(type: "TEXT", nullable: true),
                    ProcessedSizeReadable = table.Column<string>(type: "TEXT", nullable: true),
                    ThumbnailVaultPath = table.Column<string>(type: "TEXT", nullable: true),
                    ThumbnailPhysicalVaultPath = table.Column<string>(type: "TEXT", nullable: true),
                    Command = table.Column<string>(type: "TEXT", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Videos", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Videos");
        }
    }
}
