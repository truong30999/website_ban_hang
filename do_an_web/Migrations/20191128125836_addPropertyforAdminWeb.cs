using Microsoft.EntityFrameworkCore.Migrations;

namespace do_an_web.Migrations
{
    public partial class addPropertyforAdminWeb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Admins",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Phone",
                table: "Admins",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "Admins",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Email",
                table: "Admins");

            migrationBuilder.DropColumn(
                name: "Phone",
                table: "Admins");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Admins");
        }
    }
}
