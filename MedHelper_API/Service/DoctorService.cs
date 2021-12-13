using System.Threading.Tasks;
using AutoMapper;
using MedHelper_API.Dto.Doctor;
using MedHelper_API.Repository.Contracts;
using MedHelper_API.Service.Contracts;
using MedHelper_EF.Models;

namespace MedHelper_API.Service
{
    public class DoctorService : IDoctorService
    {
        private readonly IDoctorRepository _repository;
        private readonly IMapper _mapper;
        
        public DoctorService(IDoctorRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }
        
        public async Task Update(UpdateDoctorDto doctor, int userId)
        {
            var updatedDoctor = _mapper.Map<Doctor>(doctor);
            updatedDoctor.DoctorID = userId;
            updatedDoctor.Pass = BCrypt.Net.BCrypt.HashPassword(doctor.Pass);
            await _repository.Update(updatedDoctor);
        }

        public async Task Delete(int userId)
        {
            var doctor = await _repository.GetById(userId);
            await _repository.Delete(doctor);
        }
    }
}