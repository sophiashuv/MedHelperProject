using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MedHelper_API.Dto.Doctor;
using MedHelper_API.Responses;
using MedHelper_API.Service.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using MyApi.Controllers;

namespace MedHelper_API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/v1/doctor")]
    public class DoctorController: BaseController
    {
        private readonly IDoctorService _service;

        public DoctorController(IDoctorService service)
        {
            _service = service;
        }

        [HttpPut]
        public async Task<ActionResult> Update(UpdateDoctorDto doctor)
        {
            try
            {
                var userId = GetCurrentUserId();
                await _service.Update(doctor, userId);
            
                return NoContent();
            }
            catch (SecurityTokenValidationException e)
            {
                return Unauthorized(e.Message);
            }
        }
    }
}