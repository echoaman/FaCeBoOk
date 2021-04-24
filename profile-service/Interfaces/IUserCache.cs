using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using profile_service.Models;

namespace profile_service.Interfaces
{
	public interface IUserCache
	{
		public Task<bool> SetAllUsers(List<User> users, TimeSpan expiration);
		public Task<List<User>> GetAllUsers();
		public Task<bool> SetUser(User user, TimeSpan expiration);
		public Task<User> GetUser(string UId);
		public Task<List<User>> GetFriends(string UId);
	}
}