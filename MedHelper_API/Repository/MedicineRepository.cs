using System;
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

        public async Task<Medicine> GetMedicineWithInclude(int id)
        {
            var result = await _context.Medicines
                .Include(obj => obj.MedicineCompositions)
                .Include(obj => obj.MedicineContraindications)
                .Include(obj => obj.MedicineInteractions)
                .FirstOrDefaultAsync(obj => obj.MedicineID == id);
            return result;
        }

        public async Task<List<Composition>> GetMedicineCompositions(List<int> ids)
        {
            var result = await _context.Compositions
                .Where(obj => ids.Contains(obj.CompositionID)).ToListAsync();
            return result;
        }
        
        public async Task<List<Contraindication>> GetMedicineContraindications(List<int> ids)
        {
            var result = await _context.Contraindications
                .Where(obj => ids.Contains(obj.ContraindicationID)).ToListAsync();
            return result;
        }
        
        public async Task<List<MedicineInteraction>> GetMedicineInteraction(List<int> ids)
        {
            var result = await _context.MedicineInteraction
                .Where(obj => ids.Contains(obj.MedicineInteractionID)).ToListAsync();
            return result;
        }
    }
}