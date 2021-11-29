using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using MedHelper_API.Responses;
using MedHelper_API.Service.Contracts;
using MedHelper_EF.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MyApi.Controllers;

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
    }
}