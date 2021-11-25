using System.Collections.Generic;
using System.Threading.Tasks;
using MedHelper_EF.Models;

namespace MedHelper_API.Repository.Contracts
{
    public interface IMedicineRepository: IBaseRepository<Medicine>
    {
        Task<List<Medicine>> GetByIds(List<int> ids);
    }
}