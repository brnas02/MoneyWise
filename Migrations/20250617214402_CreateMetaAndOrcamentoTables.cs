using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DWebProjetoFinal.Migrations
{
    public partial class CreateMetaAndOrcamentoTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.CreateTable(
                name: "Metas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    Nome = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ValorAlvo = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    ValorAtual = table.Column<decimal>(type: "decimal(10,2)", nullable: false, defaultValue: 0m),
                    DataLimite = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Descricao = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Metas", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Orcamentos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    Categoria = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    LimiteMensal = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    GastoAtual = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    Mes = table.Column<int>(type: "int", nullable: false),
                    Ano = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orcamentos", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Orcamentos_UserId_Categoria_Mes_Ano",
                table: "Orcamentos",
                columns: new[] { "UserId", "Categoria", "Mes", "Ano" },
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Metas");

            migrationBuilder.DropTable(
                name: "Orcamentos");
        }
    }
}