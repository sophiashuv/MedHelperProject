using System.Threading.Tasks;
using MedHelper_EF.Models;

namespace MedHelper_API.Repository.Contracts
{
    public interface IDoctorRepository: IBaseRepository<Doctor>
    {
        Task<Doctor> GetByEmail(string email);
    }
}