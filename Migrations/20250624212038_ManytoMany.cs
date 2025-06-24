using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DWebProjetoFinal.Migrations
{
    /// <inheritdoc />
    public partial class ManytoMany : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UserTransacao",
                columns: table => new
                {
                    UserAccountId = table.Column<int>(type: "int", nullable: false),
                    TransacaoId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserTransacao", x => new { x.UserAccountId, x.TransacaoId });
                    table.ForeignKey(
                        name: "FK_UserTransacao_Transacoes_TransacaoId",
                        column: x => x.TransacaoId,
                        principalTable: "Transacoes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserTransacao_UserAccounts_UserAccountId",
                        column: x => x.UserAccountId,
                        principalTable: "UserAccounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserTransacao_TransacaoId",
                table: "UserTransacao",
                column: "TransacaoId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserTransacao");
        }
    }
}
