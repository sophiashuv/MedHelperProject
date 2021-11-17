using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MedHelper_API.Dto.Patient;
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
    [Route("api/v1/patient")]
    public class PatientController : BaseController
    {
        private readonly IPatientService _patientService;

        public PatientController(IPatientService patientService)
        {
            _patientService = patientService;
        }
        
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PatientResponse>>> GetAllPatients()
        {
            try
            {
                var userId = GetCurrentUserId();
                var patients = await _patientService.GetAll(userId);
                
                return Ok(patients);
            }
            catch (SecurityTokenValidationException e)
            {
                return Unauthorized(e.Message);
            }
        }
        
        [HttpGet("{id}")]
        public async Task<ActionResult<PatientResponse>> GetOne(int id)
        {
            try
            {
                var userId = GetCurrentUserId();
                
                var product = await _patientService.GetOne(id, userId);

                if (product is null)
                    return NotFound("Product hasn't been found.");
            
                return Ok(product);
            }
            catch (SecurityTokenValidationException e)
            {
                return Unauthorized(e.Message);
            }
        }
        
        [HttpPost]
        public async Task<ActionResult<PatientResponse>> Create(CreatePatientDto patientDto)
        {
            try
            {
                var userId = GetCurrentUserId();
                var patient = await _patientService.Create(patientDto, userId);
            
                return CreatedAtAction(nameof(GetOne), new { id = patient.PatientID }, patient);
            }
            catch (SecurityTokenValidationException e)
            {
                return Unauthorized(e.Message);
            }
        }
        
        [HttpPut("{id}")]
        public async Task<ActionResult<PatientResponse>> Update(int id, UpdatePatientDto patientDto)
        {
            try
            {
                var userId = GetCurrentUserId();
                await _patientService.Update(id, patientDto, userId);
            
                return NoContent();
            }
            catch (SecurityTokenValidationException e)
            {
                return Unauthorized(e.Message);
            }
            catch (KeyNotFoundException e)
            {
                return NotFound(e.Message);
            }
        }
        
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                var userId = GetCurrentUserId();
                await _patientService.Delete(id, userId);
                
                return NoContent();
            }
            catch (SecurityTokenValidationException e)
            {
                return Unauthorized(e.Message);
            }
            catch (KeyNotFoundException e)
            {
                return NotFound(e.Message);
            }
        }
    }
}