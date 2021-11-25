using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MedHelper_API.Repository.Contracts;
using MedHelper_EF.Models;
using Microsoft.EntityFrameworkCore;

namespace MedHelper_API.Repository
{
    public class PatientRepository : BaseRepository<Patient>, IPatientRepository
    {
        public PatientRepository(MedHelperDB context) : base(context)
        {
        }

        public async Task<List<Patient>> GetAll(int userId)
        {
            var result = await _context.Patients.Where(obj => obj.DoctorID == userId).ToListAsync();
            if (result == null) throw new KeyNotFoundException($"Patients hasn't been found.");

            return result;
        }

        public async Task<Patient> GetPatient(int userId, int patientId)
        {
            // var result = await _context.Patients.FirstOrDefaultAsync(obj =>
            //     obj.PatientID == patientId && obj.DoctorID == userId);
            //
            // // Console.WriteLine(result.);
            // var sql =
            //     $"SELECT P.\"PatientID\", P.\"UserName\", P.\"Gender\", P.\"DoctorID\", P.\"Birthdate\", M.\"MedicineID\", M.\"Name\", M.\"pharmacotherapeuticGroup\" FROM \"Patients\" P LEFT JOIN \"PatientMedicine\" PM on P.\"PatientID\" = PM.\"PatientID\" LEFT JOIN \"Medicines\" M on M.\"MedicineID\" = PM.\"MedicineID\" WHERE P.\"PatientID\" = {patientId}";
            // var result = _context.Patients.FromSqlRaw(sql);
            // var patient = result.FirstOrDefault();
            // var response = new Patient
            // {
            //     PatientID = 
            //     Birthdate = patient.Birthdate,
            //     DoctorID = patient.DoctorID,
            //     Gender = patient.Gender,
            //     
            // }

            if (result == null) throw new KeyNotFoundException($"Patients hasn't been found.");

            return result;
        }
    }
}