using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using MedHelper_EF.Models;

namespace MedHelper_API.Repository.Contracts
{
    public interface IPatientRepository: IBaseRepository<Patient>
    {
        Task<List<Patient>> GetAll(int userId);
        Task<Patient> GetPatient(int userId, int patientId);
    }
}