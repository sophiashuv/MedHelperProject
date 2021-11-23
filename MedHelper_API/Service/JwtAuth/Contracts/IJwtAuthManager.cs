using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using MedHelper_API.Dto.Auth;
using MedHelper_API.Responses;

namespace JwtAuth
{
    public interface IJwtAuthManager
    {
        Task<AuthResponse> GenerateTokens(List<Claim> claims);
    }
}