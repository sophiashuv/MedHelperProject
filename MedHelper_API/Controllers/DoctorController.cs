using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MedHelper_API.Dto.Doctor;
using MedHelper_API.Responses;
using MedHelper_API.Service.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
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
        private readonly ILogger<DoctorController> _logger;

        public DoctorController(IDoctorService service, ILogger<DoctorController> logger)
        {
            _service = service;
            _logger = logger;
        }

        [HttpPut]
        public async Task<ActionResult> Update(UpdateDoctorDto doctor)
        {
            try
            {
                var userId = GetCurrentUserId();
                await _service.Update(doctor, userId);
                _logger.LogInformation($"Updated doctor with id {userId}.");
                return NoContent();
            }
            catch (SecurityTokenValidationException e)
            {
                return Unauthorized(e.Message);
            }
        }
    }
}