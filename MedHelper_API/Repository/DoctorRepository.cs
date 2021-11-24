using System;
using System.Threading.Tasks;
using MedHelper_API.Repository.Contracts;
using MedHelper_EF.Models;
using Microsoft.EntityFrameworkCore;

namespace MedHelper_API.Repository
{
    public class DoctorRepository : BaseRepository<Doctor>, IDoctorRepository
    {
        public DoctorRepository(MedHelperDB context) : base(context)
        {
        }

        public async Task<Doctor> GetByEmail(string email)
        {
            var result = await _context.Doctors.FirstOrDefaultAsync(obj => obj.Email == email);
            
            return result;
        }

        public async Task<Doctor> GetById(int id)
        {
            var result = await _context.Doctors.FirstOrDefaultAsync(obj => obj.DoctorID == id);

            return result;
        }
    }
}