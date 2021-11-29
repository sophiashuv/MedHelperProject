using System.Collections.Generic;
using System.Threading.Tasks;
using MedHelper_EF.Models;

namespace MedHelper_API.Repository.Contracts
{
    public interface IMedicineRepository: IBaseRepository<Medicine>
    {
        Task<Medicine> GetMedicineWithInclude(int id);
        Task<List<Composition>> GetMedicineCompositions(List<int> id);
        Task<List<Contraindication>> GetMedicineContraindications(List<int> id);
        Task<List<MedicineInteraction>> GetMedicineInteraction(List<int> id);
    }
}