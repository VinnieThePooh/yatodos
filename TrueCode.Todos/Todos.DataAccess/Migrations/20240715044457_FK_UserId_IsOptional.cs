using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Todos.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class FK_UserId_IsOptional : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Todos_AppUsers_UserId",
                table: "Todos");

            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "Todos",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddForeignKey(
                name: "FK_Todos_AppUsers_UserId",
                table: "Todos",
                column: "UserId",
                principalTable: "AppUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Todos_AppUsers_UserId",
                table: "Todos");

            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "Todos",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Todos_AppUsers_UserId",
                table: "Todos",
                column: "UserId",
                principalTable: "AppUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
