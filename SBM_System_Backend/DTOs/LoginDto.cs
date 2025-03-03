using System.ComponentModel.DataAnnotations;

namespace SBM_System_Backend.DTOs
{
    public class LoginDto
    {
        [EmailAddress(ErrorMessage = "Email ont in format")]
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
