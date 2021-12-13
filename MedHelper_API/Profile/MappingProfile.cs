using AutoMapper;
using MedHelper_API.Dto.Auth;
using MedHelper_API.Dto.Doctor;
using MedHelper_API.Dto.Patient;
using MedHelper_API.Repository;
using MedHelper_API.Responses;
using MedHelper_EF.Models;

namespace MedHelper_API.Profile
{
    public class MappingProfile : AutoMapper.Profile
    {
        public MappingProfile()
        {
            CreateMap<Doctor, RegistrationDto>();
            CreateMap<RegistrationDto, Doctor>();
            
            CreateMap<CreatePatientDto, Patient>();
            CreateMap<Patient, CreatePatientDto>();
            CreateMap<UpdatePatientDto, Patient>();
            CreateMap<Patient, UpdatePatientDto>();
            CreateMap<PatientResponse, Patient>();
            CreateMap<Patient, PatientResponse>();

            CreateMap<Doctor, DoctorResponse>();
            CreateMap<DoctorResponse, Doctor>();
            CreateMap<Doctor, UpdateDoctorDto>();
            CreateMap<UpdateDoctorDto, Doctor>();
            
            CreateMap<Medicine, MedicineResponse>();
            CreateMap<MedicineResponse, Medicine>();
            CreateMap<Composition, CompositionResponse>();
            CreateMap<CompositionResponse, Composition>();
            CreateMap<Contraindication, ContraindicationResponse>();
            CreateMap<ContraindicationResponse, Contraindication>();
            CreateMap<DiseaseResponse, Disease>();
            CreateMap<Disease, DiseaseResponse>();
            CreateMap<MedicineInteractionResponse, MedicineInteraction>();
            CreateMap<MedicineInteraction, MedicineInteractionResponse>();
        }
    }
}