using System.Threading.Tasks;
using MedHelper_API.Dto.Auth;
using MedHelper_API.Responses;

namespace MedHelper_API.Service.Contracts
{
    public interface IAuthService
    {
        Task<AuthResponse> Login(LoginDto loginDto);
        Task<AuthResponse> Registration(RegistrationDto registrationDto);
        Task<bool> ValidateUser(LoginDto loginDto);
    }
}