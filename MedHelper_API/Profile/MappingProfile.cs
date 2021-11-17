using AutoMapper;
using MedHelper_API.Dto.Auth;
using MedHelper_EF.Models;

namespace MedHelper_API.Profile
{
    public class MappingProfile : AutoMapper.Profile
    {
        public MappingProfile()
        {
            CreateMap<Doctor, RegistrationDto>();
            CreateMap<RegistrationDto, Doctor>();
        }
    }
}