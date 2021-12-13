using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using MedHelper_API.Repository.Contracts;
using MedHelper_API.Responses;
using MedHelper_EF.Models;
using Microsoft.EntityFrameworkCore;

namespace MedHelper_API.Repository
{
    public class PatientRepository : BaseRepository<Patient>, IPatientRepository
    {
        public PatientRepository(MedHelperDB context) : base(context)
        {
        }

        public async Task<Patient> GetPatient(int userId, int patientId)
        {
            var result = await _context.Patients
                .Include(obj => obj.PatientMedicines)
                .Include(obj =>obj.PatientDiseases)
                .FirstOrDefaultAsync(obj => obj.PatientID == patientId && obj.DoctorID == userId);
            
            if (result == null) throw new KeyNotFoundException($"Patient hasn't been found.");

            return result;
        }
    }
}