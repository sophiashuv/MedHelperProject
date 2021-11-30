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

        public Task<Medicine> GetMedicineWithInclude(int id)
        {
            throw new System.NotImplementedException();
        }

        //public async Task<Medicine> GetMedicineWithInclude(int id)
        //{
        //    return _context.Medicines
        //        .Include(obj => obj.MedicineCompositions)
        //        .Include(obj => obj.MedicineContraindications);
        //}
    }
}