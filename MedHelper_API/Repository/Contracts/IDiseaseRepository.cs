using System.Collections.Generic;
using System.Threading.Tasks;
using MedHelper_EF.Models;

namespace MedHelper_API.Repository.Contracts
{
    public interface IDiseaseRepository : IBaseRepository<Disease>
    {
        Task<List<Disease>> GetByIds(List<int> ids);
    }
}