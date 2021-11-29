using System.Threading.Tasks;
using MedHelper_API.Repository.Contracts;

namespace MedHelper_API.Repository
{
    public class DataRepository : IDataRepository
    {
        public Task<TEntity> GetTEntityById<TEntity>(int id)
        {
            throw new System.NotImplementedException();
        }
    }
}