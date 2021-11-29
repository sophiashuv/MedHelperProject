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
            result.Medicines = new List<MedicineResponse>();
            result.Diseases = new List<DiseaseResponse>();
            foreach (var patientMedicine in patient.PatientMedicines)
            {
                var objMedicine = await _medicineRepository.GetMedicineWithInclude(patientMedicine.MedicineID);
                var medicineResponse = _mapper.Map<MedicineResponse>(objMedicine);
                
                var objCompositions = await _medicineRepository.GetMedicineCompositions(objMedicine.MedicineCompositions.Select(obj => 
                    obj.CompositionID).ToList());
                medicineResponse.Compositions = objCompositions.Select(obj => 
                    _mapper.Map<CompositionResponse>(obj)).ToList();
                
                var objContraindications = await _medicineRepository.GetMedicineContraindications(objMedicine.MedicineContraindications.Select(obj => 
                    obj.ContraindicationID).ToList());
                medicineResponse.Contraindications = objContraindications.Select(obj => 
                    _mapper.Map<ContraindicationResponse>(obj)).ToList();
                
                var objInteractions = await _medicineRepository.GetMedicineInteraction(objMedicine.MedicineInteractions.Select(obj => 
                    obj.MedicineInteractionID).ToList());
                medicineResponse.Interactions = objInteractions.Select(obj => 
                    _mapper.Map<MedicineInteractionResponse>(obj)).ToList();
                
                result.Medicines.Add(medicineResponse);
            }
                
            foreach (var patientDisease in patient.PatientDiseases)
            {
                var objMedicine = await _diseaseRepository.GetById(patientDisease.DiseaseID);
                result.Diseases.Add(_mapper.Map<DiseaseResponse>(objMedicine));
            }
            return result;
        }

        public async Task<List<PatientResponse>> GetAll(int userId)
        {
            var patients = await _patientRepository.GetAll(userId);
            var response = new List<PatientResponse>();
            
            foreach (var patient in patients)
            {
                var patientResponse = _mapper.Map<PatientResponse>(patient);
                patientResponse.Medicines = new List<MedicineResponse>();
                patientResponse.Diseases = new List<DiseaseResponse>();
                
                foreach (var patientMedicine in patient.PatientMedicines)
                {
                    var objMedicine = await _medicineRepository.GetMedicineWithInclude(patientMedicine.MedicineID);
                    var medicineResponse = _mapper.Map<MedicineResponse>(objMedicine);
                    
                    var objCompositions = await _medicineRepository.GetMedicineCompositions(objMedicine.MedicineCompositions.Select(obj => 
                        obj.CompositionID).ToList());
                    medicineResponse.Compositions = objCompositions.Select(obj => 
                        _mapper.Map<CompositionResponse>(obj)).ToList();
                
                    var objContraindications = await _medicineRepository.GetMedicineContraindications(objMedicine.MedicineContraindications.Select(obj => 
                        obj.ContraindicationID).ToList());
                    medicineResponse.Contraindications = objContraindications.Select(obj => 
                        _mapper.Map<ContraindicationResponse>(obj)).ToList();
                
                    var objInteractions = await _medicineRepository.GetMedicineInteraction(objMedicine.MedicineInteractions.Select(obj => 
                        obj.MedicineInteractionID).ToList());
                    medicineResponse.Interactions = objInteractions.Select(obj => 
                        _mapper.Map<MedicineInteractionResponse>(obj)).ToList();


                    patientResponse.Medicines.Add(medicineResponse);
                }
                
                foreach (var patientDisease in patient.PatientDiseases)
                {
                    var objMedicine = await _diseaseRepository.GetById(patientDisease.DiseaseID);
                    patientResponse.Diseases.Add(_mapper.Map<DiseaseResponse>(objMedicine));
                }

                response.Add(patientResponse);
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