using System.Collections.Generic;

namespace profile_service.Models
{
	public class User
	{
		public string UId { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public string Email { get; set; }
		public string Password { get; set; }
		public List<int> Friends { get; } = new List<int>();
	}
}