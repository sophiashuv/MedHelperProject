using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MedHelper_API.Repository.Contracts;
using MedHelper_EF.Models;
using Microsoft.EntityFrameworkCore;

namespace MedHelper_API.Repository
{
    public class DiseaseRepository : BaseRepository<Disease>, IDiseaseRepository
    {
        public DiseaseRepository(MedHelperDB context) : base(context)
        {
        }
    }
}