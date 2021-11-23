using System.Security.Authentication;
using System.Threading.Tasks;
using MedHelper_API.Dto.Auth;
using MedHelper_API.Responses;
using MedHelper_API.Service.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyApi.Controllers;

namespace MedHelper_API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/v1/auth")]
    public class AuthController: BaseController
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<ActionResult<AuthResponse>> Login(LoginDto loginDto)
        {
            if (!await _authService.ValidateUser(loginDto))
                return Unauthorized("Password or email is invalid");

            var response = await _authService.Login(loginDto);
                
            return StatusCode(201, response);
        }
        
        [AllowAnonymous]
        [HttpPost("registration")]
        public async Task<ActionResult<AuthResponse>> Registration(RegistrationDto registrationDto)
        {
            try
            {
                var response = await _authService.Registration(registrationDto);
                
                return StatusCode(201, response);
            }
            catch (AuthenticationException e)
            {
                return Unauthorized(e.Message);
            }
        }
    }
}
