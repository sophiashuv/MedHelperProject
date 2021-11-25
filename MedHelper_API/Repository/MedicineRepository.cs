using MedHelper_API.Repository.Contracts;
using MedHelper_EF.Models;

namespace MedHelper_API.Repository
{
    public class MedicineRepository : BaseRepository<Medicine>, IMedicineRepository
    {
        public MedicineRepository(MedHelperDB context) : base(context)
        {
        }
    }
}