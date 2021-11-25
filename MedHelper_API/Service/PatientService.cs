using System;
using System.Collections.Generic;
using System.Linq;
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
        private readonly IPatientRepository _patientRepository;
        private readonly IMedicineRepository _medicineRepository;
        private readonly IMapper _mapper;
        
        public PatientService(IPatientRepository patientRepository, IMedicineRepository medicineRepository, IMapper mapper) {
            _patientRepository = patientRepository;
            _medicineRepository = medicineRepository;
            _mapper = mapper;
        }

        public async Task<PatientResponse> GetOne(int patientId, int userId)
        {
            var patient = await _patientRepository.GetPatient(userId, patientId);
            var result = _mapper.Map<PatientResponse>(patient);
            result.Medicines = patient.PatientMedicines.Select(obj => 
                _mapper.Map<MedicineResponse>(obj.Medicine)).ToList();
            
            Console.WriteLine(patient.PatientMedicines.Count);
            
            return result;
        }

        public async Task<List<PatientResponse>> GetAll(int userId)
        {
            var patient = await _patientRepository.GetAll(userId);
                
            var result = patient.Select(obj => new PatientResponse
            {
                PatientID = obj.PatientID,
                UserName = obj.UserName,
                Birthdate = obj.Birthdate,
                DoctorID = obj.DoctorID,
                Gender = obj.Gender,
                Medicines = obj.PatientMedicines.Select(ob =>
                    _mapper.Map<MedicineResponse>(ob.Medicine)
                ).ToList()
            }).ToList();

            return result;
        }

        public async Task<PatientResponse> Create(CreatePatientDto patient, int userId)
        {
            var mappedPatient = _mapper.Map<Patient>(patient);
            mappedPatient.DoctorID = userId;
            mappedPatient.PatientMedicines = new List<PatientMedicine>();

            var createdPatient = await _patientRepository.Create(mappedPatient);

            foreach (var medicineId in patient.MedicineIds)
            {
                var medicine = await _medicineRepository.GetById(medicineId);
                var newMedicine = new PatientMedicine
                {
                    Patient = createdPatient,
                    Medicine = medicine
                };
                createdPatient.PatientMedicines.Add(newMedicine);
            }
            
            await _patientRepository.Update(createdPatient);
            
            var result = _mapper.Map<PatientResponse>(createdPatient);
            result.Medicines = createdPatient.PatientMedicines.Select(obj => 
                    _mapper.Map<MedicineResponse>(obj.Medicine)
                ).ToList();
            
            return result;
        }

        public async Task Update(int patientId, UpdatePatientDto patient, int userId)
        {
            var exPatient = await _patientRepository.GetPatient(userId, patientId);
            _mapper.Map(patient, exPatient);

            foreach (var medicineId in patient.MedicineIds)
            {
                var medicine = await _medicineRepository.GetById(medicineId);
                var newMedicine = new PatientMedicine
                {
                    Patient = exPatient,
                    Medicine = medicine
                };
                exPatient.PatientMedicines.Add(newMedicine);
            }
            
            await _patientRepository.Update(exPatient);
        }

        public async Task Delete(int patientId, int userId)
        {
            var exPatient = await _patientRepository.GetPatient(userId, patientId);
            // foreach (var medicineId in exPatient.PatientMedicines)
            // {
            //     await _patientRepository.
            // }

            await _patientRepository.Delete(exPatient);
        }
    }
}