using System.ComponentModel.DataAnnotations;

namespace profile_service.Models
{
    public class AddFriendRequest
    {
        [Required(ErrorMessage = "User's id is required")]
        public string uid { get; set; }
        [Required(ErrorMessage = "Friend's id is required")]
        public string newFriendId { get; set; }
    }
}