using System.Threading.Tasks;
using MedHelper_API.Dto.Doctor;

namespace MedHelper_API.Service.Contracts
{
    public interface IDoctorService
    {
        Task Update(UpdateDoctorDto doctor, int userId);
        Task Delete(int userId);
    }
}