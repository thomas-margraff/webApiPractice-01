using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DAL_SqlServer.Migrations
{
    public partial class modsymbolstable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Symbols",
                columns: table => new
                {
                    SymbolId = table.Column<int>(nullable: false),
                    Id = table.Column<int>(nullable: false),
                    SymbolCode = table.Column<string>(maxLength: 6, nullable: false),
                    IsActive = table.Column<bool>(nullable: false),
                    DateCreate = table.Column<DateTime>(nullable: false),
                    DateModify = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Symbols", x => x.SymbolId);
                });

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.DropTable(
                name: "Symbols");

        }
    }
}
