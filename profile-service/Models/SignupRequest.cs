using System.ComponentModel.DataAnnotations;

namespace profile_service.Models
{
    public class SignupRequest
    {
        [Required(ErrorMessage = "Name is required")]
        public string name { get; set; }
        [Required(ErrorMessage = "Password is required")]
        public string password { get; set; }
        [Required(ErrorMessage = "Email is required")]
        public string email { get; set; }
    }
}