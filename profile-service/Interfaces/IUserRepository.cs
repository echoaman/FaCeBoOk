using System.Collections.Generic;
using System.Threading.Tasks;
using profile_service.Entities;

namespace profile_service.Interfaces
{
    public interface IUserRepository
    {
        public Task<List<User>> GetAllUsers();
        public Task<User> GetUser(string uid);
        public Task<User> UpdateUser(User updatedUser);
        public Task<List<User>> SearchUser(string name);
        public Task<List<string>> GetFriends(string uid);
        public Task<User> AddFriend(string uid, string newFriendId);
        public Task<User> Login(string email, string password);
        public Task<bool> Signup(User newUser);
    }
}