using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using MedHelper_API.Responses;
using MedHelper_EF.Models;

namespace MedHelper_API.Repository.Contracts
{
    public interface IPatientRepository: IBaseRepository<Patient>
    {
        Task<Patient> GetPatient(int userId, int patientId);
    }
}