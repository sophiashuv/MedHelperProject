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
    }
}