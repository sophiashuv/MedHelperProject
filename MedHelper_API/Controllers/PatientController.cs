using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MedHelper_API.Dto.Patient;
using MedHelper_API.Responses;
using MedHelper_API.Service.Contracts;
using MedHelper_EF.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
        private readonly MedHelperDB _context;

        public PatientController(IPatientService patientService, MedHelperDB context)
        {
            _patientService = patientService;
            _context = context;
        }
        
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PatientResponse>>> GetAllPatients()
        {
            try
            {
                var userId = GetCurrentUserId();

                var result = _context.Patients
                    .Include(obj => obj.PatientDiseases)
                    .Include(obj => obj.PatientMedicines)
                    .Where(obj => obj.DoctorID == userId)
                    .Select(obj => new
                    {
                        obj.PatientID,
                        obj.DoctorID,
                        obj.Birthdate,
                        obj.Gender,
                        obj.UserName,
                        Diseases = obj.PatientDiseases.Select(d => d.Disease.Title),
                        Medicines = obj.PatientMedicines.Select(medicine => new
                        {
                            medicine.Medicine.MedicineID,
                            medicine.Medicine.Name,
                            medicine.Medicine.pharmacotherapeuticGroup,
                            MedicineCompositions =
                                medicine.Medicine.MedicineCompositions.Select(c => c.Composition.Description),
                            MedicineInteractions = medicine.Medicine.MedicineInteractions.Select(mi => new
                            {
                                CompositionDescription = mi.Composition.Description,
                                mi.Description
                            }),
                            MedicineContraindications =
                                medicine.Medicine.MedicineContraindications.Select(
                                    mc => mc.Contraindication.Description)
                        })
                    });
                
                return Ok(result);
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

                var result = _context.Patients
                    .Include(obj => obj.PatientDiseases)
                    .Include(obj => obj.PatientMedicines)
                    .Select(obj => new
                    {
                        obj.PatientID,
                        obj.DoctorID,
                        obj.Birthdate,
                        obj.Gender,
                        obj.UserName,
                        Diseases = obj.PatientDiseases.Select(d => d.Disease.Title),
                        Medicines = obj.PatientMedicines.Select(medicine => new
                        {
                            medicine.Medicine.MedicineID,
                            medicine.Medicine.Name,
                            medicine.Medicine.pharmacotherapeuticGroup,
                            MedicineCompositions =
                                medicine.Medicine.MedicineCompositions.Select(c => c.Composition.Description),
                            MedicineInteractions = medicine.Medicine.MedicineInteractions.Select(mi => new
                            {
                                CompositionDescription = mi.Composition.Description,
                                mi.Description
                            }),
                            MedicineContraindications =
                                medicine.Medicine.MedicineContraindications.Select(
                                    mc => mc.Contraindication.Description)
                        })
                    }).FirstOrDefault(obj => obj.DoctorID == userId && obj.PatientID == id);

                if (result is null)
                    return NotFound("Patient hasn't been found.");
            
                return Ok(result);
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
        public async Task<ActionResult> Update(int id, UpdatePatientDto patientDto)
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