using Microsoft.EntityFrameworkCore.Migrations;

namespace CIN.DB.One.Migrations
{
    public partial class adding_proposals_tbl : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "tblOpProposalCosting",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProjectBudgetEstimationId = table.Column<int>(type: "int", nullable: false),
                    SkillSetCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    ItemEng = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ItemArab = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Qty = table.Column<int>(type: "int", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(17,3)", nullable: false),
                    Total = table.Column<decimal>(type: "decimal(17,3)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblOpProposalCosting", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "tblOpProposalTemplate",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SiteCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    ProjectCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    CustomerCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    TitleOfService = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CoveringLetter = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Commercial = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IssuingAuthority = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TitleOfServiceArb = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CoveringLetterArb = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CommercialArb = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NotesArb = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IssuingAuthorityArb = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedBy = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblOpProposalTemplate", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tblOpProposalCosting");

            migrationBuilder.DropTable(
                name: "tblOpProposalTemplate");
        }
    }
}
