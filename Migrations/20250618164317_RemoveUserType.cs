using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DWebProjetoFinal.Migrations
{
    /// <inheritdoc />
    public partial class RemoveUserType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Type",
                table: "UserAccounts");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Type",
                table: "UserAccounts",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
