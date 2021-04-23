using System.Collections.Generic;
using profile_service.Models;

namespace profile_service.Interfaces
{
	public interface IUserService
	{
		public List<User> GetUsers();
	}
}