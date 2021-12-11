using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using MedHelper_API.Responses;
using MedHelper_API.Service.Contracts;
using MedHelper_EF.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

using Microsoft.IdentityModel.Tokens;
using MyApi.Controllers;
using Newtonsoft.Json;

namespace MedHelper_API.Controllers
{
    // [Authorize]
    [ApiController]
    [Route("api/v1")]
    public class DataController : BaseController
    {
        private readonly MedHelperDB _context;
        private readonly IMapper _mapper;
        private readonly ILogger<DataController> _logger;

        public DataController(MedHelperDB context, IMapper mapper, ILogger<DataController> logger)
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpGet("medicines")]
        public async Task<ActionResult> GetAllMedicines() // <IEnumerable<MedicineResponse>>
        {
            var medicines = await _context.Medicines
                .Include(obj => obj.MedicineCompositions)
                .Include(obj => obj.MedicineContraindications)
                .Include(obj => obj.MedicineInteractions)
                .Select(obj => new
                {
                    obj.MedicineID,
                    obj.Name,
                    obj.pharmacotherapeuticGroup,
                    Compositions = obj.MedicineCompositions.Select(c => c.Composition.Description),
                    Contraindications = obj.MedicineContraindications.Select(c => c.Contraindication.Description),
                    MedicineInteractions = obj.MedicineInteractions.Select(mi => new
                    {
                        CompositionDescription = mi.Composition.Description,
                        mi.Description
                    })
                })
                .ToListAsync();
            // var result = _mapper.Map<List<Medicine>, List<MedicineResponse>>(medicines);
            _logger.LogInformation("Returned all medicines.");
            return Ok(medicines);
        }

        [HttpGet("medicines/{id}")]
        public async Task<ActionResult> GetMedicine(int id) // <IEnumerable<MedicineResponse>>
        {
            var medicine = await _context.Medicines
                .Include(obj => obj.MedicineCompositions)
                .Include(obj => obj.MedicineContraindications)
                .Include(obj => obj.MedicineInteractions)
                .Select(obj => new
                {
                    obj.MedicineID,
                    obj.Name,
                    obj.pharmacotherapeuticGroup,
                    Compositions = obj.MedicineCompositions.Select(c => c.Composition.Description),
                    Contraindications = obj.MedicineContraindications.Select(c => c.Contraindication.Description),
                    MedicineInteractions = obj.MedicineInteractions.Select(mi => new
                    {
                        CompositionDescription = mi.Composition.Description,
                        mi.Description
                    })
                })
                .FirstOrDefaultAsync(obj => obj.MedicineID == id);
            _logger.LogInformation($"Returned medicine with id {id}.");
            return Ok(medicine);
        }

        [HttpGet("diseases")]
        public async Task<ActionResult<IEnumerable<MedicineResponse>>> GetAllDiseases()
        {
            var diseases = await _context.Diseases.ToListAsync();
            var result = _mapper.Map<List<Disease>, List<DiseaseResponse>>(diseases);
            _logger.LogInformation("Returned all diseases.");
            return Ok(result);
        }

        [HttpGet("diseases/{id}")]
        public async Task<ActionResult<IEnumerable<MedicineResponse>>> GetDisease(int id)
        {
            var diseases = await _context.Diseases.FirstOrDefaultAsync(obj => obj.DiseaseID == id);
            _logger.LogInformation($"Returned disease with {id}.");
            return Ok(new
            {
                diseases.DiseaseID,
                diseases.Title
            });
        }

