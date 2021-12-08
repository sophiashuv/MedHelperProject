using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using MedHelper_API.Responses;
using MedHelper_API.Service.Contracts;
using MedHelper_EF.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MyApi.Controllers;
using Newtonsoft.Json;

namespace MedHelper_API.Controllers
{
    // [Authorize]
    [ApiController]
    [Route("api/v1")]
    public class DataController: BaseController
    {
        private readonly MedHelperDB _context;
        private readonly IMapper _mapper;

        public DataController(MedHelperDB context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        
        [HttpGet("medicines")]
        public async Task<ActionResult<IEnumerable<MedicineResponse>>> GetAllMedicines()
        {
            var medicines = await _context.Medicines.ToListAsync();
            var result = _mapper.Map<List<Medicine>, List<MedicineResponse>>(medicines);

            return Ok(result);
        }
        
        [HttpGet("diseases")]
        public async Task<ActionResult<IEnumerable<MedicineResponse>>> GetAllDiseases()
        {
            var diseases = await _context.Diseases.ToListAsync();
            var result = _mapper.Map<List<Disease>, List<DiseaseResponse>>(diseases);

            return Ok(result);
        }
        
        [HttpGet("patient/{id}/search={search}")]
        public async Task<ActionResult> Search(int id, string search)
        {
            var user = GetCurrentUserId();
            // search = "а";
            
            var patient = await _context.Patients
                .Include(obj => obj.PatientDiseases)
                .FirstOrDefaultAsync(obj => obj.PatientID == id && obj.DoctorID == user);
            
            if (patient is null)
            {
                return NotFound();
            }

            var patientDiseases = await _context.Diseases.Where(obj =>
                patient.PatientDiseases.Select(o => o.DiseaseID).Contains(obj.DiseaseID)).ToListAsync();

            var medicines = await _context.Medicines
                .Include(obj => obj.MedicineCompositions)
                .Include(obj => obj.MedicineContraindications)
                .Include(obj => obj.MedicineInteractions)
                .Where(obj => obj.pharmacotherapeuticGroup.ToLower().Contains(search.ToLower()))
                .ToListAsync();

            var compositions = await _context.Compositions.ToListAsync();
            var contraindications = await _context.Contraindications.ToListAsync();
            var interactions = await _context.MedicineInteraction.Include(obj => obj.Composition).ToListAsync();

            var responseData = from medicine in medicines
                from composition in medicine.MedicineCompositions
                join comp in compositions on composition.CompositionID equals comp.CompositionID
                from contraindication in medicine.MedicineContraindications
                join contr in contraindications on contraindication.ContraindicationID equals contr.ContraindicationID
                from interaction in medicine.MedicineInteractions
                join intr in interactions on interaction.MedicineInteractionID equals intr.MedicineInteractionID
                where !patientDiseases.Select(obj => obj.Title).Contains(comp.Description)
                select new
                {
                    medicine.MedicineID,
                    medicine.Name,
                    medicine.pharmacotherapeuticGroup,
                    CompositionDescription = comp.Description,
                    ContraindicationDescription = contr.Description,
                    MedicineInteractionDescription = intr.Description
                };

            var response = from medicine in responseData
                group medicine by new { medicine.MedicineID, medicine.Name, medicine.pharmacotherapeuticGroup }
                into grouped
                select new
                {
                    grouped.Key.MedicineID,
                    grouped.Key.Name,
                    grouped.Key.pharmacotherapeuticGroup,
                    Compositions = grouped.Select(obj => obj.CompositionDescription).Distinct(),
                    Contraindications = grouped.Select(obj => obj.ContraindicationDescription).Distinct(),
                    MedicineInteractions = grouped.Select(obj => obj.MedicineInteractionDescription).Distinct()
                };
            
            return Ok(response);
        }
    }
}