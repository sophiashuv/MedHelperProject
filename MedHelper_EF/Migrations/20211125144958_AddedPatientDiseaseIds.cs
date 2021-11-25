using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MedHelper_EF.Migrations
{
    public partial class AddedPatientDiseaseIds : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<List<int>>(
                name: "DiseasesIds",
                table: "Patients",
                type: "integer[]",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DiseasesIds",
                table: "Patients");
        }
    }
}
