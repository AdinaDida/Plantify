using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Migrations
{
    public partial class DeleteRole : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
               name: "Role",
               table: "AspNetUsers");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
