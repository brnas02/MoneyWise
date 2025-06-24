using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DWebProjetoFinal.Migrations
{
    /// <inheritdoc />
    public partial class RemoveUserID : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Transacoes");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "Transacoes",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
