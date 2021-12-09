using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MedHelper_API.Repository.Contracts
{
    public interface IBaseRepository<TEntity> 
    {
        Task<TEntity> GetById(int id);
        Task<TEntity> Create(TEntity item);
        Task Delete(TEntity item);
        Task Update(TEntity item);
        Task<List<TEntity>> GetAllWithoutParams();
        Task<List<TEntity>> GetByIds(List<int> ids);
    }
}