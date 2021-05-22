using System.ComponentModel.DataAnnotations;

namespace profile_service.Models
{
    public class LoginRequest
    {
        [Required(ErrorMessage = "Password is needed")]
        public string password { get; set; }
        [Required(ErrorMessage = "Email is required")]
        public string email { get; set; }
    }
}