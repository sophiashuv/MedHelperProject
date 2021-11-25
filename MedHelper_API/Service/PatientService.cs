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
        private readonly IDiseaseRepository _diseaseRepository;
        private readonly IMapper _mapper;
        
        public PatientService(IPatientRepository patientRepository, IMedicineRepository medicineRepository, IMapper mapper, IDiseaseRepository diseaseRepository) {
            _patientRepository = patientRepository;
            _medicineRepository = medicineRepository;
            _mapper = mapper;
            _diseaseRepository = diseaseRepository;
        }

        public async Task<PatientResponse> GetOne(int patientId, int userId)
        {
            var patient = await _patientRepository.GetPatient(userId, patientId);
            var result = _mapper.Map<PatientResponse>(patient);
            var medicines = await _medicineRepository.GetByIds(patient.MedicineIds);
            var diseases = await _diseaseRepository.GetByIds(patient.DiseasesIds);
            
            result.Medicines = medicines.Select(o => _mapper.Map<MedicineResponse>(o)).ToList();
            result.Diseases = diseases.Select(d => _mapper.Map<DiseaseResponse>(d)).ToList();

            return result;
        }

        public async Task<List<PatientResponse>> GetAll(int userId)
        {
            var patient = await _patientRepository.GetAll(userId);

            var response = new List<PatientResponse>();
            foreach (var p in patient)
            {
                var obj = _mapper.Map<PatientResponse>(p);
                var medicines = await _medicineRepository.GetByIds(p.MedicineIds);
                var diseases = await _diseaseRepository.GetByIds(p.DiseasesIds);
                
                obj.Medicines =  medicines.Select(o => _mapper.Map<MedicineResponse>(o)).ToList();
                obj.Diseases = diseases.Select(d => _mapper.Map<DiseaseResponse>(d)).ToList();
                
                response.Add(obj);
            }

            return response;
        }

        public async Task<PatientResponse> Create(CreatePatientDto patient, int userId)
        {
            var mappedPatient = _mapper.Map<Patient>(patient);
            mappedPatient.DoctorID = userId;
            var createdPatient = await _patientRepository.Create(mappedPatient);
            
            foreach (var medicineId in patient.MedicineIds)
            {
                createdPatient.PatientMedicines.Add(new PatientMedicine
                {
                    Patient = createdPatient,
                    Medicine = await _medicineRepository.GetById(medicineId)
                });
            }
            
            foreach (var diseaseId in patient.DiseasesIds)
            {
                createdPatient.PatientDiseases.Add(new PatientDisease
                {
                    Patient = createdPatient,
                    Disease = await _diseaseRepository.GetById(diseaseId)
                });
            }
            
            await _patientRepository.Update(createdPatient);
            
            var result = _mapper.Map<PatientResponse>(createdPatient);
            result.Medicines = createdPatient.PatientMedicines.Select(obj => 
                    _mapper.Map<MedicineResponse>(obj.Medicine)
                ).ToList();
            result.Diseases = createdPatient.PatientDiseases.Select(obj => 
                _mapper.Map<DiseaseResponse>(obj.Disease)
            ).ToList();
            
            return result;
        }

        public async Task Update(int patientId, UpdatePatientDto patient, int userId)
        {
            var exPatient = await _patientRepository.GetPatient(userId, patientId);
            _mapper.Map(patient, exPatient);

            // foreach (var medicineId in patient.MedicineIds)
            // {
            //     var medicine = await _medicineRepository.GetById(medicineId);
            //     var newMedicine = new PatientMedicine
            //     {
            //         Patient = exPatient,
            //         Medicine = medicine
            //     };
            //     exPatient.PatientMedicines.Add(newMedicine);
            // }
            
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