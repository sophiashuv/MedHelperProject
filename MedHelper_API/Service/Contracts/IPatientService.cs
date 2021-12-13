using System.Collections.Generic;
using System.Threading.Tasks;
using MedHelper_API.Dto.Patient;
using MedHelper_API.Responses;

namespace MedHelper_API.Service.Contracts
{
    public interface IPatientService
    {
        Task<PatientResponse> Create(CreatePatientDto patient, int userId);
        Task Update(int patientId, UpdatePatientDto patient, int userId);
        Task Delete(int patientId, int userId);
    }
}