using System.ComponentModel.DataAnnotations;

namespace MedHelper_API.Dto.Auth
{
    public class LoginDto
    {
        [Required] 
        [EmailAddress]
        public string Email { get; set; }
        
        [Required]
        [StringLength(40, ErrorMessage = "Password is too short (minimum is 8 characters).", MinimumLength =8)]
        public string Pass { get; set; }
    }
}