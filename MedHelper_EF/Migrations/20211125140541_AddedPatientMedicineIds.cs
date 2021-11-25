using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MedHelper_EF.Migrations
{
    public partial class AddedPatientMedicineIds : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Patients_Doctors_DoctorID",
                table: "Patients");

            migrationBuilder.AlterColumn<int>(
                name: "DoctorID",
                table: "Patients",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AddColumn<List<int>>(
                name: "MedicineIds",
                table: "Patients",
                type: "integer[]",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Patients_Doctors_DoctorID",
                table: "Patients",
                column: "DoctorID",
                principalTable: "Doctors",
                principalColumn: "DoctorID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Patients_Doctors_DoctorID",
                table: "Patients");

            migrationBuilder.DropColumn(
                name: "MedicineIds",
                table: "Patients");

            migrationBuilder.AlterColumn<int>(
                name: "DoctorID",
                table: "Patients",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddForeignKey(
                name: "FK_Patients_Doctors_DoctorID",
                table: "Patients",
                column: "DoctorID",
                principalTable: "Doctors",
                principalColumn: "DoctorID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
