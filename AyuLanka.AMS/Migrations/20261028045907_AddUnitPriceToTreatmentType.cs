using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AyuLanka.AMS.Migrations
{
    /// <inheritdoc />
    public partial class AddUnitPriceToTreatmentType : Migration
    {
        // The Company / DoctorSession schema was already applied to the database manually and
        // is only captured in the model snapshot, so this migration adds UnitPrice alone.
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "UnitPrice",
                table: "TreatmentTypes",
                type: "decimal(9,2)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UnitPrice",
                table: "TreatmentTypes");
        }
    }
}
