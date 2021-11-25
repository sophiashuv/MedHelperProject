using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MedHelper_API.Repository.Contracts;
using MedHelper_EF.Models;
using Microsoft.EntityFrameworkCore;

namespace MedHelper_API.Repository
{
    public class MedicineRepository : BaseRepository<Medicine>, IMedicineRepository
    {
        public MedicineRepository(MedHelperDB context) : base(context)
        {
        }

        public async Task<List<Medicine>> GetByIds(List<int> ids)
        {
            var result = await _context.Medicines.Where(obj => ids.Contains(obj.MedicineID)).ToListAsync();

            return result;
        }
    }
}