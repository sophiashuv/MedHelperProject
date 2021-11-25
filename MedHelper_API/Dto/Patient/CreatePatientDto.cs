using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MedHelper_API.Dto.Patient
{
    public class CreatePatientDto
    {
        [Required] 
        public string UserName { get; set; }
        
        [Required] 
        public string Gender { get; set; }
        
        [Required] 
        public System.DateTime Birthdate { get; set; }

        [Required] 
        public List<int> MedicineIds { get; set; }
        
        [Required] 
        public List<int> DiseasesIds { get; set; }
        
    }
}