using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HavucDent.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddLaboratoryTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ProductCommissionRate",
                table: "User",
                newName: "LaboratoryCommissionRate");

            migrationBuilder.CreateTable(
                name: "Laboratories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CompanyName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Laboratories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AppointmentLaboratories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AppointmentId = table.Column<int>(type: "int", nullable: false),
                    LaboratoryId = table.Column<int>(type: "int", nullable: false),
                    UnitPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppointmentLaboratories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AppointmentLaboratories_Appointments_AppointmentId",
                        column: x => x.AppointmentId,
                        principalTable: "Appointments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AppointmentLaboratories_Laboratories_LaboratoryId",
                        column: x => x.LaboratoryId,
                        principalTable: "Laboratories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AppointmentLaboratories_AppointmentId",
                table: "AppointmentLaboratories",
                column: "AppointmentId");

            migrationBuilder.CreateIndex(
                name: "IX_AppointmentLaboratories_LaboratoryId",
                table: "AppointmentLaboratories",
                column: "LaboratoryId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AppointmentLaboratories");

            migrationBuilder.DropTable(
                name: "Laboratories");

            migrationBuilder.RenameColumn(
                name: "LaboratoryCommissionRate",
                table: "User",
                newName: "ProductCommissionRate");
        }
    }
}
