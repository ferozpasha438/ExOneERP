using Microsoft.EntityFrameworkCore.Migrations;

namespace CIN.DB.One.Migrations
{
    public partial class modifying_custsalessetup_tbl : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CatPrefix",
                table: "tblSndDefCustomerCategory",
                type: "nvarchar(3)",
                maxLength: 3,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "LastSeq",
                table: "tblSndDefCustomerCategory",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "tblInvDefSalesConfig",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AutoGenCustCode = table.Column<bool>(type: "bit", nullable: false),
                    PrefixCatCode = table.Column<bool>(type: "bit", nullable: false),
                    NewCustIndicator = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    CustLength = table.Column<short>(type: "smallint", nullable: false),
                    CategoryLength = table.Column<short>(type: "smallint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblInvDefSalesConfig", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tblInvDefSalesConfig");

            migrationBuilder.DropColumn(
                name: "CatPrefix",
                table: "tblSndDefCustomerCategory");

            migrationBuilder.DropColumn(
                name: "LastSeq",
                table: "tblSndDefCustomerCategory");
        }
    }
}
