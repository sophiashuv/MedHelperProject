using System.Collections.Generic;
using System.Threading.Tasks;
using MedHelper_EF.Models;

namespace MedHelper_API.Repository.Contracts
{
    public interface IDataRepository
    {
        Task<TEntity> GetTEntityById<TEntity>(int id);
    }
}