        [HttpGet("patient/{id}/allmedicines")]
        public async Task<ActionResult> Search(int id)
        {
            var user = GetCurrentUserId();

            var patient = await _context.Patients
                .Include(obj => obj.PatientDiseases)
                .FirstOrDefaultAsync(obj => obj.PatientID == id && obj.DoctorID == user);

            if (patient is null)
            {
                return NotFound();
            }

            var patientDiseases = await _context.Diseases.Where(obj =>
                patient.PatientDiseases.Select(o => o.DiseaseID).Contains(obj.DiseaseID)).ToListAsync();

            // var response = _context.Medicines
            //     .Include(obj => obj.MedicineCompositions)
            //     .Include(obj => obj.MedicineContraindications)
            //     .Include(obj => obj.MedicineInteractions)
            //     .Select(obj => new
            //     {
            //         obj.MedicineID,
            //         obj.Name,
            //         obj.pharmacotherapeuticGroup,
            //         Compositions = obj.MedicineCompositions.Select(c => c.Composition.Description),
            //         Contraindications = obj.MedicineContraindications.Select(c => c.Contraindication.Description),
            //         MedicineInteractions = obj.MedicineInteractions.Select(mi => new
            //         {
            //             CompositionDescription = mi.Composition.Description,
            //             mi.Description
            //         })
            //     })
            //     .Where(obj => !patientDiseases.Contains(obj.Compositions));

            var medicines = await _context.Medicines
                .Include(obj => obj.MedicineCompositions)
                .Include(obj => obj.MedicineContraindications)
                .Include(obj => obj.MedicineInteractions)
                .ToListAsync();

            var compositions = await _context.Compositions.ToListAsync();
            var contraindications = await _context.Contraindications.ToListAsync();
            var interactions = await _context.MedicineInteraction.Include(obj => obj.Composition).ToListAsync();
            var responseData = medicines.Select(obj => new
            {
                obj.Name,
                obj.pharmacotherapeuticGroup,
                obj.MedicineID,
                Compositions = compositions.Where(x => obj.MedicineCompositions
                .FirstOrDefault(y => y.CompositionID == x.CompositionID) != null).Select(x => x.Description),
                Contraindications = contraindications.Where(x => obj.MedicineContraindications
                .FirstOrDefault(y => y.ContraindicationID == x.ContraindicationID) != null).Select(x => x.Description),
                MedicineInteractions = interactions.Where(x => obj.MedicineInteractions
                .FirstOrDefault(y => y.MedicineInteractionID == x.MedicineInteractionID) != null).Select(x => new
                {
                    Description = x.Description,
                    CompositionDescription = x.Composition.Description
                })
            });
            //var responseData = from medicine in medicines
            //                   from composition in medicine.MedicineCompositions.DefaultIfEmpty()
            //                   join comp in compositions on composition.CompositionID equals comp.CompositionID
            //                   from contraindication in medicine.MedicineContraindications.DefaultIfEmpty()
            //                   join contr in contraindications on contraindication.ContraindicationID equals contr.ContraindicationID
            //                   from interaction in medicine.MedicineInteractions.DefaultIfEmpty()
            //                   join intr in interactions on interaction.MedicineInteractionID equals intr.MedicineInteractionID 
            //                   select new
            //                   {
            //                       medicine.MedicineID,
            //                       medicine.Name,
            //                       medicine.pharmacotherapeuticGroup,
            //                       CompositionDescription = comp?.Description ?? String.Empty,
            //                       ContraindicationDescription = contr?.Description ?? String.Empty,
            //                       MedicineInteractionDescription = new
            //                       {
            //                           Description = intr?.Description ?? String.Empty,
            //                           CompositionDescription = intr?.Composition.Description ?? String.Empty
            //                       }
            //                   };
            //var InteractionList = new[] { new { Description = "", CompositionDescription = "" } }.ToList();
            //InteractionList.Clear();
            //var response = from medicine in responseData
            //               group medicine by new { medicine.MedicineID, medicine.Name, medicine.pharmacotherapeuticGroup }
            //    into grouped
            //               select new
            //               {
            //                   grouped.Key.MedicineID,
            //                   grouped.Key.Name,
            //                   grouped.Key.pharmacotherapeuticGroup,
            //                   Compositions = grouped.Select(obj => obj.CompositionDescription)?.Distinct() ?? new List<string>(),
            //                   Contraindications = grouped.Select(obj => obj.ContraindicationDescription)?.Distinct() ?? new List<string>(),
            //                   MedicineInteractions = grouped.Select(obj => obj.MedicineInteractionDescription)?.Distinct() ?? InteractionList
            //               };
            foreach (var item in patientDiseases)
            {
                responseData = responseData.Where(x => !x.Contraindications.Contains(item.Title));
            }
            _logger.LogInformation($"Returned list of medicines according to the search and contraindications of a patient with id {id}.");
            return Ok(responseData);
        }
    }
}