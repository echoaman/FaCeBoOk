using System.Collections.Generic;
using System.Threading.Tasks;
using profile_service.Models;

namespace profile_service.Interfaces
{
    public interface IUserService
    {
        public Task<List<User>> GetAllUsers();
        public Task<User> GetUser(string uid);
        public Task<Events> UpdateUser(User updatedUser);
        public Task<List<User>> SearchUser(string name);
        public Task<List<string>> GetFriends(string uid);
        public Task<Events> AddFriend(string uid, string newFriendId);
        public Task<User> Login(string email, string password);
        public Task<Events> Signup(User newUser);
    }
}