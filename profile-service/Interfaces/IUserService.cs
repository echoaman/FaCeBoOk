using System.Collections.Generic;
using System.Threading.Tasks;
using profile_service.Models;

namespace profile_service.Interfaces
{
	public interface IUserService
	{
		public Task<User> Login(string email, string password);
		public Task<UserEvents> Signup(User newUser);
		public Task<User> GetUser(string UId);
		public Task<List<User>> GetFriends(string UId);
		public Task<UserEvents> AddFriend(string UId, string NewFriend);
		public Task<List<User>> GetAllUsers();
	}
}