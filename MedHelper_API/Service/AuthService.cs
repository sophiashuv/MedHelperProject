using System.Collections.Generic;
using System.Security.Authentication;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using JwtAuth;
using MedHelper_API.Dto.Auth;
using MedHelper_API.Repository.Contracts;
using MedHelper_API.Responses;
using MedHelper_API.Service.Contracts;
using MedHelper_EF.Models;

namespace MedHelper_API.Service
{
    public class AuthService : IAuthService
    {
        private readonly IDoctorRepository _repository;
        private readonly IJwtAuthManager _jwtAuthManager;
        private readonly IMapper _mapper;
        
        public AuthService(IDoctorRepository repository, IMapper mapper, IJwtAuthManager jwtAuthManager)
        {
            _repository = repository;
            _jwtAuthManager = jwtAuthManager;
            _mapper = mapper;
        }
        
        private async Task<AuthResponse> GetAccessToken(Doctor user)
        { 
            // var userRoles = await _repository.GetUserRoles(user);
            
            var claims = new List<Claim>
            {
                new Claim("DoctorID", user.DoctorID.ToString()),
            };
            
            // claims.AddRange(userRoles.Select(role => new Claim(ClaimTypes.Role, role)));

            return await _jwtAuthManager.GenerateTokens(claims);
        }
        
        public async Task<bool> ValidateUser(LoginDto loginDto)
        {
            if (string.IsNullOrWhiteSpace(loginDto.Email) || string.IsNullOrWhiteSpace(loginDto.Pass))
                return false;
            
            var user =  await _repository.GetByEmail(loginDto.Email);

            return user is not null && BCrypt.Net.BCrypt.Verify(loginDto.Pass, user.Pass);
        }
        
        public async Task<AuthResponse> Login(LoginDto loginDto)
        {
            var user = await _repository.GetByEmail(loginDto.Email);
            return await GetAccessToken(user);
        }

        public async Task<AuthResponse> Registration(RegistrationDto registrationDto)
        {
            var isDuplicateEmail = await _repository.GetByEmail(registrationDto.Email);
            if (isDuplicateEmail != null)
                throw new AuthenticationException("There is already a user with this email address. Please log in.");
            
            var user = _mapper.Map<Doctor>(registrationDto);
            user.Pass = BCrypt.Net.BCrypt.HashPassword(registrationDto.Pass);
            await _repository.Create(user);

            return await GetAccessToken(user);
        }

        public async Task<Doctor> getInfo(int id)
        {
           var doctor = _repository.GetById(id);
            return doctor.Result;
        }
    }
}