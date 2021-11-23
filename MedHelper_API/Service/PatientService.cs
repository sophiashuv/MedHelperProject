using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using JwtAuth;
using MedHelper_API.Dto.Patient;
using MedHelper_API.Repository.Contracts;
using MedHelper_API.Responses;
using MedHelper_API.Service.Contracts;
using MedHelper_EF.Models;

namespace MedHelper_API.Service
{
    public class PatientService: IPatientService
    {
        private readonly IPatientRepository _repository;
        private readonly IMapper _mapper;
        
        public PatientService(IPatientRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<PatientResponse> GetOne(int patientId, int userId)
        {
            var patient = await _repository.GetPatient(userId, patientId);
            var result = _mapper.Map<PatientResponse>(patient);
            
            return result;
        }

        public async Task<List<PatientResponse>> GetAll(int userId)
        {
            var patient = await _repository.GetAll(userId);
            var result = _mapper.Map<List<Patient>, List<PatientResponse>>(patient);
            
            return result;
        }

        public async Task<PatientResponse> Create(CreatePatientDto patient, int userId)
        {
            var mappedPatient = _mapper.Map<Patient>(patient);
            mappedPatient.DoctorID = userId;

            await _repository.Create(mappedPatient);
            var result = _mapper.Map<PatientResponse>(mappedPatient);

            return result;
        }

        public async Task Update(int patientId, UpdatePatientDto patient, int userId)
        {
            var exPatient = await _repository.GetPatient(userId, patientId);

            _mapper.Map(patient, exPatient);
            await _repository.Update(exPatient);
        }

        public async Task Delete(int patientId, int userId)
        {
            var exPatient = await _repository.GetPatient(userId, patientId);

            await _repository.Delete(exPatient);
        }
    }
}