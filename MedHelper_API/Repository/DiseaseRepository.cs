using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MedHelper_API.Repository.Contracts;
using MedHelper_EF.Models;
using Microsoft.EntityFrameworkCore;

namespace MedHelper_API.Repository
{
    public class DiseaseRepository : BaseRepository<Disease>, IDiseaseRepository
    {
        public DiseaseRepository(MedHelperDB context) : base(context)
        {
        }
        
        public async Task<List<Disease>> GetByIds(List<int> ids)
        {
            var result = await _context.Diseases.Where(obj => ids.Contains(obj.DiseaseID)).ToListAsync();

            return result;
        }
    }
}