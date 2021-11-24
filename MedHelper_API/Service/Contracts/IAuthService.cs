using System.Threading.Tasks;
using MedHelper_API.Dto.Auth;
using MedHelper_API.Responses;
using MedHelper_EF.Models;

namespace MedHelper_API.Service.Contracts
{
    public interface IAuthService
    {
        Task<AuthResponse> Login(LoginDto loginDto);
        Task<AuthResponse> Registration(RegistrationDto registrationDto);
        Task<bool> ValidateUser(LoginDto loginDto);

        Task<Doctor> getInfo(int id);
    }
}