using System.Collections.Generic;
using System.Threading.Tasks;
using MedHelper_EF.Models;

namespace MedHelper_API.Repository.Contracts
{
    public interface IMedicineRepository: IBaseRepository<Medicine>
    {
        Task<List<Medicine>> GetMedicineWithInclude(List<int> ids);
        Task<List<Composition>> GetMedicineCompositions(List<int> ids);
        Task<List<Contraindication>> GetMedicineContraindications(List<int> ids);
        Task<List<MedicineInteraction>> GetMedicineInteraction(List<int> ids);
        Task<List<TEntity>> GetAllTEntity<TEntity>() where TEntity : BaseEntity;
    }
}