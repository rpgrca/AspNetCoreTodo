using Microsoft.EntityFrameworkCore.Migrations;

namespace TodoApi.Migrations
{
    public partial class AddApplicationUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ResponsibleId",
                table: "TodoItems",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "AspNetUsers",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_TodoItems_ResponsibleId",
                table: "TodoItems",
                column: "ResponsibleId");

            /*migrationBuilder.AddForeignKey(
                name: "FK_TodoItems_AspNetUsers_ResponsibleId",
                table: "TodoItems",
                column: "ResponsibleId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);*/
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            /*migrationBuilder.DropForeignKey(
                name: "FK_TodoItems_AspNetUsers_ResponsibleId",
                table: "TodoItems");*/

            migrationBuilder.DropIndex(
                name: "IX_TodoItems_ResponsibleId",
                table: "TodoItems");

            migrationBuilder.DropColumn(
                name: "ResponsibleId",
                table: "TodoItems");

            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "AspNetUsers");
        }
    }
}
